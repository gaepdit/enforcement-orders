using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Entities.Users;
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

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var principal = _httpContextAccessor?.HttpContext?.User;
            return principal == null ? null : await _userManager.GetUserAsync(principal);
        }

        public async Task<IList<string>> GetCurrentUserRolesAsync() =>
            await _userManager.GetRolesAsync(await GetCurrentUserAsync());

        public Task<List<ApplicationUser>> GetUsersAsync(string nameFilter, string emailFilter) =>
            _context.Users.AsNoTracking()
                .Where(m => string.IsNullOrEmpty(nameFilter)
                    || m.GivenName.Contains(nameFilter)
                    || m.FamilyName.Contains(nameFilter))
                .Where(m => string.IsNullOrEmpty(emailFilter)
                    || m.Email == emailFilter)
                .OrderBy(m => m.FamilyName)
                .ToListAsync();

        public Task<ApplicationUser> GetUserByIdAsync(Guid id) =>
            _userManager.FindByIdAsync(id.ToString());

        public async Task<IList<string>> GetUserRolesAsync(Guid id) =>
            await _userManager.GetRolesAsync(await GetUserByIdAsync(id));

        public async Task<IdentityResult> UpdateUserRolesAsync(Guid id, Dictionary<string, bool> roleSettings)
        {
            foreach (var (key, value) in roleSettings)
            {
                var result = await UpdateUserRoleAsync(id, key, value);

                if (result != IdentityResult.Success)
                {
                    return result;
                }
            }

            return IdentityResult.Success;
        }

        private async Task<IdentityResult> UpdateUserRoleAsync(Guid id, string role, bool addToRole)
        {
            var user = await GetUserByIdAsync(id);
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
