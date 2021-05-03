using System;
using Enfo.Domain.Entities.Users;

namespace Enfo.Domain.Resources.Users
{
    public class UserView
    {
        public UserView(ApplicationUser user) =>
            (Id, Name, Email) = (user.Id, user.DisplayName, user.Email);

        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
    }
}
