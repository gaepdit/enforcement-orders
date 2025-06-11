using Enfo.Domain.EnforcementOrders.Entities;

namespace EnfoTests.TestData;

public static class AttachmentData
{
    internal static List<Attachment> Attachments { get; } =
    [
        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
            FileName = "FileOne.pdf",
            FileExtension = ".pdf",
            Size = 1,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 1),
            DateUploaded = DateTime.Today.AddDays(-2),
        },

        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000002"),
            FileName = "File Two.pdf",
            FileExtension = ".pdf",
            Size = 10,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 1),
            DateUploaded = DateTime.Today.AddDays(-1),
        },

        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000003"),
            FileName = "File-Three-💻.pdf",
            FileExtension = ".pdf",
            Size = 100,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 2),
            DateUploaded = DateTime.Today.AddDays(-1),
        },

        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000004"),
            FileName = "FileFourDeleted.pdf",
            FileExtension = ".pdf",
            Size = 1000,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 2),
            DateUploaded = DateTime.Today.AddDays(-2),
            Deleted = true,
            DateDeleted = DateTime.Today.AddDays(-1),
        },

        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000005"),
            FileName = "FileFive-OrderNotPublic.pdf",
            FileExtension = ".pdf",
            Size = 1000000,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 3),
            DateUploaded = DateTime.Today.AddDays(-2),
        },

        new()
        {
            Id = new Guid("00000000-0000-0000-0000-000000000006"),
            FileName = "FileSix-OrderDeleted.pdf",
            FileExtension = ".pdf",
            Size = 1000000,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 4),
            DateUploaded = DateTime.Today.AddDays(-2),
        },
    ];

    public record AttachmentFile(string FileName, string Base64EncodedFile);

    private static List<AttachmentFile> _attachmentFiles;

    public static List<AttachmentFile> GetAttachmentFiles()
    {
        if (_attachmentFiles is not null) return _attachmentFiles;
        _attachmentFiles = SeedAttachmentFiles;
        return _attachmentFiles;
    }

    private static List<AttachmentFile> SeedAttachmentFiles =>
    [
        new("00000000-0000-0000-0000-000000000001.pdf", EncodedPdfFile),
        new("00000000-0000-0000-0000-000000000002.pdf", EncodedPdfFile),
        new("00000000-0000-0000-0000-000000000003.pdf", EncodedPdfFile),
        new("00000000-0000-0000-0000-000000000004.pdf", Base64EncodedFile: null),
        new("00000000-0000-0000-0000-000000000005.pdf", EncodedPdfFile),
        new("00000000-0000-0000-0000-000000000006.pdf", EncodedPdfFile),
    ];


    private const string EncodedPdfFile =
        "JVBERi0xLjIKMSAwIG9iago8PD4+CnN0cmVhbQpCVC9GMSAyNCBUZiAxMCA4IFREIChIZWxsbyB3b3JsZCEpJyBFVAplbmRzdHJlYW0KZW5" +
        "kb2JqCjQgMCBvYmoKPDwvVHlwZSAvUGFnZS9QYXJlbnQgMiAwIFIvQ29udGVudHMgMSAwIFI+PgplbmRvYmoKMiAwIG9iago8PC9LaWRzIF" +
        "s0IDAgUl0vQ291bnQgMS9UeXBlIC9QYWdlcy9NZWRpYUJveCBbMCAwIDI1MCA1MF0+PgplbmRvYmoKMyAwIG9iago8PC9QYWdlcyAyIDAgU" +
        "i9UeXBlIC9DYXRhbG9nPj4KZW5kb2JqCnRyYWlsZXIKPDwvUm9vdCAzIDAgUj4+CiUlRU9G";
}
