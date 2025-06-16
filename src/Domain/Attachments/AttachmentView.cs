using Enfo.Domain.Utils;

namespace Enfo.Domain.Attachments;

public class AttachmentView
{
    public AttachmentView(Attachment a)
    {
        Id = a.Id;
        EnforcementOrderId = a.EnforcementOrder.Id;
        FileName = a.FileName;
        FileExtension = a.FileExtension;
        Size = a.Size;
        DateUploaded = a.DateUploaded;
    }

    public Guid Id { get; }
    
    [JsonIgnore]
    public int EnforcementOrderId { get; }

    public string FileName { get; }

    [UIHint("FileTypeIcon")]
    public string FileExtension { get; }

    public string AttachmentFileName => string.Concat(Id.ToString(), FileExtension);

    [UIHint("FileSize")]
    public long Size { get; }

    [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
    public DateTime DateUploaded { get; }
}
