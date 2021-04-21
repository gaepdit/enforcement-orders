using System.Threading.Tasks;
using Enfo.Repository.Mapping;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Resources.EpdContact;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.Repository.Validation;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Pages.Admin
{
    // [Authorize(Roles = UserRole.OrderAdministrator)]
    public class Edit : PageModel
    {
        [BindProperty]
        public EnforcementOrderUpdate Item { get; set; }

        [BindProperty]
        [HiddenInput]
        public int Id { get; set; }
        
        public string OriginalOrderNumber { get; private set; }

        // Select Lists
        public SelectList EpdContactsSelectList { get; private set; }
        public SelectList LegalAuthoritiesSelectList { get; private set; }

        private readonly IEnforcementOrderRepository _order;
        private readonly ILegalAuthorityRepository _legalAuthority;
        private readonly IEpdContactRepository _contact;

        public Edit(IEnforcementOrderRepository order,
            ILegalAuthorityRepository legalAuthority,
            IEpdContactRepository contact) =>
            (_order, _legalAuthority, _contact) = (order, legalAuthority, contact);

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var originalItem = await _order.GetAdminViewAsync(id.Value);
            if (originalItem == null) return NotFound("ID not found.");

            if (originalItem.Deleted)
            {
                TempData?.SetDisplayMessage(Context.Warning, "This Enforcement Order is deleted and cannot be edited.");
                return RedirectToPage("Details", new {id});
            }

            Item = EnforcementOrderMapping.ToEnforcementOrderUpdate(originalItem);
            Id = id.Value;
            OriginalOrderNumber = originalItem.OrderNumber;
            await PopulateSelectListsAsync();
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            var originalItem = await _order.GetAdminViewAsync(Id);
            if (originalItem == null) return NotFound();

            if (originalItem.Deleted)
            {
                TempData?.SetDisplayMessage(Context.Warning, "This Enforcement Order is deleted and cannot be edited.");
                return RedirectToPage("Details", new {Id});
            }

            Item.TrimAll();
            var validationResult = EnforcementOrderValidation.ValidateEnforcementOrderUpdate(originalItem, Item);

            if (await _order.OrderNumberExistsAsync(Item.OrderNumber, Id).ConfigureAwait(false))
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
                OriginalOrderNumber = originalItem.OrderNumber;
                await PopulateSelectListsAsync();
                return Page();
            }

            try
            {
                await _order.UpdateAsync(Id, Item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _order.ExistsAsync(Id)) return NotFound();
                throw;
            }

            TempData?.SetDisplayMessage(Context.Success, "The Enforcement Order has been successfully updated.");
            return RedirectToPage("Details", new {Id});
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