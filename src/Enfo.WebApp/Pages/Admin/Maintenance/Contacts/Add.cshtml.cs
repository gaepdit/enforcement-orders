using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.Address;
using Enfo.Repository.Resources.EpdContact;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Contacts
{
    public class Add : PageModel
    {
        [BindProperty]
        public EpdContactCreate Item { get; set; }

        public SelectList AddressSelectList { get; private set; }

        public static MaintenanceOption ThisOption { get; } = MaintenanceOption.EpdContact;

        [TempData]
        public int HighlightId { get; set; }

        private readonly IEpdContactRepository _repository;
        private readonly IAddressRepository _address;

        public Add(IEpdContactRepository repository, IAddressRepository address) =>
            (_repository, _address) = (repository, address);

        [UsedImplicitly]
        public async Task OnGetAsync() => await PopulateSelectListsAsync();

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            Item.TrimAll();

            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return Page();
            }

            HighlightId = await _repository.CreateAsync(Item);
            TempData?.SetDisplayMessage(Context.Success, $"{ThisOption.SingularName} successfully added.");
            return RedirectToPage("Index");
        }

        private async Task PopulateSelectListsAsync() =>
            AddressSelectList = new SelectList(await _address.ListAsync(), nameof(AddressView.Id),
                nameof(AddressView.AsLinearString));
    }
}