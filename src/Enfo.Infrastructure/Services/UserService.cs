using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Entities.Users;
using Enfo.Domain.Resources.Users;
using Enfo.Domain.Services;
using Enfo.Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Services
{
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
            return principal == null ? null : await _userManager.GetUserAsync(principal);
        }

        public async Task<UserView> GetCurrentUserAsync()
        {
            var user = await GetCurrentApplicationUserAsync();
            return user == null ? null : new UserView(user);
        }

        public async Task<IList<string>> GetCurrentUserRolesAsync() =>
            await _userManager.GetRolesAsync(await GetCurrentApplicationUserAsync());

        public async Task<List<UserView>> GetUsersAsync(
            string nameFilter, string emailFilter, string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return await _context.Users.AsNoTracking()
                    .Where(m => string.IsNullOrEmpty(nameFilter)
                        || m.GivenName.Contains(nameFilter)
                        || m.FamilyName.Contains(nameFilter))
                    .Where(m => string.IsNullOrEmpty(emailFilter) || m.Email == emailFilter)
                    .OrderBy(m => m.FamilyName).ThenBy(m => m.GivenName)
                    .Select(e => new UserView(e))
                    .ToListAsync();
            }

            return (await _userManager.GetUsersInRoleAsync(role))
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
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user == null ? null : new UserView(user);
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
}
