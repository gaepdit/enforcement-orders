using Enfo.Domain.Users.Entities;

namespace Enfo.Domain.Users.Resources;

public record UserView
{
    public UserView(ApplicationUser user)
    {
        Id = user.Id;
        Name = user.DisplayName;
        Email = user.Email;
        ObjectId = user.ObjectId;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Email { get; }
    public string ObjectId { get; set; }
}
