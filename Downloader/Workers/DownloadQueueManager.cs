using System.Data;
using Downloader.Data.Models;
using Downloader.Models;
using Downloader.Services.Interfaces;
using Lib.AspNetCore.ServerSentEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Downloader.Workers;

public class DownloadQueueManager
{
    private const int BUFFER_SIZE = 1024 * 1024;
    
    private readonly string _finishedPath;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DownloadQueueManager> _logger;
    private readonly Dictionary<Guid, DownloadTask> _jobs;
    private readonly HttpClient _client;
    private readonly IServerSentEventsService _serverSentEventsService;

    public DownloadQueueManager(ILogger<DownloadQueueManager> logger,
        IHttpClientFactory clientFactory,
        IServiceProvider serviceProvider,
        IServerSentEventsService serverSentEventsService,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _jobs = new();
        _client = clientFactory.CreateClient(nameof(DownloadQueueManager));
        _serviceProvider = serviceProvider;
        _serverSentEventsService = serverSentEventsService;
        _finishedPath = configuration.GetValue<string>("FilesPath") ??
                        throw new NoNullAllowedException(nameof(_finishedPath));
    }

    public DownloadTask DownloadLink(Request request)
    {
        _logger.LogInformation("Adding {Url}", request.Url);
        var downloadTask = DownloadTask.FromRequest(request);
        _jobs.Add(request.Id, downloadTask);
        StartDownload(request.Id);
        _logger.LogInformation("Added {Url}", request.Url);
        return downloadTask;
    }

    public DownloadTask? Cancel(Guid id)
    {
        if (!_jobs.TryGetValue(id, out var downloadTask))
            return null;
        downloadTask.CancellationTokenSource.Cancel();
        RemoveFile(downloadTask);
        _jobs.Remove(id);
        return downloadTask;
    }

    private void RemoveFile(Request downloadTask)
    {
        var path = Path.Join(_finishedPath, downloadTask.Name);
        if (File.Exists(path))
            File.Delete(path);
    }

    private void StartDownload(Guid id)
    {
        DownloadLink(id);
    }

    private async Task DownloadLink(Guid id, CancellationToken? token = null)
    {
        if (!_jobs.TryGetValue(id, out var downloadTask))
            return;
        var t = token ?? downloadTask.CancellationTokenSource.Token;
        try
        {
            var response = await MakeRequest(downloadTask, t);
            await using var stream = await response.Content.ReadAsStreamAsync(t);
            await using var fs = new FileStream(Path.Join(_finishedPath, downloadTask.Name),
                FileMode.OpenOrCreate);
            var length = GetLength(response, downloadTask, stream, fs);

            while (true)
            {
                if (await DownloadStream(stream, downloadTask, length, fs, t)) break;
            }

            await SaveDownloadTask(downloadTask);
            await _serverSentEventsService.SendEventAsync(
                new ServerSentEvent()
                {
                    Id = downloadTask.Id.ToString(),
                    Type = nameof(DownloadTask),
                    Data = new List<string>()
                    {
                        JsonConvert.SerializeObject(new RequestModel(downloadTask), Formatting.None,
                            new JsonSerializerSettings()
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            })
                    }
                },
                t);
            _jobs.Remove(id);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Download of {Url} was cancelled", downloadTask.Url);
            _jobs.Remove(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred executing task work item");
            Retry(id, downloadTask);
        }
    }

    private async Task SaveDownloadTask(Request downloadTask)
    {
        if (downloadTask.BytesTotal <= 0)
            downloadTask.BytesTotal = downloadTask.BytesDownloaded;

        downloadTask.FinishedAt = downloadTask.IsDone ? DateTime.UtcNow : null;
        using var scope = _serviceProvider.CreateScope();
        var requestService = scope.ServiceProvider.GetRequiredService<IRequestService>();
        await requestService.UpdateAsync(downloadTask);
    }

    private void Retry(Guid id, Request downloadTask)
    {
        downloadTask.CurrentTry++;
        if (downloadTask.Retry && (downloadTask.MaxTries <= 0 ||
                                   downloadTask.CurrentTry < downloadTask.MaxTries))
        {
            StartDownload(id);
        }
    }

    private static async Task<bool> DownloadStream(
        Stream stream,
        Request downloadTask,
        long length,
        Stream fs,
        CancellationToken token)
    {
        var buffer = new byte[BUFFER_SIZE];
        var read = await stream.ReadAsync(buffer, token);
        if (read == 0 || downloadTask.BytesDownloaded >= length) return true;
        downloadTask.BytesDownloaded += read;
        await fs.WriteAsync(buffer.AsMemory(0, read), token);
        return false;
    }

    private static long GetLength(HttpResponseMessage response, Request downloadTask, Stream stream, Stream fs)
    {
        var length = response.Content.Headers.ContentLength ?? -1;
        downloadTask.BytesTotal = stream.CanSeek ? stream.Length : length;
        if (downloadTask.BytesDownloaded == 0 || !stream.CanSeek || !fs.CanSeek)
        {
            downloadTask.BytesDownloaded = 0;
            return length;
        }

        stream.Seek(downloadTask.BytesDownloaded, SeekOrigin.Begin);
        fs.Seek(downloadTask.BytesDownloaded, SeekOrigin.Begin);

        return length;
    }

    private async Task<HttpResponseMessage> MakeRequest(Request downloadTask, CancellationToken token)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, downloadTask.Url);
        var response =
            await _client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, token);
        return response;
    }

    public DownloadTask? GetDownloadTask(Guid key)
    {
        return _jobs.TryGetValue(key, out var task) ? task : null;
    }

    public IEnumerable<DownloadTask> GetAll()
    {
        return _jobs.Values;
    }
}

public class DownloadTask : Request
{
    private DownloadTask(Request r)
    {
        Id = r.Id;
        DownloadStartedAt = DateTime.UtcNow;
        BytesTotal = r.BytesTotal;
        BytesDownloaded = r.BytesDownloaded;
        Url = r.Url;
        Name = r.Name;
        MaxTries = r.MaxTries;
        CurrentTry = r.CurrentTry;
        Retry = r.Retry;
    }

    public float Progress => (float)BytesDownloaded / BytesTotal;
    public DateTime DownloadStartedAt { get; set; }
    public double BytesPerSecond => BytesDownloaded / (DateTime.UtcNow - DownloadStartedAt).TotalSeconds;
    public double MegaBytesPerSecond => BytesPerSecond / 1024 / 1024;
    public CancellationTokenSource CancellationTokenSource { get; set; } = new();
    public static DownloadTask FromRequest(Request r) => new(r);
}