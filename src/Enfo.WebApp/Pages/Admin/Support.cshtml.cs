using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin;

[Authorize]
public class Support : PageModel
{
    public string SupportEmail { get; private set; }

    public void OnGet([FromServices] IConfiguration configuration) =>
        SupportEmail = configuration["SupportEmail"];
}
