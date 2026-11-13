using Enfo.Domain.Identity;
using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.PageModelHelpers;
using FluentValidation;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class EditModel(ILegalAuthorityRepository repository) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public LegalAuthorityUpdateDto Item { get; set; } = null!;

    [TempData]
    public int HighlightId { get; set; }

    [BindProperty]
    public string OriginalName { get; set; } = null!;

    public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;

    public async Task<IActionResult> OnGetAsync(CancellationToken token = default)
    {
        if (Id == 0) return NotFound();
        var originalItem = await repository.GetAsync(Id, token);
        if (originalItem == null) return NotFound("ID not found.");

        Item = new LegalAuthorityUpdateDto { AuthorityName = originalItem.AuthorityName, Active = originalItem.Active };
        OriginalName = originalItem.AuthorityName;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync([FromServices] IValidator<LegalAuthorityUpdateDto> validator,
        CancellationToken token = default)
    {
        if (Id == 0) return BadRequest();

        await validator.ApplyValidationAsync(Item, ModelState, Id);
        if (!ModelState.IsValid) return Page();

        await repository.UpdateAsync(Id, Item, token);

        HighlightId = Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"{Item.AuthorityName} successfully updated.");
        return RedirectToPage("Index");
    }
}
