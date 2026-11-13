using Enfo.Domain.Utils;

namespace Enfo.Domain.Attachments;

public class AttachmentView(Attachment a)
{
    public Guid Id { get; } = a.Id;

    [JsonIgnore]
    public int EnforcementOrderId { get; } = a.EnforcementOrder.Id;

    public string FileName { get; } = a.FileName;

    [JsonIgnore]
    [UIHint("FileTypeIcon")]
    public string FileExtension { get; } = a.FileExtension;

    public string AttachmentFileName => string.Concat(Id.ToString(), FileExtension);

    [UIHint("FileSize")]
    public long Size { get; } = a.Size;

    [DisplayFormat(DataFormatString = DisplayFormats.ShortDateComposite)]
    public DateTime DateUploaded { get; } = a.DateUploaded;
}
