using Enfo.Domain.Identity;
using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.PageModelHelpers;
using FluentValidation;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class AddModel : PageModel
{
    [BindProperty]
    public LegalAuthorityCreateDto Item { get; init; } = null!;

    public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;

    [TempData]
    public int HighlightId { get; set; }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] ILegalAuthorityRepository repository,
        [FromServices] IValidator<LegalAuthorityCreateDto> validator,
        CancellationToken token = default)
    {
        await validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        HighlightId = await repository.CreateAsync(Item, token).ConfigureAwait(false);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"{Item.AuthorityName} successfully added.");
        return RedirectToPage("Index");
    }
}
