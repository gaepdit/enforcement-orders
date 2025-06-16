using Enfo.Domain.BaseEntities;
using Enfo.Domain.EnforcementOrders.Entities;

namespace Enfo.Domain.Attachments;

public class Attachment : IdentifiedEntity<Guid>, IAuditable
{
    [Required]
    [StringLength(245)]
    public string FileName { get; set; }

    [Required]
    [StringLength(10)]
    public string FileExtension { get; set; }

    public string AttachmentFileName => string.Concat(Id.ToString(), FileExtension);
    
    public long Size { get; set; }

    public EnforcementOrder EnforcementOrder { get; set; }

    public DateTime DateUploaded { get; set; }

    public bool Deleted { get; set; }

    public DateTime? DateDeleted { get; set; }
}
