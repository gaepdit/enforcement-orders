using EnfoTests.TestData;
using Microsoft.AspNetCore.Identity;

namespace Enfo.LocalRepository.Identity;

/// <summary>
/// This store is intentionally only partially implemented. RoleStore is read-only.
/// </summary>
public sealed class LocalRoleStore : IRoleStore<IdentityRole<Guid>>
{
    private readonly IEnumerable<IdentityRole<Guid>> _roles = UserData.GetRoles;

    public Task<IdentityResult> CreateAsync(IdentityRole<Guid> role, CancellationToken cancellationToken) =>
        Task.FromResult(new IdentityResult()); // Intentionally left unimplemented.

    public Task<IdentityResult> UpdateAsync(IdentityRole<Guid> role, CancellationToken cancellationToken) =>
        Task.FromResult(new IdentityResult()); // Intentionally left unimplemented.

    public Task<IdentityResult> DeleteAsync(IdentityRole<Guid> role, CancellationToken cancellationToken) =>
        Task.FromResult(new IdentityResult()); // Intentionally left unimplemented.

    public Task<string> GetRoleIdAsync(IdentityRole<Guid> role, CancellationToken cancellationToken) =>
        Task.FromResult(role.Id.ToString());

    public Task<string> GetRoleNameAsync(IdentityRole<Guid> role, CancellationToken cancellationToken) =>
        Task.FromResult(role.Name);

    public Task SetRoleNameAsync(IdentityRole<Guid> role, string roleName, CancellationToken cancellationToken) =>
        Task.CompletedTask; // Intentionally left unimplemented.

    public Task<string> GetNormalizedRoleNameAsync(IdentityRole<Guid> role, CancellationToken cancellationToken) =>
        Task.FromResult(role.NormalizedName);

    public Task SetNormalizedRoleNameAsync(IdentityRole<Guid> role, string normalizedName,
        CancellationToken cancellationToken)
    {
        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }

    public Task<IdentityRole<Guid>> FindByIdAsync(string roleId, CancellationToken cancellationToken) =>
        Task.FromResult(_roles.SingleOrDefault(r => r.Id.ToString() == roleId));

    public Task<IdentityRole<Guid>> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) =>
        Task.FromResult(_roles.SingleOrDefault(r =>
            string.Equals(r.NormalizedName, normalizedRoleName, StringComparison.InvariantCultureIgnoreCase)));

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
