using Enfo.Domain.Users.Entities;
using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enfo.WebApp.Pages.Admin.Users
{
    [Authorize(Roles = UserRole.UserMaintenance)]
    public class Edit : PageModel
    {
        [BindProperty, HiddenInput]
        public Guid UserId { get; set; }

        [BindProperty]
        public List<UserRoleSetting> UserRoleSettings { get; [UsedImplicitly] set; }

        public UserView DisplayUser { get; private set; }

        private readonly IUserService _userService;
        public Edit(IUserService userService) => _userService = userService;

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) return NotFound();
            DisplayUser = await _userService.GetUserByIdAsync(id.Value);
            if (DisplayUser == null) return NotFound();
            UserId = id.Value;

            await PopulateRoleSettingsAsync();
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                DisplayUser = await _userService.GetUserByIdAsync(UserId);
                if (DisplayUser == null) return NotFound();
                await PopulateRoleSettingsAsync();
                return Page();
            }

            var roleUpdates = UserRoleSettings.ToDictionary(r => r.Name, r => r.IsSelected);
            var result = await _userService.UpdateUserRolesAsync(UserId, roleUpdates);

            if (result.Succeeded)
            {
                TempData.SetDisplayMessage(Context.Success, "User roles successfully updated.");
                return RedirectToPage("Details", new { id = UserId });
            }

            foreach (var err in result.Errors)
                ModelState.AddModelError(string.Empty, string.Concat(err.Code, ": ", err.Description));

            DisplayUser = await _userService.GetUserByIdAsync(UserId);
            if (DisplayUser == null) return NotFound();
            await PopulateRoleSettingsAsync();
            return Page();
        }

        private async Task PopulateRoleSettingsAsync()
        {
            var roles = await _userService.GetUserRolesAsync(UserId);

            UserRoleSettings = UserRole.AllRoles.Select(r => new UserRoleSetting
            {
                Name = r.Key,
                Description = r.Value.Description,
                DisplayName = r.Value.DisplayName,
                IsSelected = roles.Contains(r.Key)
            }).ToList();
        }

        public class UserRoleSetting
        {
            public string Name { get; init; }
            public string DisplayName { get; init; }
            public string Description { get; init; }
            public bool IsSelected { get; init; }
        }
    }
}
