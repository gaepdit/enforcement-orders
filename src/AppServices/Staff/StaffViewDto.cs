using Enfo.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace Enfo.AppServices.Staff;

public record StaffViewDto
{
    public StaffViewDto(ApplicationUser user)
    {
        Id = user.Id;
        Name = user.DisplayName;
        Email = user.Email;
    }

    public Guid Id { get; }
    public string Name { get; }

    [Display(Name = "Email (cannot be changed)")]
    public string? Email { get; }
}
