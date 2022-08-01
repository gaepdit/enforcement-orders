using Enfo.Domain.Users.Entities;
using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using EnfoTests.TestData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Enfo.LocalRepository;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) =>
        (_userManager, _httpContextAccessor) = (userManager, httpContextAccessor);

    private async Task<ApplicationUser> GetCurrentApplicationUserAsync()
    {
        var principal = _httpContextAccessor?.HttpContext?.User;
        return principal == null ? null : await _userManager.GetUserAsync(principal);
    }

    public async Task<UserView> GetCurrentUserAsync()
    {
        var user = await GetCurrentApplicationUserAsync();
        return user == null ? null : new UserView(user);
    }

    public async Task<IList<string>> GetCurrentUserRolesAsync() =>
        await _userManager.GetRolesAsync(await GetCurrentApplicationUserAsync());

    private static Task<List<UserView>> GetUsersAsync(string nameFilter, string emailFilter) =>
        Task.FromResult(
            UsersData.Users
                .Where(m => string.IsNullOrEmpty(nameFilter)
                    || m.GivenName.Contains(nameFilter)
                    || m.FamilyName.Contains(nameFilter))
                .Where(m => string.IsNullOrEmpty(emailFilter) || m.Email == emailFilter)
                .OrderBy(m => m.FamilyName).ThenBy(m => m.GivenName)
                .Select(e => new UserView(e))
                .ToList());

    public async Task<List<UserView>> GetUsersAsync(string nameFilter, string emailFilter, string role)
    {
        if (string.IsNullOrEmpty(role)) return await GetUsersAsync(nameFilter, emailFilter);

        return (await _userManager.GetUsersInRoleAsync(role))
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
        var user = await _userManager.FindByIdAsync(id.ToString());
        return user == null ? null : await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> UpdateUserRolesAsync(Guid id, Dictionary<string, bool> roleUpdates)
    {
        foreach (var (key, value) in roleUpdates)
        {
            var result = await UpdateUserRoleAsync(id, key, value);
            if (result != IdentityResult.Success) return result;
        }

        return IdentityResult.Success;
    }

    private async Task<IdentityResult> UpdateUserRoleAsync(Guid id, string role, bool addToRole)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return IdentityResult.Failed(_userManager.ErrorDescriber.DefaultError());

        var isInRole = await _userManager.IsInRoleAsync(user, role);
        if (addToRole == isInRole) return IdentityResult.Success;

        return addToRole switch
        {
            true => await _userManager.AddToRoleAsync(user, role),
            false => await _userManager.RemoveFromRoleAsync(user, role)
        };
    }
}
