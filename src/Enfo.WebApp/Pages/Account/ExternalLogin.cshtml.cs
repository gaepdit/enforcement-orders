using Enfo.Domain.Users.Entities;
using Enfo.Domain.Users.Services;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Constants;
using Enfo.WebApp.Platform.Local;
using Enfo.WebApp.Platform.RazorHelpers;
using Enfo.WebApp.Platform.Settings;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Enfo.WebApp.Pages.Account;

[AllowAnonymous]
public class ExternalLogin : PageModel
{
    [BindProperty]
    public ApplicationUser DisplayFailedUser { get; set; }

    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    private readonly IUserService _userService;

    public ExternalLogin(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IWebHostEnvironment environment,
        IUserService userService) =>
        (_signInManager, _userManager, _configuration, _environment, _userService) =
        (signInManager, userManager, configuration, environment, userService);

    // Don't call the page directly
    [UsedImplicitly]
    public IActionResult OnGet() => RedirectToPage("./Login");

    // This Post method is called by the Login page
    [UsedImplicitly]
    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        // If "test" users is enabled, create user information and sign in locally.
        if (_environment.IsLocalEnv()) return await SignInAsLocalUser();

        // Request a redirect to the external login provider.
#pragma warning disable 618
        const string provider = AzureADDefaults.AuthenticationScheme;
#pragma warning restore 618
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);

        async Task<IActionResult> SignInAsLocalUser()
        {
            if(!ApplicationSettings.LocalDevSettings.AuthenticatedUser) return Forbid();

            var userView = (await _userService.GetUsersAsync(null, null, null)).First();
            var user = new ApplicationUser
            {
                Email = userView.Email,
                FamilyName = userView.Name,
                Id = userView.Id,
                UserName = userView.Email,
            };

            var userExists = await _userManager.FindByEmailAsync(user.Email);
            if (userExists == null)
            {
                await _userManager.CreateAsync(user);
                foreach (var role in UserRole.AllRoles) await _userManager.AddToRoleAsync(user, role.Key);
            }

            await _signInManager.SignInAsync(userExists ?? user, false);
            return LocalRedirect(returnUrl ?? "/");
        }
    }

    // This method is called by the external login provider.
    [UsedImplicitly]
    [SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high")]
    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
        if (remoteError != null)
        {
            TempData.SetDisplayMessage(Context.Error, $"Error from work account provider: {remoteError}");
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        // Get information about the user from the external provider.
        var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();

        if (externalLoginInfo == null
            || !externalLoginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier)
            || !externalLoginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        {
            TempData.SetDisplayMessage(Context.Error, "Error loading work account information.");
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        // Sign in the user with the external provider.
        var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey, true, true);

        if (signInResult.Succeeded) return LocalRedirect(returnUrl ?? "/");

        if (signInResult.IsLockedOut || signInResult.IsNotAllowed || signInResult.RequiresTwoFactor)
            return RedirectToPage("./Unavailable");

        // If ExternalLoginInfo successfully returned from external provider, but ExternalLoginSignInAsync
        // failed, local account may need to be configured. Start by checking if an account exists.
        var userEmail = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        var existingUser = await _userManager.FindByNameAsync(userEmail);

        // If user account exists, add the external provider info to the user and sign in.
        if (existingUser != null) return await AddProviderAndSignInUserAsync(existingUser, externalLoginInfo);

        // If the user does not have an account, then create one and sign in.
        var newUser = new ApplicationUser
        {
            Email = userEmail,
            UserName = externalLoginInfo.Principal.FindFirstValue(ClaimConstants.PreferredUserName) ?? userEmail,
            SubjectId = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.NameIdentifier),
            ObjectId = externalLoginInfo.Principal.FindFirstValue(ClaimConstants.ObjectId),
            GivenName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
            FamilyName = externalLoginInfo.Principal.FindFirstValue(ClaimConstants.FamilyName)
        };

        // Create the user in the backing store.
        var createUserResult = await _userManager.CreateAsync(newUser);
        if (!createUserResult.Succeeded) return FailedLogin(createUserResult, newUser);

        // Add new user to application Roles if seeded in app settings.
        var seedUsers = _configuration.GetSection("SeedAdminUsers").Get<string[]>().AsEnumerable();
        if (seedUsers != null && seedUsers.Contains(newUser.Email, StringComparer.InvariantCultureIgnoreCase))
            foreach (var role in UserRole.AllRoles)
                await _userManager.AddToRoleAsync(newUser, role.Key);

        // Add the external provider info to the user and sign in.
        return await AddProviderAndSignInUserAsync(newUser, externalLoginInfo);

        // Local function: Add external provider info to user account, sign in user, and redirect
        // to original requested URL.
        async Task<IActionResult> AddProviderAndSignInUserAsync(ApplicationUser user, ExternalLoginInfo loginInfo)
        {
            var addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
            if (!addLoginResult.Succeeded) return FailedLogin(addLoginResult, user);

            // Include the access token in the properties.
            var props = new AuthenticationProperties();
            props.StoreTokens(loginInfo.AuthenticationTokens);
            props.IsPersistent = true;

            await _signInManager.SignInAsync(user, true);
            return LocalRedirect(returnUrl ?? "/");
        }

        // Local function: Add errors from failed login and return this Page.
        IActionResult FailedLogin(IdentityResult result, ApplicationUser user)
        {
            DisplayFailedUser = user;
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }
    }
}
