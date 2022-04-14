using Enfo.Domain.Users.Entities;

namespace Enfo.Domain.Users.Resources;

public class UserView
{
    public UserView(ApplicationUser user) => (Id, Name, Email) = (user.Id, user.DisplayName, user.Email);

    public Guid Id { get; }
    public string Name { get; }
    public string Email { get; }
}
