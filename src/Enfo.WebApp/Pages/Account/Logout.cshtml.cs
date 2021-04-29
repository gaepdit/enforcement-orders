using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Account
{
    [AllowAnonymous]
    public class Logout : PageModel
    {
        [UsedImplicitly]
        public IActionResult OnGet() =>
            SignOut("Identity.Application", "Identity.External", AzureADDefaults.OpenIdScheme);

        [UsedImplicitly]
        public IActionResult OnPost() =>
            SignOut("Identity.Application", "Identity.External", AzureADDefaults.OpenIdScheme);
    }
}