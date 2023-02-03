using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PlaywrightTests.Pages.NotSignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
public class NotSignedIn : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions() => PlaywrightHelpers.DefaultContextOptions();

    [Test]
    public async Task WhenNotSignedInAccountPageRedirectsToLoginPage()
    {
        await Page.GotoAsync("/Account");

        // Expect the login page.
        await Page.WaitForURLAsync("/Account/Login?ReturnUrl=%2FAccount");
        await Expect(Page).ToHaveTitleAsync(new Regex("Agency Login"));
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Agency Login" })).ToBeVisibleAsync();
    }
}
