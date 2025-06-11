using Enfo.Domain.Users.Entities;
using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using EnfoTests.TestData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Enfo.LocalRepository;

public class InMemoryUserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    : IUserService
{
    private async Task<ApplicationUser> GetCurrentApplicationUserAsync()
    {
        var principal = httpContextAccessor?.HttpContext?.User;
        return principal == null ? null : await userManager.GetUserAsync(principal).ConfigureAwait(false);
    }

    public async Task<UserView> GetCurrentUserAsync()
    {
        var user = await GetCurrentApplicationUserAsync().ConfigureAwait(false);
        return user == null ? null : new UserView(user);
    }

    public async Task<IList<string>> GetCurrentUserRolesAsync() =>
        await userManager.GetRolesAsync(await GetCurrentApplicationUserAsync().ConfigureAwait(false))
            .ConfigureAwait(false);

    private static List<UserView> GetUsers(string nameFilter, string emailFilter) =>
        UsersData.Users
            .Where(m => string.IsNullOrEmpty(nameFilter)
                        || m.GivenName.Contains(nameFilter)
                        || m.FamilyName.Contains(nameFilter))
            .Where(m => string.IsNullOrEmpty(emailFilter) || m.Email == emailFilter)
            .OrderBy(m => m.FamilyName).ThenBy(m => m.GivenName)
            .Select(e => new UserView(e))
            .ToList();

    public async Task<List<UserView>> GetUsersAsync(string nameFilter, string emailFilter, string role)
    {
        if (string.IsNullOrEmpty(role)) return GetUsers(nameFilter, emailFilter);

        return (await userManager.GetUsersInRoleAsync(role).ConfigureAwait(false))
            .Where(m => string.IsNullOrEmpty(nameFilter)
                        || m.GivenName.Contains(nameFilter)
                        || m.FamilyName.Contains(nameFilter))
            .Where(m => string.IsNullOrEmpty(emailFilter) || m.Email == emailFilter)
            .OrderBy(m => m.FamilyName).ThenBy(m => m.GivenName)
            .Select(e => new UserView(e))
            .ToList();
    }

    public Task<UserView> GetUserByIdAsync(Guid id)
    {
        var user = UsersData.Users.SingleOrDefault(e => e.Id == id);
        return Task.FromResult(user == null ? null : new UserView(user));
    }

    public async Task<IList<string>> GetUserRolesAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        return user == null ? null : await userManager.GetRolesAsync(user).ConfigureAwait(false);
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
