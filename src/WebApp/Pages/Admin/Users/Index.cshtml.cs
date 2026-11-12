using Enfo.AppServices.Staff;
using Enfo.Domain.Identity;
using Enfo.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Pages.Admin.Users;

[Authorize]
public class UsersIndexModel(IStaffService staffService) : PageModel
{
    public StaffSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public List<StaffViewDto> SearchResults { get; private set; } = null!;

    public SelectList RolesSelectList { get; private set; } = null!;

    public void OnGet() => PopulateSelectLists();

    public async Task<IActionResult> OnGetSearchAsync(StaffSearchDto spec)
    {
        Spec = spec.TrimAll();
        PopulateSelectLists();
        SearchResults = await staffService.SearchAsync(Spec);
        ShowResults = true;
        return Page();
    }

    private void PopulateSelectLists()
    {
        RolesSelectList = AppRole.AllRoles
            .Select(pair => new ListItem<string>(pair.Key, pair.Value.DisplayName))
            .ToSelectList();
    }
}
