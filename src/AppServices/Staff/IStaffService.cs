using Microsoft.AspNetCore.Identity;

namespace Enfo.AppServices.Staff;

/// <summary>
/// Provide methods for interacting with application user accounts
/// </summary>
public interface IStaffService
{
    public Task<StaffView?> GetCurrentUserAsync();
    public Task<IList<string>> GetCurrentUserRolesAsync();
    public Task<List<StaffView>> SearchUsersAsync(string nameFilter, string emailFilter, string role);
    public Task<StaffView?> FindUserAsync(Guid id);
    public Task<IList<string>> GetUserRolesAsync(Guid id);
    public Task<IdentityResult> UpdateUserRolesAsync(Guid id, Dictionary<string, bool> roleUpdates);
}
