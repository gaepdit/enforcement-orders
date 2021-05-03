using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enfo.Domain.Resources.Users;
using Enfo.Domain.Services;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Users
{
    [Authorize]
    public class Details : PageModel
    {
        public UserView DisplayUser { get; private set; }
        public IList<string> Roles { get; private set; }
        public DisplayMessage Message { get; private set; }

        private readonly IUserService _userService;
        public Details(IUserService userService) => _userService = userService;

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) return NotFound();
            DisplayUser = await _userService.GetUserByIdAsync(id.Value);
            if (DisplayUser == null) return NotFound();
            Roles = await _userService.GetUserRolesAsync(DisplayUser.Id);
            Message = TempData?.GetDisplayMessage();
            return Page();
        }
    }
}