using Enfo.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace Enfo.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _basePath;

    public FileService(string basePath) => _basePath = basePath;

    public async Task<byte[]> GetFileAsync(string fileName)
    {
        try
        {
            return await File.ReadAllBytesAsync(Path.Combine(_basePath, fileName));
        }
        catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
        {
            return Array.Empty<byte>();
        }
    }

    public async Task TryDeleteFileAsync(string path) => throw new NotImplementedException();

    public async Task SaveFileAsync(IFormFile file) => throw new NotImplementedException();
}
