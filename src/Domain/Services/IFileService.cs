using Microsoft.AspNetCore.Http;

namespace Enfo.Domain.Services;

public interface IFileService
{
    Task<byte[]> GetFileAsync(string fileName) => Task.FromResult(Array.Empty<byte>());
    Task TryDeleteFileAsync(string path) => Task.CompletedTask;
    Task SaveFileAsync(IFormFile file) => Task.CompletedTask;
}
