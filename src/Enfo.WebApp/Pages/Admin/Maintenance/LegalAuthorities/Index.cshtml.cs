using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[Authorize]
public class Index : PageModel
{
    public IReadOnlyList<LegalAuthorityView> Items { get; private set; }
    public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;
    public DisplayMessage Message { get; private set; }

    [TempData]
    public int HighlightId { get; set; }

    private readonly ILegalAuthorityRepository _repository;
    public Index(ILegalAuthorityRepository repository) => _repository = repository;

    public async Task OnGetAsync()
    {
        Items = await _repository.ListAsync(true);
        Message = TempData?.GetDisplayMessage();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return BadRequest();
        if (!ModelState.IsValid) return Page();
        var item = await _repository.GetAsync(id.Value);
        if (item == null) return NotFound();

        try
        {
            await _repository.UpdateStatusAsync(id.Value, !item.Active);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repository.ExistsAsync(id.Value)) return NotFound();
            throw;
        }

        TempData?.SetDisplayMessage(Context.Success,
            $"{item.AuthorityName} successfully {(item.Active ? "deactivated" : "restored")}.");

        return RedirectToPage("Index");
    }
}
