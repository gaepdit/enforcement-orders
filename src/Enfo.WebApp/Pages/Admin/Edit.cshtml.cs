using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.Users.Entities;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Enfo.WebApp.Pages.Admin
{
    [Authorize(Roles = UserRole.OrderAdministrator)]
    public class Edit : PageModel
    {
        [BindProperty]
        public EnforcementOrderUpdate Item { get; set; }

        [BindProperty, HiddenInput]
        public string OriginalOrderNumber { get; set; }

        // Select Lists
        public SelectList EpdContactsSelectList { get; private set; }
        public SelectList LegalAuthoritiesSelectList { get; private set; }

        private readonly IEnforcementOrderRepository _orderRepository;
        private readonly ILegalAuthorityRepository _legalAuthorityRepository;
        private readonly IEpdContactRepository _contactRepository;

        public Edit(IEnforcementOrderRepository orderRepository,
            ILegalAuthorityRepository legalAuthorityRepository,
            IEpdContactRepository contactRepository) =>
            (_orderRepository, _legalAuthorityRepository, _contactRepository) =
            (orderRepository, legalAuthorityRepository, contactRepository);

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var originalItem = await _orderRepository.GetAdminViewAsync(id.Value);
            if (originalItem == null) return NotFound("ID not found.");

            if (originalItem.Deleted)
            {
                TempData.SetDisplayMessage(Context.Warning, "This Enforcement Order is deleted and cannot be edited.");
                return RedirectToPage("Details", new { id });
            }

            Item = new EnforcementOrderUpdate(originalItem);
            OriginalOrderNumber = originalItem.OrderNumber;
            await PopulateSelectListsAsync();
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            var originalItem = await _orderRepository.GetAdminViewAsync(Item.Id);
            if (originalItem == null) return NotFound();

            if (originalItem.Deleted)
            {
                TempData.SetDisplayMessage(Context.Warning, "This Enforcement Order is deleted and cannot be edited.");
                return RedirectToPage("Details", new { Item.Id });
            }

            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return Page();
            }

            try
            {
                await _orderRepository.UpdateAsync(Item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _orderRepository.ExistsAsync(Item.Id))
                {
                    return NotFound();
                }

                throw;
            }

            TempData.SetDisplayMessage(Context.Success, "The Enforcement Order has been successfully updated.");
            return RedirectToPage("Details", new { Item.Id });
        }

        private async Task PopulateSelectListsAsync()
        {
            LegalAuthoritiesSelectList = new SelectList(await _legalAuthorityRepository.ListAsync(),
                nameof(LegalAuthorityView.Id), nameof(LegalAuthorityView.AuthorityName));
            EpdContactsSelectList = new SelectList(await _contactRepository.ListAsync(),
                nameof(EpdContactView.Id), nameof(EpdContactView.AsLinearString));
        }
    }
}
