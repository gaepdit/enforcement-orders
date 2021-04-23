using System.Threading.Tasks;
using Enfo.Domain.Mapping;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.Address;
using Enfo.Domain.Resources.EpdContact;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Contacts
{
    public class Edit : PageModel
    {
        [BindProperty]
        public EpdContactUpdate Item { get; set; }

        [BindProperty]
        [HiddenInput]
        public int Id { get; set; }

        [TempData]
        public int HighlightId { get; set; }

        public SelectList AddressSelectList { get; private set; }

        public static MaintenanceOption ThisOption => MaintenanceOption.EpdContact;

        private readonly IEpdContactRepository _repository;
        private readonly IAddressRepository _address;

        public Edit(IEpdContactRepository repository, IAddressRepository address) =>
            (_repository, _address) = (repository, address);

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var originalItem = await _repository.GetAsync(id.Value);
            if (originalItem == null) return NotFound("ID not found.");

            if (!originalItem.Active)
            {
                TempData?.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
                return RedirectToPage("Index");
            }

            Item = EpdContactMapping.ToEpdContactUpdate(originalItem);
            Id = id.Value;
            await PopulateSelectListsAsync();
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            var originalItem = await _repository.GetAsync(Id);
            if (originalItem == null) return NotFound();

            if (!originalItem.Active)
            {
                TempData?.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
                return RedirectToPage("Index");
            }

            Item.TrimAll();

            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return Page();
            }

            try
            {
                await _repository.UpdateAsync(Id, Item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(Id)) return NotFound();
                throw;
            }

            HighlightId = Id;
            TempData?.SetDisplayMessage(Context.Success, $"{ThisOption.SingularName} successfully updated.");
            return RedirectToPage("Index");
        }

        private async Task PopulateSelectListsAsync() =>
            AddressSelectList = new SelectList(await _address.ListAsync(), nameof(AddressView.Id),
                nameof(AddressView.AsLinearString));
    }
}