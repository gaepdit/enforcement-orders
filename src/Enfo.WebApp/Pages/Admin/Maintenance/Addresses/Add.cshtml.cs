using System.Threading.Tasks;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.Address;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Addresses
{
    public class Add : PageModel
    {
        [BindProperty]
        public AddressCreate Item { get; set; }

        public static MaintenanceOption ThisOption => MaintenanceOption.Address;

        [TempData]
        public int HighlightId { get; set; }

        private readonly IAddressRepository _repository;
        public Add(IAddressRepository repository) => _repository = repository;

        [UsedImplicitly]
        public static void OnGet()
        {
            // Method intentionally left empty.
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            Item.TrimAll();

            if (!ModelState.IsValid) return Page();

            HighlightId = await _repository.CreateAsync(Item);
            TempData?.SetDisplayMessage(Context.Success, $"{ThisOption.SingularName} successfully added.");
            return RedirectToPage("Index");
        }
    }
}