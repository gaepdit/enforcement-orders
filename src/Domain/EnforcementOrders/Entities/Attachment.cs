using Enfo.Domain.BaseEntities;
using Enfo.Domain.Users.Entities;

namespace Enfo.Domain.EnforcementOrders.Entities;

public class Attachment : IdentifiedEntity<Guid>
{
    [Required]
    [StringLength(245)]
    public string FileName { get; set; }

    [Required]
    [StringLength(10)]
    public string FileExtension { get; set; }

    public long Size { get; set; }

    public EnforcementOrder EnforcementOrder { get; set; }

    [CanBeNull]
    public ApplicationUser UploadedBy { get; set; }

    public DateTime DateUploaded { get; set; }

    public bool Deleted { get; set; }

    [CanBeNull]
    public ApplicationUser DeletedBy { get; set; }

    public DateTime? DateDeleted { get; set; }
}
