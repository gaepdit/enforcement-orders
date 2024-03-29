﻿using Enfo.Domain.Users.Entities;
using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using Enfo.Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly EnfoDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        UserManager<ApplicationUser> userManager,
        EnfoDbContext context,
        IHttpContextAccessor httpContextAccessor) =>
        (_userManager, _context, _httpContextAccessor) =
        (userManager, context, httpContextAccessor);

    private async Task<ApplicationUser> GetCurrentApplicationUserAsync()
    {
        var principal = _httpContextAccessor?.HttpContext?.User;
        return principal == null ? null : await _userManager.GetUserAsync(principal).ConfigureAwait(false);
    }

    public async Task<UserView> GetCurrentUserAsync()
    {
        var user = await GetCurrentApplicationUserAsync().ConfigureAwait(false);
        return user == null ? null : new UserView(user);
    }

    public async Task<IList<string>> GetCurrentUserRolesAsync() =>
        await _userManager.GetRolesAsync(await GetCurrentApplicationUserAsync().ConfigureAwait(false)).ConfigureAwait(false);

    private Task<List<UserView>> GetUsersAsync(string nameFilter, string emailFilter) =>
        _context.Users.AsNoTracking()
            .Where(m => string.IsNullOrEmpty(nameFilter)
                || m.GivenName.Contains(nameFilter)
                || m.FamilyName.Contains(nameFilter))
            .Where(m => string.IsNullOrEmpty(emailFilter) || m.Email == emailFilter)
            .OrderBy(m => m.FamilyName).ThenBy(m => m.GivenName)
            .Select(e => new UserView(e))
            .ToListAsync();

    public async Task<List<UserView>> GetUsersAsync(string nameFilter, string emailFilter, string role)
    {
        if (string.IsNullOrEmpty(role)) return await GetUsersAsync(nameFilter, emailFilter).ConfigureAwait(false);

        return (await _userManager.GetUsersInRoleAsync(role).ConfigureAwait(false))
            .Where(m => string.IsNullOrEmpty(nameFilter)
                || m.GivenName.Contains(nameFilter)
                || m.FamilyName.Contains(nameFilter))
            .Where(m => string.IsNullOrEmpty(emailFilter) || m.Email == emailFilter)
            .OrderBy(m => m.FamilyName).ThenBy(m => m.GivenName)
            .Select(e => new UserView(e))
            .ToList();
    }

    public async Task<UserView> GetUserByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        return user == null ? null : new UserView(user);
    }

    public async Task<IList<string>> GetUserRolesAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        return user == null ? null : await _userManager.GetRolesAsync(user).ConfigureAwait(false);
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
        var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        if (user == null) return IdentityResult.Failed(_userManager.ErrorDescriber.DefaultError());

        var isInRole = await _userManager.IsInRoleAsync(user, role).ConfigureAwait(false);
        if (addToRole == isInRole) return IdentityResult.Success;

        return addToRole switch
        {
            true => await _userManager.AddToRoleAsync(user, role).ConfigureAwait(false),
            false => await _userManager.RemoveFromRoleAsync(user, role).ConfigureAwait(false),
        };
    }
}
