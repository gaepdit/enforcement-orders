using GaEpd.FileService;
using Microsoft.AspNetCore.Http;

namespace Enfo.Domain.Attachments;

public interface IAttachmentStore
{
    Task<byte[]> GetFileAttachmentAsync(string fileName);
    Task TryDeleteFileAttachmentAsync(string fileName);
    Task SaveFileAttachmentAsync(IFormFile file, Guid fileId);
}

public class AttachmentStore(IFileService fileService) : IAttachmentStore
{
    public async Task SaveFileAttachmentAsync(IFormFile file, Guid fileId)
    {
        if (file.Length == 0 || string.IsNullOrWhiteSpace(file.FileName)) return;
        var fileName = string.Concat(fileId.ToString(), Path.GetExtension(file.FileName));

        await fileService.SaveFileAsync(file.OpenReadStream(), fileName, ExpandPath(fileName)).ConfigureAwait(false);
    }

    public async Task<byte[]> GetFileAttachmentAsync(string fileName)
    {
        var response = await fileService.TryGetFileAsync(fileName, ExpandPath(fileName)).ConfigureAwait(false);

        await using var _ = response.ConfigureAwait(false);
        if (!response.Success) return [];

        using var ms = new MemoryStream();
        await response.Value.CopyToAsync(ms).ConfigureAwait(false);
        return ms.ToArray();
    }

    public Task TryDeleteFileAttachmentAsync(string fileName) =>
        fileService.DeleteFileAsync(fileName, ExpandPath(fileName));

    private static string ExpandPath(string fileId) => fileId[..2];
}
