using Enfo.Domain.Users;

namespace WebAppTests.Pages.Admin.Users;

public static class UserTestData
{
    public static List<ApplicationUser> ApplicationUsers =>
    [
        new()
        {
            Email = "example.one@example.com",
            GivenName = "Sample",
            FamilyName = "User",
        },

        new()
        {
            Email = "example.two@example.com",
            GivenName = "Another",
            FamilyName = "Sample",
        },
    ];
}
