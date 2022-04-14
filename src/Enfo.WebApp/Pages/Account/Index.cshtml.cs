using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Account
{
    [Authorize]
    public class Index : PageModel
    {
        public UserView DisplayUser { get; private set; }
        public IList<string> Roles { get; private set; }

        [UsedImplicitly]
        public async Task OnGetAsync([FromServices] IUserService userService)
        {
            DisplayUser = await userService.GetCurrentUserAsync() ?? throw new Exception("Current user not found");
            Roles = await userService.GetCurrentUserRolesAsync();
        }
    }
}