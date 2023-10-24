using Microsoft.AspNetCore.Http;

namespace Enfo.Domain.Services;

public interface IAttachmentStore
{
    Task<byte[]> GetFileAttachmentAsync(string fileName);
    Task TryDeleteFileAttachmentAsync(string fileName);
    Task SaveFileAttachmentAsync(IFormFile file, Guid fileId);
}
