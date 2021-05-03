using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Resources.Users;
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

        public UserView DisplayUser { get; private set; }
        public IList<string> Roles { get; private set; }

        [UsedImplicitly]
        public async Task OnGet()
        {
            DisplayUser = await _userService.GetCurrentUserAsync()
                ?? throw new Exception("Current user not found");
            Roles = await _userService.GetCurrentUserRolesAsync();
        }
    }
}