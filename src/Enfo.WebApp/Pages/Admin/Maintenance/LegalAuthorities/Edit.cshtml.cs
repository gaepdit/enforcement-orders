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
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities;

[Authorize(Roles = UserRole.SiteMaintenance)]
public class Edit : PageModel
{
    [BindProperty]
    public LegalAuthorityCommand Item { get; set; }

    [TempData, UsedImplicitly]
    public int HighlightId { [UsedImplicitly] get; set; }

    [BindProperty, HiddenInput]
    public string OriginalName { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;

    private readonly ILegalAuthorityRepository _repository;
    public Edit(ILegalAuthorityRepository repository) => _repository = repository;

    [UsedImplicitly]
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        var originalItem = await _repository.GetAsync(id.Value);
        if (originalItem == null) return NotFound("ID not found.");

        if (!originalItem.Active)
        {
            TempData.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
            return RedirectToPage("Index");
        }

        Item = new LegalAuthorityCommand(originalItem);
        OriginalName = originalItem.AuthorityName;
        return Page();
    }

    [UsedImplicitly]
    public async Task<IActionResult> OnPostAsync([FromServices] IValidator<LegalAuthorityCommand> validator)
    {
        if (Item.Id is null) return BadRequest();

        var originalItem = await _repository.GetAsync(Item.Id.Value);
        if (originalItem == null) return NotFound();

        if (!originalItem.Active)
        {
            TempData.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
            return RedirectToPage("Index");
        }

        var validationResult = await validator.ValidateAsync(Item);
        if (!validationResult.IsValid) validationResult.AddToModelState(ModelState, nameof(Item));
        if (!ModelState.IsValid) return Page();

        try
        {
            await _repository.UpdateAsync(Item);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repository.ExistsAsync(Item.Id.Value)) return NotFound();
            throw;
        }

        HighlightId = Item.Id.Value;
        TempData.SetDisplayMessage(Context.Success, $"{Item.AuthorityName} successfully updated.");
        return RedirectToPage("Index");
    }
}
