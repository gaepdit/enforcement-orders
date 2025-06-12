using Enfo.AppServices.Staff;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Users;

[Authorize]
public class Details : PageModel
{
    public StaffView DisplayStaff { get; private set; }
    public IList<string> Roles { get; private set; }
    public DisplayMessage Message { get; private set; }

    public async Task<IActionResult> OnGetAsync([FromServices] IStaffService staffService, Guid? id)
    {
        if (id == null) return NotFound();
        DisplayStaff = await staffService.FindUserAsync(id.Value);
        if (DisplayStaff == null) return NotFound();
        Roles = await staffService.GetUserRolesAsync(DisplayStaff.Id);
        Message = TempData?.GetDisplayMessage();
        return Page();
    }
}
