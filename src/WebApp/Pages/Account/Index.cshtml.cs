using Enfo.AppServices.Staff;
using Enfo.Domain.Identity;
using System.Security.Claims;

namespace Enfo.WebApp.Pages.Account;

[Authorize]
public class AccountIndexModel : PageModel
{
    public StaffViewDto DisplayStaff { get; private set; } = null!;
    public IReadOnlyList<AppRole> Roles { get; private set; } = null!;
    public IEnumerable<Claim> Claims => User.Claims;

    public async Task OnGetAsync([FromServices] IStaffService staffService)
    {
        DisplayStaff = await staffService.GetCurrentUserAsync();
        Roles = await staffService.GetAppRolesAsync(DisplayStaff.Id);
    }
}
