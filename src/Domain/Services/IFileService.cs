using Microsoft.AspNetCore.Http;

namespace Enfo.Domain.Services;

public interface IFileService
{
    Task<byte[]> GetFileAsync(string fileName);
    void TryDeleteFile(string fileName);
    Task SaveFileAsync(IFormFile file, Guid fileId);
}
