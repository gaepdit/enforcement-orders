using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Maintenance;

[Authorize]
public class Index : PageModel
{
    public static void OnGet()
    {
        // Method intentionally left empty.
    }
}
