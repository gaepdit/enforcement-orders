using Enfo.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Enfo.AppServices.Staff;

/// <summary>
/// Provide methods for interacting with user accounts
/// </summary>
public interface IStaffService
{
    public Task<StaffViewDto> GetCurrentUserAsync();
    public Task<StaffViewDto?> FindAsync(Guid id);
    Task<List<StaffViewDto>> SearchAsync(StaffSearchDto spec);
    Task<IList<string>> GetRolesAsync(Guid id);
    public Task<IReadOnlyList<AppRole>> GetAppRolesAsync(Guid id);
    public Task<IdentityResult> UpdateRolesAsync(Guid id, Dictionary<string, bool> roleUpdates);
}

public class StaffService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    : IStaffService
{
    // Application User methods
    private ClaimsPrincipal? GetCurrentPrincipal() => httpContextAccessor.HttpContext?.User;

    private async Task<ApplicationUser?> GetCurrentApplicationUserAsync()
    {
        var principal = GetCurrentPrincipal();
        return principal == null ? null : await userManager.GetUserAsync(principal).ConfigureAwait(false);
    }

    // IUserService methods
    public async Task<StaffViewDto> GetCurrentUserAsync()
    {
        var user = await GetCurrentApplicationUserAsync().ConfigureAwait(false)
                   ?? throw new CurrentUserNotFoundException();
        return new StaffViewDto(user);
    }

    public async Task<StaffViewDto?> FindAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        return user == null ? null : new StaffViewDto(user);
    }

    public async Task<List<StaffViewDto>> SearchAsync(StaffSearchDto spec) =>
        FilterUsers(string.IsNullOrEmpty(spec.Role)
                ? userManager.Users
                : (await userManager.GetUsersInRoleAsync(spec.Role).ConfigureAwait(false)).AsQueryable(),
            spec.Name, spec.Email);

    [SuppressMessage("Performance",
        "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
    private static List<StaffViewDto>
        FilterUsers(IQueryable<ApplicationUser> users, string? nameFilter, string? emailFilter) =>
        users
            .Where(m => string.IsNullOrWhiteSpace(nameFilter)
                        || m.GivenName.ToLower().Contains(nameFilter.ToLower())
                        || m.FamilyName.ToLower().Contains(nameFilter.ToLower()))
            .Where(m => string.IsNullOrWhiteSpace(emailFilter) || m.Email == emailFilter)
            .OrderBy(m => m.FamilyName).ThenBy(m => m.GivenName)
            .Select(e => new StaffViewDto(e))
            .ToList();

    public async Task<IList<string>> GetRolesAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        return user is null ? [] : await userManager.GetRolesAsync(user).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<AppRole>> GetAppRolesAsync(Guid id) =>
        AppRole.RolesAsAppRoles(await GetRolesAsync(id).ConfigureAwait(false))
            .OrderBy(r => r.DisplayName, StringComparer.CurrentCultureIgnoreCase).ToList();

    public async Task<IdentityResult> UpdateRolesAsync(Guid id, Dictionary<string, bool> roleUpdates)
    {
        foreach (var (key, value) in roleUpdates)
        {
            var result = await UpdateUserRoleAsync(id, key, value).ConfigureAwait(false);
            if (result != IdentityResult.Success) return result;
        }

        return IdentityResult.Success;
    }

    private async Task<IdentityResult> UpdateUserRoleAsync(Guid id, string role, bool addToRole)
    {
        var user = await userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        if (user == null) return IdentityResult.Failed(userManager.ErrorDescriber.DefaultError());

        var isInRole = await userManager.IsInRoleAsync(user, role).ConfigureAwait(false);
        if (addToRole == isInRole) return IdentityResult.Success;

        return addToRole switch
        {
            true => await userManager.AddToRoleAsync(user, role).ConfigureAwait(false),
            false => await userManager.RemoveFromRoleAsync(user, role).ConfigureAwait(false),
        };
    }
}
