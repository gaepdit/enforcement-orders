using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin.Maintenance;

[Authorize]
public class Index : PageModel
{
    [UsedImplicitly]
    public static void OnGet()
    {
        // Method intentionally left empty.
    }
}
