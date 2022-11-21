using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Users.Entities;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[Authorize(Roles = UserRole.SiteMaintenance)]
public class Add : PageModel
{
    [BindProperty]
    public LegalAuthorityCommand Item { get; init; }

    public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;

    [TempData]
    public int HighlightId { get; [UsedImplicitly] set; }

    [UsedImplicitly]
    public static void OnGet()
    {
        // Method intentionally left empty.
    }

    [UsedImplicitly]
    public async Task<IActionResult> OnPostAsync(
        [FromServices] ILegalAuthorityRepository repository,
        [FromServices] IValidator<LegalAuthorityCommand> validator)
    {
        var validationResult = await validator.ValidateAsync(Item);
        if (!validationResult.IsValid) validationResult.AddToModelState(ModelState, nameof(Item));
        if (!ModelState.IsValid) return Page();

        var id = await repository.CreateAsync(Item);

        HighlightId = id;
        TempData.SetDisplayMessage(Context.Success, $"{Item.AuthorityName} successfully added.");
        return RedirectToPage("Index");
    }
}
