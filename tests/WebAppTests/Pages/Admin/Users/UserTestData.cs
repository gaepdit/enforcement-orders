using System;
using System.Collections.Generic;
using Enfo.Domain.Entities.Users;

namespace EnfoTests.WebApp.Pages.Admin.Users
{
    public static class UserTestData
    {
        public static List<ApplicationUser> ApplicationUsers => new()
        {
            new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "example.one@example.com",
                GivenName = "Sample",
                FamilyName = "User"
            },
            new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "example.two@example.com",
                GivenName = "Another",
                FamilyName = "Sample"
            }
        };
    }
}