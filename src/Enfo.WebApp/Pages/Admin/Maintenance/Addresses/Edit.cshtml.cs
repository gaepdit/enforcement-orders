using System.Threading.Tasks;
using Enfo.Domain.Mapping;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.Address;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Addresses
{
    public class Edit : PageModel
    {
        [BindProperty]
        public AddressUpdate Item { get; set; }

        [BindProperty]
        [HiddenInput]
        public int Id { get; set; }

        [TempData]
        public int HighlightId { get; set; }

        public static MaintenanceOption ThisOption => MaintenanceOption.Address;

        private readonly IAddressRepository _repository;
        public Edit(IAddressRepository repository) => _repository = repository;

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

            Item = AddressMapping.ToAddressUpdate(originalItem);
            Id = id.Value;
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

            if (!ModelState.IsValid) return Page();

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
    }
}