using System.Threading.Tasks;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.LegalAuthority;
using Enfo.WebApp.Extensions;
using Enfo.WebApp.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Maintenance.LegalAuthority
{
    public class Add : PageModel
    {
        [BindProperty]
        public LegalAuthorityCreate Item { get; set; }

        public static MaintenanceOption ThisOption { get; } = MaintenanceOption.LegalAuthority;

        [TempData]
        public int NewId { get; set; }

        private readonly ILegalAuthorityRepository _repository;
        public Add(ILegalAuthorityRepository repository) => _repository = repository;

        [UsedImplicitly]
        public static void OnGet() { }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            Item.TrimAll();

            if (await _repository.NameExistsAsync(Item.AuthorityName))
            {
                ModelState.AddModelError("Item.AuthorityName", "The authority name entered already exists.");
            }

            if (!ModelState.IsValid) return Page();

            NewId = await _repository.CreateAsync(Item);
            TempData?.SetDisplayMessage(Context.Success, $"{Item.AuthorityName} successfully added.");
            return RedirectToPage("Index");
        }
    }
}