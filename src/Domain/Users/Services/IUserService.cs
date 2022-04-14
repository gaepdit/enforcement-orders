using Enfo.Domain.Users.Resources;
using Microsoft.AspNetCore.Identity;

namespace Enfo.Domain.Users.Services;

/// <summary>
/// Provide methods for interacting with application user accounts
/// </summary>
public interface IUserService
{
    public Task<UserView> GetCurrentUserAsync();
    public Task<IList<string>> GetCurrentUserRolesAsync();
    public Task<List<UserView>> GetUsersAsync(string nameFilter, string emailFilter, string role);
    public Task<UserView> GetUserByIdAsync(Guid id);
    public Task<IList<string>> GetUserRolesAsync(Guid id);
    public Task<IdentityResult> UpdateUserRolesAsync(Guid id, Dictionary<string, bool> roleUpdates);
}
