using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PlaywrightTests.Pages.SignedIn;

[Parallelizable(ParallelScope.None)]
[TestFixture]
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
public class SignInOut : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions() => PlaywrightHelpers.DefaultContextOptions();

    [TearDown]
    public async Task TearDown() => await PlaywrightHelpers.LogOutAsync(Page);

    [Test]
    public async Task TestSignInAndSignOutProcesses()
    {
        await Page.GotoAsync("/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync("Georgia EPD Enforcement Orders");

        // Expect sign in link in navigation
        var signInLink = Page.GetByRole(AriaRole.Navigation).GetByRole(AriaRole.Link, new (){NameString = "Sign in"});
        await Expect(signInLink).ToBeVisibleAsync();
    
        // Go to sign in page
        await signInLink.ClickAsync();
        await Page.WaitForURLAsync("/Account/Login");

        // Expect the following text in the home page
        await Expect(Page).ToHaveTitleAsync(new Regex("Agency Login"));
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Agency Login" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("The Enforcement Orders admin site is a State of Georgia application."))
            .ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Sign in using your work account" }))
            .ToBeVisibleAsync();

        // Sign in
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("/Admin/Index");

        // Expect Account button in navigation
        var accountButton = Page.GetByRole(AriaRole.Navigation).GetByRole(AriaRole.Button, new (){NameString = "Account"});
        await Expect(accountButton).ToBeVisibleAsync();

        // Open account menu dropdown
        await accountButton.ClickAsync();

        // Expect sign out button
        var signOutButton = Page.GetByRole(AriaRole.Button, new() { NameString = "Sign out" });
        await Expect(signOutButton).ToBeVisibleAsync();
        
        // Sign out
        await signOutButton.ClickAsync();
        await Page.WaitForURLAsync("/");

        // Expect sign in link in navigation again
        await Expect(signInLink).ToBeVisibleAsync();
    }
}
