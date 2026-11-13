using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EpdContacts;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.Identity;
using Enfo.Domain.LegalAuthorities;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Pages.Admin;

[Authorize(Roles = AppRole.OrderAdministrator)]
public class Edit(
    IEnforcementOrderRepository orderRepository,
    ILegalAuthorityRepository legalAuthorityRepository,
    IEpdContactRepository contactRepository)
    : PageModel
{
    [BindProperty]
    public EnforcementOrderUpdate Item { get; set; }

    [BindProperty, HiddenInput]
    public string OriginalOrderNumber { get; set; }

    // Select Lists
    public SelectList EpdContactsSelectList { get; private set; }
    public SelectList LegalAuthoritiesSelectList { get; private set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        var originalItem = await orderRepository.GetAdminViewAsync(id.Value);
        if (originalItem == null) return NotFound("ID not found.");

        if (originalItem.Deleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "This Enforcement Order is deleted and cannot be edited.");
            return RedirectToPage("Details", new { id });
        }

        Item = new EnforcementOrderUpdate(originalItem);
        OriginalOrderNumber = originalItem.OrderNumber;
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync([FromServices] IValidator<EnforcementOrderUpdate> validator)
    {
        var originalItem = await orderRepository.GetAdminViewAsync(Item.Id);
        if (originalItem == null) return NotFound();

        if (originalItem.Deleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "This Enforcement Order is deleted and cannot be edited.");
            return RedirectToPage("Details", new { Item.Id });
        }

        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        try
        {
            await orderRepository.UpdateAsync(Item);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await orderRepository.ExistsAsync(Item.Id))
            {
                return NotFound();
            }

            throw;
        }

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            "The Enforcement Order has been successfully updated.");
        return RedirectToPage("Details", new { Item.Id });
    }

    private async Task PopulateSelectListsAsync()
    {
        LegalAuthoritiesSelectList = new SelectList(await legalAuthorityRepository.ListAsync(),
            nameof(LegalAuthorityViewDto.Id), nameof(LegalAuthorityViewDto.AuthorityName));
        EpdContactsSelectList = new SelectList(await contactRepository.ListAsync(),
            nameof(EpdContactViewDto.Id), nameof(EpdContactViewDto.AsLinearString));
    }
}
