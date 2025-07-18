using Enfo.AppServices.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Enfo.WebApp.Pages.Account;

[Authorize]
public class IndexModel(IStaffService staffService) : PageModel
{
    public StaffView DisplayStaff { get; private set; }
    public IList<string> Roles { get; private set; }
    public IEnumerable<Claim> Claims => User.Claims;

    public async Task OnGetAsync()
    {
        DisplayStaff = await staffService.GetCurrentUserAsync();
        Roles = await staffService.GetCurrentUserRolesAsync();
    }
}
