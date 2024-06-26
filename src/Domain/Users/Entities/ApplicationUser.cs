﻿using Microsoft.AspNetCore.Identity;

namespace Enfo.Domain.Users.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class.
// (IdentityUser already includes Id, Email, and UserName properties.)
public class ApplicationUser : IdentityUser<Guid>
{
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

    /// <summary>
    /// "oid: The object identifier for the user in Azure AD. This value is the immutable and non-reusable identifier
    /// of the user. Use this value, not email, as a unique identifier for users; email addresses can change.
    /// If you use the Azure AD Graph API in your app, object ID is that value used to query profile information."
    /// https://learn.microsoft.com/en-us/azure/architecture/multitenant-identity/claims
    ///
    /// In ASP.NET Core, the OpenID Connect middleware converts some of the claim types when it populates the
    /// Claims collection for the user principal:
    /// oid > http://schemas.microsoft.com/identity/claims/objectidentifier
    /// 
    /// <para>ID token claims reference: https://learn.microsoft.com/en-us/entra/identity-platform/id-token-claims-reference#use-claims-to-reliably-identify-a-user</para>
    /// </summary>
    [PersonalData]
    public string ObjectId { get; set; }

    public string DisplayName =>
        string.Join(" ", new[] { GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));
}
