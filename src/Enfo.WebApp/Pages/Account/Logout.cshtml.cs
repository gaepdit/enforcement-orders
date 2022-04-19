using Enfo.Domain.Users.Entities;
using Enfo.WebApp.Platform.Local;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Account;

[AllowAnonymous]
public class Logout : PageModel
{
    [UsedImplicitly]
    public IActionResult OnGet() => RedirectToPage("/Index");

#pragma warning disable 618
    [UsedImplicitly]
    public async Task<IActionResult> OnPostAsync(
        [FromServices] SignInManager<ApplicationUser> signInManager,
        [FromServices] IWebHostEnvironment environment)
    {
        if (!environment.IsLocalEnv())
        {
            return SignOut(IdentityConstants.ApplicationScheme, IdentityConstants.ExternalScheme,
                AzureADDefaults.OpenIdScheme);
        }

        // If "test" users is enabled, sign out locally and redirect to home page.
        await signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }
#pragma warning restore 618
}
