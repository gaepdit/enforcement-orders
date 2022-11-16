using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests.Pages.NotSignedIn;

public class HomePage : PageTest
{

    [Test]
    public async Task TestTextHomePage()
    {
        /**
        await Page.GotoAsync("https://localhost:44331/");

        //await Page.PauseAsync();

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Expect the following text in the home page
        await Expect(Page.GetByText("The Georgia Environmental Protection Division uses enforcement actions to correc")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Proposed Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Executed Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Mail Subscriptions" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("Mail subscriptions to these notices are available at a cost of $50 per year. Tha")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "API Access" })).ToBeVisibleAsync();

        //await Page.PauseAsync();
        */
    }
}
