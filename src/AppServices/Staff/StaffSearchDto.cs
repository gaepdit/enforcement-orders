using System.ComponentModel.DataAnnotations;

namespace Enfo.AppServices.Staff;

public record StaffSearchDto(
    // Fields
    string? Name,
    [EmailAddress] string? Email,
    string? Role
)
{
    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Name), Name },
        { nameof(Email), Email },
        { nameof(Role), Role },
    };

    public StaffSearchDto TrimAll() => this with
    {
        Name = Name?.Trim(),
        Email = Email?.Trim(),
    };
}
