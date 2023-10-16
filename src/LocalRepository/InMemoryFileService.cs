using Enfo.Domain.Services;
using EnfoTests.TestData;
using Microsoft.AspNetCore.Http;

namespace Enfo.LocalRepository;

public class InMemoryFileService : IFileService
{
    public Task<byte[]> GetFileAsync(string fileName)
    {
        try
        {
            if (AttachmentData.AttachmentFiles.All(e => e.FileName != fileName))
                return Task.FromResult(Array.Empty<byte>());

            var base64EncodedFile = AttachmentData.AttachmentFiles
                .Single(e => e.FileName == fileName).Base64EncodedFile;

            return Task.FromResult(string.IsNullOrEmpty(base64EncodedFile)
                ? Array.Empty<byte>()
                : Convert.FromBase64String(base64EncodedFile));
        }
        catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
        {
            return Task.FromResult(Array.Empty<byte>());
        }
    }

    public void TryDeleteFile(string fileName)
    {
        // Method intentionally left empty.
    }

    public async Task SaveFileAsync(IFormFile file, Guid fileId)
    {
        if (file.Length == 0 || string.IsNullOrWhiteSpace(file.FileName)) return;

        var stream = new MemoryStream(Convert.ToInt32(file.Length));
        await file.CopyToAsync(stream);

        var attachmentFile = new AttachmentData.AttachmentFile(
            string.Concat(fileId.ToString(), Path.GetExtension(file.FileName)),
            Convert.ToBase64String(stream.ToArray())
        );

        AttachmentData.AttachmentFiles.Add(attachmentFile);
    }
}
