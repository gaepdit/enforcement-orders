using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Domain.Entities.Users;
using Enfo.Domain.Resources.Users;
using Enfo.Domain.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Pages.Admin.Users
{
    [Authorize]
    public class Index : PageModel
    {
        public string Name { get; init; }

        [EmailAddress]
        public string Email { get; init; }

        public string Role { get; init; }

        public IEnumerable<SelectListItem> RoleItems { get; } =
            UserRole.AllRoles.Select(d => new SelectListItem(d.Value.DisplayName, d.Key));

        public bool ShowResults { get; private set; }
        public List<UserView> SearchResults { get; private set; }

        [UsedImplicitly]
        public static void OnGet()
        {
            // Method intentionally left empty.
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnGetSearchAsync([FromServices] IUserService userService,
            string name, string email, string role)
        {
            if (!ModelState.IsValid) return Page();
            SearchResults = await userService.GetUsersAsync(name, email, role);
            ShowResults = true;
            return Page();
        }
    }
}