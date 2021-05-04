using System.Threading.Tasks;
using Enfo.Domain.Entities.Users;
using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.LegalAuthority;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthorities
{
    [Authorize(Roles = UserRole.SiteMaintenance)]
    public class Add : PageModel
    {
        [BindProperty]
        public LegalAuthorityCreate Item { get; init; }

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
            Item.TrimAll();

            if (await repository.NameExistsAsync(Item.AuthorityName))
            {
                ModelState.AddModelError("Item.AuthorityName", "The authority name entered already exists.");
            }

            if (!ModelState.IsValid) return Page();

            HighlightId = await repository.CreateAsync(Item);
            TempData?.SetDisplayMessage(Context.Success, $"{Item.AuthorityName} successfully added.");
            return RedirectToPage("Index");
        }
    }
}