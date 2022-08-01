using Enfo.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace Enfo.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _attachmentsBasePath;

    public FileService(string attachmentsBasePath) => _attachmentsBasePath = attachmentsBasePath;

    public async Task<byte[]> GetFileAsync(string fileName)
    {
        try
        {
            return await File.ReadAllBytesAsync(Path.Combine(_attachmentsBasePath, fileName));
        }
        catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
        {
            return Array.Empty<byte>();
        }
    }

    public void TryDeleteFile(string fileName)
    {
        if (!string.IsNullOrWhiteSpace(fileName)) File.Delete(Path.Combine(_attachmentsBasePath, fileName));
    }

    public async Task SaveFileAsync(IFormFile file, Guid fileId)
    {
        if (file.Length == 0 || string.IsNullOrWhiteSpace(file.FileName)) return;

        Directory.CreateDirectory(_attachmentsBasePath);

        var fileName = file.FileName.Trim();
        var fileExtension = Path.GetExtension(fileName);
        var savePath = Path.Combine(_attachmentsBasePath, string.Concat(fileId.ToString(), fileExtension));

        await using var stream = new FileStream(savePath, FileMode.Create);
        await file.CopyToAsync(stream);
    }
}
