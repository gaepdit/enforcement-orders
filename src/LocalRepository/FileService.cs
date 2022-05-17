using Enfo.Domain.Services;
using Microsoft.AspNetCore.Http;
using TestData;

namespace Enfo.LocalRepository;

public class FileService : IFileService
{
    internal List<AttachmentData.AttachmentFile> Files { get; }

    public FileService() => Files = new List<AttachmentData.AttachmentFile>(AttachmentData.AttachmentFiles);

    public Task<byte[]> GetFileAsync(string fileName)
    {
        try
        {
            if (Files.All(e => e.FileName != fileName)) return Task.FromResult(Array.Empty<byte>());
            var base64EncodedFile = Files
                .Single(e => e.FileName == fileName).Base64EncodedFile;
            return Task.FromResult(base64EncodedFile is null
                ? Array.Empty<byte>()
                : Convert.FromBase64String(base64EncodedFile));
        }
        catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
        {
            return Task.FromResult(Array.Empty<byte>());
        }
    }

    public void TryDeleteFile(string fileName) =>
        Files.Remove(Files.SingleOrDefault(a => a.FileName == fileName));

    public async Task SaveFileAsync(IFormFile file, Guid fileId)
    {
        if (file.Length == 0 || string.IsNullOrWhiteSpace(file.FileName)) return;

        var stream = new MemoryStream(Convert.ToInt32(file.Length));
        await file.CopyToAsync(stream);

        var attachmentFile = new AttachmentData.AttachmentFile
        {
            FileName = string.Concat(fileId.ToString(), Path.GetExtension(file.FileName)),
            Base64EncodedFile = Convert.ToBase64String(stream.ToArray()),
        };

        Files.Add(attachmentFile);
    }
}
