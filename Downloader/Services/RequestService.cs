using System.Data;
using Downloader.Data;
using Downloader.Data.Models;
using Downloader.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Downloader.Services;

public class RequestService : IRequestService
{
    private readonly IRepository<Request> _repository;
    private readonly string _downloadPath;

    public RequestService(IRepository<Request> repository, IConfiguration configuration)
    {
        _repository = repository;
        _downloadPath = configuration.GetValue<string>("FilesPath") ?? throw new NoNullAllowedException(nameof(_downloadPath));
    }

    public async Task<IEnumerable<Request>> GetAllAsync(bool includeDone = true)
    {
        var res = _repository.Get();
        if (!includeDone)
            res = res.Where(x => !x.IsDone);
        return await res.ToListAsync();
    }

    public async Task<Request?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Request> CreateAsync(Request request)
    {
        return await _repository.AddAsync(request);
    }

    public async Task<Request> UpdateAsync(Request request, bool noSave = false)
    {
        return await _repository.UpdateAsync(request, noSave);
    }

    public async Task<Request?> DeleteAsync(Guid id, bool noSave = false)
    {
        var request = await _repository.GetByIdAsync(id);
        if (request == null)
            return null;
        return await this.DeleteAsync(request, noSave);
    }

    public async Task<Request> DeleteAsync(Request request, bool noSave = false)
    {
        var r = await _repository.DeleteAsync(request, noSave);
        // Delete the file associated with the request
        var filePath = Path.Join(_downloadPath, r.Name);
        if (File.Exists(filePath))
            File.Delete(filePath);
        return r;
    }
}