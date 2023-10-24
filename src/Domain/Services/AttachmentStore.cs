using GaEpd.FileService;
using Microsoft.AspNetCore.Http;

namespace Enfo.Domain.Services;

public class AttachmentStore : IAttachmentStore
{
    private readonly IFileService _fileService;
    public AttachmentStore(IFileService fileService) => _fileService = fileService;

    public async Task SaveFileAttachmentAsync(IFormFile file, Guid fileId)
    {
        if (file.Length == 0 || string.IsNullOrWhiteSpace(file.FileName)) return;
        var fileName = string.Concat(fileId.ToString(), Path.GetExtension(file.FileName));
        await _fileService.SaveFileAsync(file.OpenReadStream(), fileName);
    }

    public async Task<byte[]> GetFileAttachmentAsync(string fileName)
    {
        await using var response = await _fileService.TryGetFileAsync(fileName);
        if (!response.Success) return Array.Empty<byte>();

        using var ms = new MemoryStream();
        await response.Value.CopyToAsync(ms);
        return ms.ToArray();
    }

    public Task TryDeleteFileAttachmentAsync(string fileName)
    {
        return _fileService.DeleteFileAsync(fileName);
    }
}
