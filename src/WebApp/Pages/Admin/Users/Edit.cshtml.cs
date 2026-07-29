using Enfo.AppServices.Staff;
using Enfo.Domain.Users;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;

namespace Enfo.WebApp.Pages.Admin.Users;

[Authorize(Roles = AppRole.UserMaintenance)]
public class Edit(IStaffService staffService) : PageModel
{
    [FromRoute]
    public Guid? Id { get; set; }

    [BindProperty]
    public List<UserRoleSetting> UserRoleSettings { get; set; }

    public StaffView DisplayStaff { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id is null) return RedirectToPage("Index");

        DisplayStaff = await staffService.FindUserAsync(Id.Value);
        if (DisplayStaff == null) return NotFound();

        await PopulateRoleSettingsAsync(Id.Value);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Id is null) return BadRequest();

        if (!ModelState.IsValid)
        {
            DisplayStaff = await staffService.FindUserAsync(Id.Value);
            if (DisplayStaff == null) return NotFound();
            await PopulateRoleSettingsAsync(Id.Value);
            return Page();
        }

        var roleUpdates = UserRoleSettings.ToDictionary(r => r.Name, r => r.IsSelected);
        var result = await staffService.UpdateUserRolesAsync(Id.Value, roleUpdates);

        if (result.Succeeded)
        {
            TempData.SetDisplayMessage(Context.Success, "User roles successfully updated.");
            return RedirectToPage("Details", new { Id });
        }

        foreach (var err in result.Errors)
            ModelState.AddModelError(string.Empty, string.Concat(err.Code, ": ", err.Description));

        DisplayStaff = await staffService.FindUserAsync(Id.Value);
        if (DisplayStaff == null) return NotFound();
        await PopulateRoleSettingsAsync(Id.Value);
        return Page();
    }

    private async Task PopulateRoleSettingsAsync(Guid id)
    {
        var roles = await staffService.GetUserRolesAsync(id);

        UserRoleSettings = AppRole.AllRoles.Select(r => new UserRoleSetting
        {
            Name = r.Key,
            Description = r.Value.Description,
            DisplayName = r.Value.DisplayName,
            IsSelected = roles.Contains(r.Key),
        }).ToList();
    }

    public class UserRoleSetting
    {
        public string Name { get; init; }
        public string DisplayName { get; init; }
        public string Description { get; init; }
        public bool IsSelected { get; init; }
    }
}
