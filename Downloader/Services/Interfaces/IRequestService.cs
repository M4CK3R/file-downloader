using Downloader.Data.Models;

namespace Downloader.Services.Interfaces;

public interface IRequestService
{
    Task<IEnumerable<Request>> GetAllAsync(bool includeDone = true);
    Task<Request?> GetByIdAsync(Guid id);
    Task<Request> CreateAsync(Request request);
    Task<Request> UpdateAsync(Request request, bool noSave = false);
    Task<Request?> DeleteAsync(Guid id, bool noSave = false);
    Task<Request> DeleteAsync(Request request, bool noSave = false);
}