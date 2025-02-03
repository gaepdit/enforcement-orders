using Enfo.Domain.Users.Entities;
using Enfo.Domain.Users.Services;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Local;
using Enfo.WebApp.Platform.RazorHelpers;
using Enfo.WebApp.Platform.Settings;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace Enfo.WebApp.Pages.Account;

[AllowAnonymous]
public class ExternalLogin(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    IWebHostEnvironment environment,
    IUserService userService,
    ILogger<ExternalLogin> logger)
    : PageModel
{
    public ApplicationUser DisplayFailedUser { get; set; }
    public string ReturnUrl { get; private set; }

    // Don't call the page directly
    [UsedImplicitly]
    public IActionResult OnGet() => RedirectToPage("./Login");

    // This Post method is called by the Login page
    [UsedImplicitly]
    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;

        // If a local user is enabled, create user information and sign in locally.
        if (environment.IsLocalEnv() && !ApplicationSettings.LocalDevSettings.UseAzureAd)
            return await SignInAsLocalUser();

        // Request a redirect to the external login provider.
        const string provider = OpenIdConnectDefaults.AuthenticationScheme;
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    private async Task<IActionResult> SignInAsLocalUser()
    {
        logger.LogInformation("Local user signin attempted with settings {AuthenticatedUser}",
            ApplicationSettings.LocalDevSettings.AuthenticatedUser.ToString());
        if (!ApplicationSettings.LocalDevSettings.AuthenticatedUser) return Forbid();

        var localUser = (await userService.GetUsersAsync(null, null, null))[0];
        var existingUser = await userManager.FindByEmailAsync(localUser.Email);

        if (existingUser == null)
        {
            var newUser = new ApplicationUser
            {
                Email = localUser.Email,
                FamilyName = localUser.Name,
                Id = localUser.Id,
                UserName = localUser.Email,
                ObjectId = localUser.ObjectId,
                MostRecentLogin = DateTimeOffset.Now,
            };

            await userManager.CreateAsync(newUser);
            foreach (var role in UserRole.AllRoles) await userManager.AddToRoleAsync(newUser, role.Key);
            await signInManager.SignInAsync(newUser, false);
        }
        else
        {
            existingUser.MostRecentLogin = DateTimeOffset.Now;
            await userManager.UpdateAsync(existingUser);
            await signInManager.SignInAsync(existingUser, false);
        }

        return LocalRedirectOrHome();
    }

    // This callback method is called by the external login provider.
    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
        ReturnUrl = returnUrl;

        // Handle errors returned from the external provider.
        if (remoteError is not null)
            return RedirectToLoginPageWithError($"Error from work account provider: {remoteError}");

        // Get information about the user from the external provider.
        var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo?.Principal is null)
            return RedirectToLoginPageWithError("Error loading work account information.");

        var userTenant = externalLoginInfo.Principal.GetTenantId();
        var userEmail = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

        if (userEmail is null || userTenant is null)
            return RedirectToLoginPageWithError("Error loading detailed work account information.");

        if (!userEmail.IsValidEmailDomain())
        {
            logger.LogWarning("User with invalid email domain attempted signin");
            return RedirectToPage("./Unavailable");
        }

        logger.LogInformation("User with object ID {ObjectId} in tenant {TenantID} successfully authenticated",
            externalLoginInfo.Principal.GetObjectId(), userTenant);

        // Determine if a user account already exists with the Object ID.
        // If not, then determine if a user account already exists with the given username.
        var user = await userManager.Users
                       .SingleOrDefaultAsync(au => au.ObjectId == externalLoginInfo.Principal.GetObjectId()) ??
                   await userManager.FindByNameAsync(userEmail);

        // If the user does not have an account yet, then create one and sign in.
        if (user is null)
            return await CreateUserAndSignInAsync(externalLoginInfo);

        // Try to sign in the user locally with the external provider key.
        var signInResult = await signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey, true, true);

        if (signInResult.IsLockedOut || signInResult.IsNotAllowed || signInResult.RequiresTwoFactor)
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("./Unavailable");
        }

        user.MostRecentLogin = DateTimeOffset.Now;
        await userManager.UpdateAsync(user);

        // If ExternalLoginInfo was successfully returned from the external login provider and the user exists, but
        // ExternalLoginSignInAsync (signInResult) failed, then add the external login provider to the user and sign in.
        return signInResult.Succeeded
            ? await RefreshUserInfoAndSignInAsync(user, externalLoginInfo)
            : await AddLoginProviderAndSignInAsync(user, externalLoginInfo);
    }

    // Redirect to Login page with error message.
    private RedirectToPageResult RedirectToLoginPageWithError(string message)
    {
        logger.LogWarning("External login error: {Message}", message);
        TempData.SetDisplayMessage(Context.Error, message);
        return RedirectToPage("./Login", new { ReturnUrl });
    }

    // Create a new user account and sign in.
    private async Task<IActionResult> CreateUserAndSignInAsync(ExternalLoginInfo info)
    {
        var user = new ApplicationUser
        {
            UserName = info.Principal.GetDisplayName(),
            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
            GivenName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty,
            FamilyName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty,
            ObjectId = info.Principal.GetObjectId(),
            MostRecentLogin = DateTimeOffset.Now,
        };

        // Create the user in the backing store.
        var createUserResult = await userManager.CreateAsync(user);
        if (!createUserResult.Succeeded)
        {
            logger.LogWarning("Failed to create new user with object ID {ObjectId}", user.ObjectId);
            return await FailedLogin(createUserResult, user);
        }

        logger.LogInformation("Created new user with object ID {ObjectId}", user.ObjectId);

        await SeedRoles(user);

        // Add the external provider info to the user and sign in.
        return await AddLoginProviderAndSignInAsync(user, info);
    }

    private async Task SeedRoles(ApplicationUser user)
    {
        // Add new user to application Roles if seeded in app settings (or running locally as local admin user).
        var seedUsers = configuration.GetSection("SeedAdminUsers").Get<string[]>();
        if (environment.IsLocalEnv() ||
            (seedUsers != null && seedUsers.Contains(user.Email, StringComparer.InvariantCultureIgnoreCase)))
        {
            logger.LogInformation("Seeding roles for new user {UserName}", user.UserName);
            foreach (var role in UserRole.AllRoles)
                await userManager.AddToRoleAsync(user, role.Key);
        }
    }

    // Update local store with from external provider. 
    private async Task<IActionResult> RefreshUserInfoAndSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        logger.LogInformation("Existing user with object ID {ObjectId} logged in with {LoginProvider} provider",
            user.ObjectId, info.LoginProvider);

        var externalValues = new ApplicationUser
        {
            UserName = info.Principal.GetDisplayName(),
            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
            GivenName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
            FamilyName = info.Principal.FindFirstValue(ClaimTypes.Surname),
        };

        if (user.UserName != externalValues.UserName || user.Email != externalValues.Email ||
            user.GivenName != externalValues.GivenName || user.FamilyName != externalValues.FamilyName)
        {
            user.UserName = externalValues.UserName;
            user.Email = externalValues.Email;
            user.GivenName = externalValues.GivenName;
            user.FamilyName = externalValues.FamilyName;
            user.ProfileUpdatedAt = DateTimeOffset.Now;
            await userManager.UpdateAsync(user);
        }

        await signInManager.RefreshSignInAsync(user);
        return LocalRedirectOrHome();
    }

    // Add external login provider to user account and sign in user.
    private async Task<IActionResult> AddLoginProviderAndSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        var addLoginResult = await userManager.AddLoginAsync(user, info);

        if (!addLoginResult.Succeeded)
        {
            logger.LogWarning("Failed to add login provider {LoginProvider} for user with object ID {ObjectId}",
                info.LoginProvider, user.ObjectId);
            return await FailedLogin(addLoginResult, user);
        }

        logger.LogInformation("Login provider {LoginProvider} added for user {UserName}", info.LoginProvider,
            user.UserName);

        // Include the access token in the properties.
        var props = new AuthenticationProperties();
        if (info.AuthenticationTokens is not null) props.StoreTokens(info.AuthenticationTokens);
        props.IsPersistent = true;

        await signInManager.SignInAsync(user, props, info.LoginProvider);
        return LocalRedirectOrHome();
    }

    // Add error info and return this Page.
    private async Task<PageResult> FailedLogin(IdentityResult result, ApplicationUser user)
    {
        DisplayFailedUser = user;
        await signInManager.SignOutAsync();
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return Page();
    }

    private IActionResult LocalRedirectOrHome() =>
        ReturnUrl is null ? RedirectToPage("/Index") : LocalRedirect(ReturnUrl);
}
