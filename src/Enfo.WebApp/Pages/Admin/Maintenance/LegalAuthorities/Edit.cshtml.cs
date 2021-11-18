using Enfo.Domain.Entities.Users;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities
{
    [Authorize(Roles = UserRole.SiteMaintenance)]
    public class Edit : PageModel
    {
        [BindProperty]
        public LegalAuthorityCommand Item { get; set; }

        [BindProperty, HiddenInput]
        public int Id { get; set; }

        [TempData, UsedImplicitly]
        public int HighlightId { [UsedImplicitly] get; set; }

        [BindProperty, HiddenInput]
        public string OriginalName { get; set; }
        public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;

        private readonly ILegalAuthorityRepository _repository;
        public Edit(ILegalAuthorityRepository repository) => _repository = repository;

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

            Item = new LegalAuthorityCommand(originalItem);
            Id = id.Value;
            OriginalName = originalItem.AuthorityName;
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await Item.TryUpdate(_repository, Id);

            if (result.Success)
            {
                HighlightId = Id;
                TempData?.SetDisplayMessage(Context.Success, $"{Item.AuthorityName} successfully updated.");
                return RedirectToPage("Index");
            }

            if (result.OriginalItem is null)
            {
                return NotFound();
            }

            if (!result.OriginalItem.Active)
            {
                TempData?.SetDisplayMessage(Context.Warning, $"Inactive {ThisOption.PluralName} cannot be edited.");
                return RedirectToPage("Index");
            }

            foreach (var (key, value) in result.ValidationErrors)
            {
                ModelState.AddModelError(string.Concat(nameof(Item), ".", key), value);
            }

            return Page();
        }
    }
}