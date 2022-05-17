using Enfo.Domain.BaseEntities;

namespace Enfo.Domain.EnforcementOrders.Entities;

public class Attachment : IdentifiedEntity<Guid>, IAuditable
{
    [Required]
    [StringLength(245)]
    public string FileName { get; set; }

    [Required]
    [StringLength(10)]
    public string FileExtension { get; set; }

    public long Size { get; set; }

    public EnforcementOrder EnforcementOrder { get; set; }

    public DateTime DateUploaded { get; set; }

    public bool Deleted { get; set; }

    public DateTime? DateDeleted { get; set; }
}
