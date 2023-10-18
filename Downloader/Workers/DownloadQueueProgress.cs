using Downloader.Models;
using Lib.AspNetCore.ServerSentEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Downloader.Workers;

public class DownloadQueueProgress : BackgroundService
{
    private readonly IServerSentEventsService _serverSentEventsService;
    private readonly DownloadQueueManager _downloadQueueManager;

    public DownloadQueueProgress(
        IServerSentEventsService serverSentEventsService,
        DownloadQueueManager downloadQueueManager)
    {
        _serverSentEventsService = serverSentEventsService;
        _downloadQueueManager = downloadQueueManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateProgress(stoppingToken);
            await Task.Delay(500);
        }
    }

    private async Task UpdateProgress(CancellationToken stoppingToken)
    {
        var dts = _downloadQueueManager.GetAll().ToList();
        foreach (var downloadTask in dts)
        {
            await SendEvent(stoppingToken, downloadTask);
        }
    }

    private async Task SendEvent(CancellationToken stoppingToken, DownloadTask downloadTask)
    {
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
            stoppingToken);
    }
}