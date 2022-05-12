using Enfo.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace Enfo.LocalRepository.Attachments;

public class FileService : IFileService
{
    public async Task<byte[]> GetFileAsync(string fileName)
    {
        try
        {
            if (AttachmentData.AttachmentFiles.All(e => e.FileName != fileName)) return Array.Empty<byte>();
            var base64EncodedFile = AttachmentData.AttachmentFiles
                .Single(e => e.FileName == fileName).Base64EncodedFile;
            return base64EncodedFile is null ? Array.Empty<byte>() : Convert.FromBase64String(base64EncodedFile);
        }
        catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
        {
            return Array.Empty<byte>();
        }
    }

    public async Task TryDeleteFileAsync(string path) => throw new NotImplementedException();

    public async Task SaveFileAsync(IFormFile file) => throw new NotImplementedException();
}
