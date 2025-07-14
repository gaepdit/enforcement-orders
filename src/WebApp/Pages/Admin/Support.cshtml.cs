using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin;

[Authorize]
public class Support : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }
}
