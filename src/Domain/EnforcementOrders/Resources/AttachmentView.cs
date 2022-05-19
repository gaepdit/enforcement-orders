using Enfo.Domain.EnforcementOrders.Entities;

namespace Enfo.Domain.EnforcementOrders.Resources;

public class AttachmentView
{
    public AttachmentView(Attachment a)
    {
        Id = a.Id;
        FileName = a.FileName;
        FileExtension = a.FileExtension;
        Size = a.Size;
    }

    public Guid Id { get; }

    public string FileName { get; }

    [UIHint("FileTypeIcon")]
    public string FileExtension { get; }

    public string AttachmentFileName => string.Concat(Id.ToString(), FileExtension);

    [UIHint("FileSize")]
    public long Size { get; }
}
