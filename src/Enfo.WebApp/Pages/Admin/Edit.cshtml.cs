using Enfo.Domain.Entities.Users;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.Domain.Resources.EpdContact;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Enfo.WebApp.Pages.Admin
{
    [Authorize(Roles = UserRole.OrderAdministrator)]
    public class Edit : PageModel
    {
        [BindProperty]
        public EnforcementOrderUpdate Item { get; set; }

        [BindProperty, HiddenInput]
        public int Id { get; set; }

        [BindProperty, HiddenInput]
        public string OriginalOrderNumber { get; set; }

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
                return RedirectToPage("Details", new { id });
            }

            Item = new EnforcementOrderUpdate(originalItem);
            Id = id.Value;
            OriginalOrderNumber = originalItem.OrderNumber;
            await PopulateSelectListsAsync();
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return Page();
            }

            var result = await Item.TryUpdateAsync(_order, Id);

            if (result.Success)
            {
                TempData?.SetDisplayMessage(Context.Success, "The Enforcement Order has been successfully updated.");
                return RedirectToPage("Details", new { Id });
            }

            if (result.OriginalItem is null)
            {
                return NotFound();
            }

            if (result.OriginalItem.Deleted)
            {
                TempData?.SetDisplayMessage(Context.Warning, "This Enforcement Order is deleted and cannot be edited.");
                return RedirectToPage("Details", new { Id });
            }

            foreach (var (key, value) in result.ValidationErrors)
            {
                ModelState.AddModelError(string.Concat(nameof(Item), ".", key), value);
            }

            await PopulateSelectListsAsync();
            return Page();
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