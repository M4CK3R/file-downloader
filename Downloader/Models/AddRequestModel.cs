namespace Downloader.Models;

public class AddRequestModel
{
    public string Url { get; set; }
    public string Name { get; set; }
    public bool Retry { get; set; } = false;
    public int? MaxTries { get; set; }
}