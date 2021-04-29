using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Account
{
    [Authorize]
    public class Index : PageModel
    {
        private readonly IUserService _userService;
        public Index(IUserService userService) => _userService = userService;

        public string DisplayName { get; private set; }
        public string Email { get; private set; }
        public IList<string> Roles { get; private set; }

        [UsedImplicitly]
        public async Task OnGet()
        {
            var currentUser = await _userService.GetCurrentUserAsync()
                ?? throw new Exception("Current user not found");

            DisplayName = currentUser.DisplayName;
            Email = currentUser.Email;
            Roles = await _userService.GetCurrentUserRolesAsync();
        }
    }
}