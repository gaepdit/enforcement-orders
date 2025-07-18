using Enfo.AppServices.Staff;
using Enfo.Domain.Users;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Users;

[Authorize(Roles = AppRole.UserMaintenance)]
public class Edit(IStaffService staffService) : PageModel
{
    [BindProperty, HiddenInput]
    public Guid UserId { get; set; }

    [BindProperty]
    public List<UserRoleSetting> UserRoleSettings { get; set; }

    public StaffView DisplayStaff { get; private set; }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();
        DisplayStaff = await staffService.FindUserAsync(id.Value);
        if (DisplayStaff == null) return NotFound();
        UserId = id.Value;

        await PopulateRoleSettingsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            DisplayStaff = await staffService.FindUserAsync(UserId);
            if (DisplayStaff == null) return NotFound();
            await PopulateRoleSettingsAsync();
            return Page();
        }

        var roleUpdates = UserRoleSettings.ToDictionary(r => r.Name, r => r.IsSelected);
        var result = await staffService.UpdateUserRolesAsync(UserId, roleUpdates);

        if (result.Succeeded)
        {
            TempData.SetDisplayMessage(Context.Success, "User roles successfully updated.");
            return RedirectToPage("Details", new { id = UserId });
        }

        foreach (var err in result.Errors)
            ModelState.AddModelError(string.Empty, string.Concat(err.Code, ": ", err.Description));

        DisplayStaff = await staffService.FindUserAsync(UserId);
        if (DisplayStaff == null) return NotFound();
        await PopulateRoleSettingsAsync();
        return Page();
    }

    private async Task PopulateRoleSettingsAsync()
    {
        var roles = await staffService.GetUserRolesAsync(UserId);

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
