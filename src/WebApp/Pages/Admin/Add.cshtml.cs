using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Users;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Pages.Admin;

[Authorize(Roles = AppRole.OrderAdministrator)]
public class Add(
    IEnforcementOrderRepository order,
    ILegalAuthorityRepository legalAuthority,
    IEpdContactRepository contact)
    : PageModel
{
    [BindProperty]
    public EnforcementOrderCreate Item { get; set; }

    // Select Lists
    public SelectList EpdContactsSelectList { get; private set; }
    public SelectList LegalAuthoritiesSelectList { get; private set; }

    public async Task OnGetAsync()
    {
        await PopulateSelectListsAsync();
        Item = new EnforcementOrderCreate();
    }

    public async Task<IActionResult> OnPostAsync([FromServices] IValidator<EnforcementOrderCreate> validator)
    {
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        var id = await order.CreateAsync(Item);
        TempData.SetDisplayMessage(Context.Success, "The new Enforcement Order has been successfully added.");
        return RedirectToPage("Details", new { id });
    }

    private async Task PopulateSelectListsAsync()
    {
        LegalAuthoritiesSelectList = new SelectList(await legalAuthority.ListAsync(),
            nameof(LegalAuthorityView.Id), nameof(LegalAuthorityView.AuthorityName));
        EpdContactsSelectList = new SelectList(await contact.ListAsync(),
            nameof(EpdContactView.Id), nameof(EpdContactView.AsLinearString));
    }
}
