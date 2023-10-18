using Downloader.Data.Models;
using Downloader.Models;
using Downloader.Services.Interfaces;
using Downloader.Workers;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Downloader.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly DownloadQueueManager _downloadQueueManager;

    public RequestController(ILogger<RequestController> logger,
        IRequestService requestService,
        DownloadQueueManager downloadQueueManager,
        IServerSentEventsService serverSentEventsService)
    {
        _requestService = requestService;
        _downloadQueueManager = downloadQueueManager;
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<RequestModel>> GetAll()
    {
        var inProgress = _downloadQueueManager.GetAll().Select(dt => new RequestModel(dt));
        var inDatabase = (await _requestService.GetAllAsync()).Select(r => new RequestModel(r));

        var result = inProgress
            .Union(
                inDatabase,
                comparer: new ValueComparer<RequestModel>(
                    (a, b) => a!.Id == b!.Id,
                    a => a.Id.GetHashCode()
                )
            )
            .ToList();
        return result;
    }

    [HttpGet("Progress")]
    public DownloadTask? Progress(Guid id)
    {
        return _downloadQueueManager.GetDownloadTask(id);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<RequestModel>> Add(AddRequestModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Url) || string.IsNullOrWhiteSpace(model.Name))
            return BadRequest("Url and Name must be specified");
        try
        {
            var request = await _requestService.CreateAsync(new Request()
            {
                Name = model.Name,
                Retry = model.Retry,
                Url = model.Url,
                MaxTries = model.MaxTries
            });
            return Ok(new RequestModel(_downloadQueueManager.DownloadLink(request)));
        }
        catch (DbUpdateException)
        {
            return BadRequest("Request with that name is already in present");
        }
        catch (Exception)
        {
            return BadRequest("Unknown error occurred");
        }
    }
    
    [HttpDelete("Delete")]
    public async Task<ActionResult<RequestModel?>> Delete([FromQuery] Guid id)
    {
        var downloadTask = _downloadQueueManager.Cancel(id);
        var request = await _requestService.DeleteAsync(id);
        if (request != null || downloadTask != null)
            return Ok(new RequestModel(downloadTask ?? request!));
        
        return NotFound();
    }
}