using System.Threading.Tasks;
using Enfo.Repository;
using Enfo.Repository.Repositories;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin
{
    // [Authorize(Roles = UserRole.OrderAdministrator)]
    public class Restore : PageModel
    {
        [BindProperty]
        [HiddenInput]
        public int Id { get; set; }

        public EnforcementOrderAdminView Item { get; set; }

        private readonly IEnforcementOrderRepository _repository;
        public Restore(IEnforcementOrderRepository repository) => _repository = repository;

        [UsedImplicitly]
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            Item = await _repository.GetAdminViewAsync(id.Value);
            if (Item == null) return NotFound("ID not found.");
            if (!Item.Deleted) return RedirectToPage("Details", new {Id});
            Id = id.Value;
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync()
        {
            await _repository.RestoreAsync(Id);
            TempData?.SetDisplayMessage(Context.Success, "The Order has been successfully restored.");
            return RedirectToPage("Details", new {Id});
        }
    }
}