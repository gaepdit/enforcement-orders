using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Enfo.Domain.Services
{
    /// <summary>
    /// Provide methods for interacting with application user accounts
    /// </summary>
    public interface IUserService
    {
        public Task<ApplicationUser> GetCurrentUserAsync();
        public Task<IList<string>> GetCurrentUserRolesAsync();
        public Task<List<ApplicationUser>> GetUsersAsync(string nameFilter, string emailFilter);
        public Task<ApplicationUser> GetUserByIdAsync(Guid id);
        public Task<IList<string>> GetUserRolesAsync(Guid id);
        public Task<IdentityResult> UpdateUserRolesAsync(Guid id, Dictionary<string, bool> roleSettings);
    }
}
