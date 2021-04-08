using System.Threading.Tasks;
using Enfo.Domain.Data;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Resources.EpdContact;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Enfo.Repository.Validation.EnforcementOrderValidation;

namespace Enfo.WebApp.Pages.Admin
{
    public class Add : PageModel
    {
        [BindProperty]
        public EnforcementOrderCreate Item { get; set; }

        // Select Lists
        public static SelectList CountiesSelectList => new(DomainData.Counties);
        public SelectList EpdContactsSelectList { get; private set; }
        public SelectList LegalAuthoritiesSelectList { get; private set; }

        private readonly IEnforcementOrderRepository _order;
        private readonly ILegalAuthorityRepository _legalAuthority;
        private readonly IEpdContactRepository _contact;

        public Add(IEnforcementOrderRepository order,
            ILegalAuthorityRepository legalAuthority,
            IEpdContactRepository contact) =>
            (_order, _legalAuthority, _contact) = (order, legalAuthority, contact);

        [UsedImplicitly]
        public async Task OnGetAsync()
        {
            await PopulateSelectListsAsync();
            Item = new EnforcementOrderCreate();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            Item.TrimAll();
            var validationResult = ValidateNewEnforcementOrder(Item);

            if (await _order.OrderNumberExistsAsync(Item.OrderNumber).ConfigureAwait(false))
            {
                validationResult.AddErrorMessage(nameof(EnforcementOrderCreate.OrderNumber),
                    $"An Order with the same number ({Item.OrderNumber}) already exists.");
            }

            if (!validationResult.IsValid)
            {
                foreach (var (key, value) in validationResult.ErrorMessages)
                {
                    ModelState.AddModelError(string.Concat(nameof(Item), ".", key), value);
                }
            }

            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return Page();
            }

            var id = await _order.CreateAsync(Item);
            TempData?.SetDisplayMessage(Context.Success, "The new Enforcement Order has been successfully added.");
            return RedirectToPage("Details", new {id});
        }

        private async Task PopulateSelectListsAsync()
        {
            LegalAuthoritiesSelectList = new SelectList(await _legalAuthority.ListAsync(),
                nameof(LegalAuthorityView.Id), nameof(LegalAuthorityView.AuthorityName));
            EpdContactsSelectList = new SelectList(await _contact.ListAsync(),
                nameof(EpdContactView.Id), nameof(EpdContactView.ContactName));
        }
    }
}