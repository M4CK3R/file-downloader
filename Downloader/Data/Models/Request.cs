using System.ComponentModel.DataAnnotations.Schema;

namespace Downloader.Data.Models;

public class Request : Entity
{
    public string Url { get; set; }
    public string Name { get; set; }
    public int? MaxTries { get; set; }
    public int CurrentTry { get; set; } = 0;
    public bool Retry { get; set; } = true;
    public long BytesDownloaded { get; set; } = 0;
    public long BytesTotal { get; set; } = 1;
    public DateTime? FinishedAt { get; set; }
    public bool IsDone
    {
        get => BytesDownloaded == BytesTotal;
        set { }
    }
    public long MegaBytesDownloaded => BytesDownloaded / 1024 / 1024;
};