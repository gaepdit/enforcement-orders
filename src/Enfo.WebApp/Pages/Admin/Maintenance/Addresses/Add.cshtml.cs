using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.Address;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Addresses
{
    public class Add : PageModel
    {
        [BindProperty]
        public AddressCreate Item { get; set; }

        public static MaintenanceOption ThisOption { get; } = MaintenanceOption.Address;

        [TempData]
        public int NewId { get; set; }

        private readonly IAddressRepository _repository;
        public Add(IAddressRepository repository) => _repository = repository;

        [UsedImplicitly]
        public static void OnGet() { }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            Item.TrimAll();

            if (!ModelState.IsValid) return Page();

            NewId = await _repository.CreateAsync(Item);
            TempData?.SetDisplayMessage(Context.Success, $"{ThisOption.SingularName} successfully added.");
            return RedirectToPage("Index");
        }
    }
}