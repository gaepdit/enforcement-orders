using System;
using System.Threading.Tasks;
using Enfo.Domain.Entities.Users;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> OnPostAsync([FromServices] SignInManager<ApplicationUser> signInManager)
        {
            if (Environment.GetEnvironmentVariable("ENABLE_TEST_USER") != "true")
                return SignOut("Identity.Application", "Identity.External", AzureADDefaults.OpenIdScheme);

            await signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}