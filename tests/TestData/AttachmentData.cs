using Enfo.Domain.EnforcementOrders.Entities;

namespace EnfoTests.TestData;

internal static class AttachmentData
{
    public static readonly List<Attachment> Attachments = new()
    {
        new Attachment
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
            FileName = "FileOne.pdf",
            FileExtension = ".pdf",
            Size = 1,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 1),
            DateUploaded = DateTime.Today.AddDays(-2),
        },
        new Attachment
        {
            Id = new Guid("00000000-0000-0000-0000-000000000002"),
            FileName = "File-Two.pdf",
            FileExtension = ".pdf",
            Size = 10,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 1),
            DateUploaded = DateTime.Today.AddDays(-1),
        },
        new Attachment
        {
            Id = new Guid("00000000-0000-0000-0000-000000000003"),
            FileName = "File Three.pdf",
            FileExtension = ".pdf",
            Size = 100,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 2),
            DateUploaded = DateTime.Today.AddDays(-1),
        },
        new Attachment
        {
            Id = new Guid("00000000-0000-0000-0000-000000000004"),
            FileName = "FileFourDeleted.pdf",
            FileExtension = ".pdf",
            Size = 1000,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 3),
            DateUploaded = DateTime.Today.AddDays(-2),
            Deleted = true,
            DateDeleted = DateTime.Today.AddDays(-1),
        },
    };

    internal record AttachmentFile(string FileName, string? Base64EncodedFile);

    internal static readonly List<AttachmentFile> AttachmentFiles = new()
    {
        new AttachmentFile("00000000-0000-0000-0000-000000000001.pdf", EncodedPdfFile),
        new AttachmentFile("00000000-0000-0000-0000-000000000002.pdf", null),
        new AttachmentFile("00000000-0000-0000-0000-000000000003.pdf", ""),
    };

    private const string EncodedPdfFile =
        "JVBERi0xLjIKMSAwIG9iago8PD4+CnN0cmVhbQpCVC9GMSAyNCBUZiAxMCA4IFREIChIZWxsbyB3b3JsZCEpJyBFVAplbmRzdHJlYW0KZW5kb2JqCjQgMCBvYmoKPDwvVHlwZSAvUGFnZS9QYXJlbnQgMiAwIFIvQ29udGVudHMgMSAwIFI+PgplbmRvYmoKMiAwIG9iago8PC9LaWRzIFs0IDAgUl0vQ291bnQgMS9UeXBlIC9QYWdlcy9NZWRpYUJveCBbMCAwIDI1MCA1MF0+PgplbmRvYmoKMyAwIG9iago8PC9QYWdlcyAyIDAgUi9UeXBlIC9DYXRhbG9nPj4KZW5kb2JqCnRyYWlsZXIKPDwvUm9vdCAzIDAgUj4+CiUlRU9G";
}
