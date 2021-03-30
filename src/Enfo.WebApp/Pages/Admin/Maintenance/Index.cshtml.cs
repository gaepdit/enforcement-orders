using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Maintenance
{
    // TODO: Auth
    // [Authorize(Roles = UserRole.SiteMaintenance)]
    public class Index : PageModel
    {
        [UsedImplicitly]
        public static void OnGet() { }
    }
}