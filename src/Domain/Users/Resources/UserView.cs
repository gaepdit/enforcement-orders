using Enfo.Domain.Users.Entities;

namespace Enfo.Domain.Users.Resources;

public class UserView
{
    public UserView(ApplicationUser user)
    {
        Id = user.Id;
        Name = user.DisplayName;
        Email = user.Email;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Email { get; }
}
