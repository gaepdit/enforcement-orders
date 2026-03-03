using System.Security.Claims;

namespace Enfo.AppServices.AuthenticationServices.Claims;

public static class ClaimsPrincipalExtensions
{
    // Identity Provider claim types
    private const string IdentityProviderId = "idp";
    private const string TenantId = "http://schemas.microsoft.com/identity/claims/tenantid";

    extension(ClaimsPrincipal principal)
    {
        public string? GetIdentityProviderId() => principal.GetClaimValue(IdentityProviderId, TenantId);

        public string? GetAuthenticationMethod() => principal.FindFirstValue(ClaimTypes.AuthenticationMethod);

        public string? GetEmail() => principal.FindFirstValue(ClaimTypes.Email);

        public string GetGivenName() => principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;

        public string GetFamilyName() => principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;

        private string? GetClaimValue(params string[] claimNames) => claimNames
            .Select(principal.FindFirstValue)
            .FirstOrDefault(currentValue => !string.IsNullOrEmpty(currentValue));
    }
}
