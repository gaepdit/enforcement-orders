using Enfo.Domain.Users.Resources;
using Enfo.Domain.Users.Services;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Users;

[Authorize]
public class Details : PageModel
{
    public UserView DisplayUser { get; private set; }
    public IList<string> Roles { get; private set; }
    public DisplayMessage Message { get; private set; }

    [UsedImplicitly]
    public async Task<IActionResult> OnGetAsync([FromServices] IUserService userService, Guid? id)
    {
        if (id == null) return NotFound();
        DisplayUser = await userService.GetUserByIdAsync(id.Value);
        if (DisplayUser == null) return NotFound();
        Roles = await userService.GetUserRolesAsync(DisplayUser.Id);
        Message = TempData?.GetDisplayMessage();
        return Page();
    }
}
