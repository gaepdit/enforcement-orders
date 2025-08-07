using Enfo.AppServices.Staff;
using Enfo.Domain.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Enfo.WebApp.Pages.Admin.Users;

[Authorize]
public class Index : PageModel
{
    public string Name { get; init; }

    [EmailAddress]
    public string Email { get; init; }

    public string Role { get; init; }

    public IEnumerable<SelectListItem> RoleItems { get; } =
        AppRole.AllRoles.Select(d => new SelectListItem(d.Value.DisplayName, d.Key));

    public bool ShowResults { get; private set; }
    public List<StaffView> SearchResults { get; private set; }

    public static void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnGetSearchAsync([FromServices] IStaffService staffService,
        string name, string email, string role)
    {
        if (!ModelState.IsValid) return Page();
        SearchResults = await staffService.SearchUsersAsync(name, email, role);
        ShowResults = true;
        return Page();
    }
}
