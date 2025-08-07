using Microsoft.AspNetCore.Http;

namespace Enfo.Domain.Attachments;

public interface IAttachmentStore
{
    Task<byte[]> GetFileAttachmentAsync(string fileName);
    Task TryDeleteFileAttachmentAsync(string fileName);
    Task SaveFileAttachmentAsync(IFormFile file, Guid fileId);
}
