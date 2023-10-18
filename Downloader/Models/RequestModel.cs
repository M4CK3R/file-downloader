using Downloader.Data.Models;
using Downloader.Workers;

namespace Downloader.Models;

public class RequestModel
{
    public RequestModel()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Url = string.Empty;
        isDone = false;
        Progress = null;
    }

    public RequestModel(Request r)
    {
        Id = r.Id;
        Name = r.Name;
        Url = r.Url;
        isDone = r.IsDone;
        Progress = null;
    }

    public RequestModel(DownloadTask dt)
    {
        Id = dt.Id;
        Name = dt.Name;
        Url = dt.Url;
        isDone = dt.IsDone;
        Progress = new ProgressInfoModel()
        {
            BytesDownloaded = dt.BytesDownloaded,
            BytesTotal = dt.BytesTotal,
            DownloadStartedAt = dt.DownloadStartedAt
        };
    }
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public bool isDone { get; set; }
    public ProgressInfoModel? Progress { get; set; }
}
public class ProgressInfoModel
{
    public DateTime DownloadStartedAt { get; set; }
    public long BytesDownloaded { get; set; }
    public long BytesTotal { get; set; }
    public long MegaBytesDownloaded => BytesDownloaded / 1024 / 1024;
    public long MegaBytesTotal => BytesTotal / 1024 / 1024;
    public double Percentage => (double)BytesDownloaded / BytesTotal * 100;
    public double BytesPerSecond => BytesDownloaded / (DateTime.UtcNow - DownloadStartedAt).TotalSeconds;
    public double MegaBytesPerSecond => BytesPerSecond / 1024 / 1024;
}