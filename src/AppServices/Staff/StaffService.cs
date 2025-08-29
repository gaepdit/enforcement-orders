using Enfo.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Enfo.AppServices.Staff;

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
    public async Task<StaffView?> GetCurrentUserAsync()
    {
        var user = await GetCurrentApplicationUserAsync().ConfigureAwait(false);
        return user == null ? null : new StaffView(user);
    }

    public async Task<IList<string>> GetCurrentUserRolesAsync()
    {
        var user = await GetCurrentApplicationUserAsync().ConfigureAwait(false);
        return user is null ? [] : await userManager.GetRolesAsync(user).ConfigureAwait(false);
    }

    [SuppressMessage("Performance",
        "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
    private static List<StaffView>
        FilterUsers(IQueryable<ApplicationUser> users, string nameFilter, string emailFilter) =>
        users
            .Where(m => string.IsNullOrWhiteSpace(nameFilter)
                        || m.GivenName.ToLower().Contains(nameFilter.ToLower())
                        || m.FamilyName.ToLower().Contains(nameFilter.ToLower()))
            .Where(m => string.IsNullOrWhiteSpace(emailFilter) || m.Email == emailFilter)
            .OrderBy(m => m.FamilyName).ThenBy(m => m.GivenName)
            .Select(e => new StaffView(e))
            .ToList();

    public async Task<List<StaffView>> SearchUsersAsync(string nameFilter, string emailFilter, string role) =>
        FilterUsers(string.IsNullOrEmpty(role)
                ? userManager.Users
                : (await userManager.GetUsersInRoleAsync(role).ConfigureAwait(false)).AsQueryable(),
            nameFilter, emailFilter);

    public async Task<StaffView?> FindUserAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        return user == null ? null : new StaffView(user);
    }

    public async Task<IList<string>> GetUserRolesAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        return user == null ? [] : await userManager.GetRolesAsync(user).ConfigureAwait(false);
    }

    public async Task<IdentityResult> UpdateUserRolesAsync(Guid id, Dictionary<string, bool> roleUpdates)
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
