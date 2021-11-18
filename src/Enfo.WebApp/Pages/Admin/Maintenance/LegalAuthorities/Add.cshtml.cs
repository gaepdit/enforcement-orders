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
    public class Add : PageModel
    {
        [BindProperty]
        public LegalAuthorityCommand Item { get; init; }

        public static MaintenanceOption ThisOption => MaintenanceOption.LegalAuthority;

        [TempData]
        public int HighlightId { get; [UsedImplicitly] set; }

        [UsedImplicitly]
        public static void OnGet()
        {
            // Method intentionally left empty.
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync([FromServices] ILegalAuthorityRepository repository)
        {
            if (!ModelState.IsValid) return Page();

            var result = await Item.TrySaveNew(repository);

            if (result.Success)
            {
                HighlightId = result.NewId.GetValueOrDefault();
                TempData?.SetDisplayMessage(Context.Success, $"{Item.AuthorityName} successfully added.");
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