using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Users;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin;

[Authorize(Roles = AppRole.OrderAdministrator)]
public class Delete(IEnforcementOrderRepository repository) : PageModel
{
    [BindProperty]
    [HiddenInput]
    public int Id { get; set; }

    public EnforcementOrderAdminView Item { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        Item = await repository.GetAdminViewAsync(id.Value);
        if (Item == null) return NotFound("ID not found.");
        if (Item.Deleted) return RedirectToPage("Details", new { Id });
        Id = id.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await repository.DeleteAsync(Id);
        TempData?.SetDisplayMessage(Context.Success, "The Order has been successfully deleted.");
        return RedirectToPage("Details", new { Id });
    }
}
