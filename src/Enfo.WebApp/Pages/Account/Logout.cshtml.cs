﻿using System;
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
        public IActionResult OnGet() => RedirectToPage("/Index");

#pragma warning disable 618
        [UsedImplicitly]
        public async Task<IActionResult> OnPostAsync([FromServices] SignInManager<ApplicationUser> signInManager)
        {
            if (Environment.GetEnvironmentVariable("ENABLE_TEST_USER") != "true")
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
}