using Microsoft.AspNetCore.Identity;

namespace Enfo.Domain.Users;

// Add profile data for application users by adding properties to the ApplicationUser class.
// (IdentityUser already includes ID, Email, and UserName properties.)
public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        // `IdentityUser()` (the default implementation of IdentityUser<TKey> which uses a string as a primary key)
        // sets `Id` and `SecurityStamp` in the constructor, but `IdentityUser<Guid>()` does not,
        // so we have to set it here.
        Id = Guid.NewGuid();
        SecurityStamp = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// A claim that specifies the given name of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname
    /// </summary>
    [ProtectedPersonalData]
    [StringLength(150)]
    public string GivenName { get; set; }

    /// <summary>
    /// A claim that specifies the surname of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname
    /// </summary>
    [ProtectedPersonalData]
    [StringLength(150)]
    public string FamilyName { get; set; }

    public DateTimeOffset? AccountCreatedAt { get; init; }
    public DateTimeOffset? AccountUpdatedAt { get; set; }
    public DateTimeOffset? ProfileUpdatedAt { get; set; }
    public DateTimeOffset? MostRecentLogin { get; set; }

    public string DisplayName =>
        string.Join(" ", new[] { GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));
}
