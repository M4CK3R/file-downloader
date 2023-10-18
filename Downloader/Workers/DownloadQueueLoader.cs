using Downloader.Services.Interfaces;

namespace Downloader.Workers;

public class DownloadQueueLoader : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    
    public DownloadQueueLoader(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var requestService = scope.ServiceProvider.GetRequiredService<IRequestService>();
        var requests = await requestService.GetAllAsync(false);
        var downloadQueueManager = _serviceProvider.GetRequiredService<DownloadQueueManager>();
        foreach (var request in requests)
        {
            downloadQueueManager.DownloadLink(request);
        }
    }
}