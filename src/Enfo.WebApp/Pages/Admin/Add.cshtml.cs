using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Users.Entities;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Enfo.WebApp.Pages.Admin
{
    [Authorize(Roles = UserRole.OrderAdministrator)]
    public class Add : PageModel
    {
        [BindProperty]
        public EnforcementOrderCreate Item { get; set; }

        // Select Lists
        public SelectList EpdContactsSelectList { get; private set; }
        public SelectList LegalAuthoritiesSelectList { get; private set; }

        private readonly IEnforcementOrderRepository _order;
        private readonly ILegalAuthorityRepository _legalAuthority;
        private readonly IEpdContactRepository _contact;

        public Add(IEnforcementOrderRepository order,
            ILegalAuthorityRepository legalAuthority,
            IEpdContactRepository contact)
        {
            _order = order;
            _legalAuthority = legalAuthority;
            _contact = contact;
        }

        [UsedImplicitly]
        public async Task OnGetAsync()
        {
            await PopulateSelectListsAsync();
            Item = new EnforcementOrderCreate();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return Page();
            }

            var id = await _order.CreateAsync(Item);
            TempData.SetDisplayMessage(Context.Success, "The new Enforcement Order has been successfully added.");
            return RedirectToPage("Details", new { id });
        }

        private async Task PopulateSelectListsAsync()
        {
            LegalAuthoritiesSelectList = new SelectList(await _legalAuthority.ListAsync(),
                nameof(LegalAuthorityView.Id), nameof(LegalAuthorityView.AuthorityName));
            EpdContactsSelectList = new SelectList(await _contact.ListAsync(),
                nameof(EpdContactView.Id), nameof(EpdContactView.AsLinearString));
        }
    }
}
