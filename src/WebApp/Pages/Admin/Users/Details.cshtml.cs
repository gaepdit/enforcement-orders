using Enfo.AppServices.Staff;
using Enfo.Domain.Identity;

namespace Enfo.WebApp.Pages.Admin.Users;

[Authorize]
public class DetailsModel : PageModel
{
    public StaffViewDto DisplayStaff { get; private set; } = null!;
    public IReadOnlyList<AppRole> Roles { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync([FromServices] IStaffService staffService, Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var staff = await staffService.FindAsync(id.Value);
        if (staff is null) return NotFound();

        DisplayStaff = staff;
        Roles = await staffService.GetAppRolesAsync(DisplayStaff.Id);

        return Page();
    }
}
