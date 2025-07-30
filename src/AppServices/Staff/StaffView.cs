using Enfo.Domain.Users;

namespace Enfo.AppServices.Staff;

public record StaffView
{
    public StaffView(ApplicationUser user)
    {
        Id = user.Id;
        Name = user.DisplayName;
        Email = user.Email ?? "unknown";
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Email { get; }
}
