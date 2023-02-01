﻿using Enfo.Domain.Users.Entities;
using Enfo.WebApp.Platform.Local;
using Enfo.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Account;

[AllowAnonymous]
public class Logout : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IWebHostEnvironment _environment;

    public Logout(SignInManager<ApplicationUser> signInManager, IWebHostEnvironment environment)
    {
        _signInManager = signInManager;
        _environment = environment;
    }

    public Task<IActionResult> OnGetAsync() => LogOutAndRedirectToIndex();

    public Task<IActionResult> OnPostAsync() => LogOutAndRedirectToIndex();

    private async Task<IActionResult> LogOutAndRedirectToIndex()
    {
        // If Azure AD is enabled, sign out all authentication schemes.
        if (!_environment.IsLocalEnv() || ApplicationSettings.LocalDevSettings.UseAzureAd)
            return SignOut(new AuthenticationProperties { RedirectUri = "/Index" },
                IdentityConstants.ApplicationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);

        // If a local user is enabled instead, sign out locally and redirect to home page.
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }
}
