using Enfo.Domain.Entities.Users;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.EpdContact;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Enfo.WebApp.Pages.Admin.Maintenance.Contacts
{
    [Authorize(Roles = UserRole.SiteMaintenance)]
    public class Edit : PageModel
    {
        [BindProperty]
        public EpdContactCommand Item { get; set; }

        [TempData, UsedImplicitly]
        public int HighlightId { [UsedImplicitly] get; set; }

        public static MaintenanceOption ThisOption => MaintenanceOption.EpdContact;

        private readonly IEpdContactRepository _repository;
        public Edit(IEpdContactRepository repository) => _repository = repository;

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var originalItem = await _repository.GetAsync(id.Value);
            if (originalItem == null) return NotFound("ID not found.");

            if (!originalItem.Active)
            {
                TempData.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
                return RedirectToPage("Index");
            }

            Item = new EpdContactCommand(originalItem);
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            if (Item.Id is null) return BadRequest();

            var originalItem = await _repository.GetAsync(Item.Id.Value);
            if (originalItem == null) return NotFound();

            if (!originalItem.Active)
            {
                TempData.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
                return RedirectToPage("Index");
            }

            if (!ModelState.IsValid) return Page();

            try
            {
                await _repository.UpdateAsync(Item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(Item.Id.Value)) return NotFound();
                throw;
            }

            HighlightId = Item.Id.Value;
            TempData.SetDisplayMessage(Context.Success, $"{ThisOption.SingularName} successfully updated.");
            return RedirectToPage("Index");
        }
    }
}
