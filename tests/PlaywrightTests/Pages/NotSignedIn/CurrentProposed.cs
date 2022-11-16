using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests.Pages.NotSignedIn;

public class CurrentProposed : PageTest
{

    [Test]
    public async Task TestCurrentProposed()
    {
        /**
        await Page.GotoAsync("https://localhost:44331/");

        // click on the link
        await Page.Locator("div:has-text(\"Georgia EPD provides notice and opportunity for public comment on certain propos\")").GetByRole(AriaRole.Link, new() { NameString = "View Full Report" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/CurrentProposed");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Current Proposed Enforcement Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("(Notices that change weekly)")).ToBeVisibleAsync();

        // check to see if there are these text in the table
        await Expect(Page.GetByRole(AriaRole.Row, new() { NameString = "Facility fgh15-554cc092-5ebe-4784-9fb5-b5ea7c3cf0c6" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Facility" })).ToBeVisibleAsync();
        await Expect(Page.Locator("tbody:has-text(\"Facility fgh15-554cc092-5ebe-4784-9fb5-b5ea7c3cf0c6 County Stephens County Cause\")").GetByRole(AriaRole.Rowheader, new() { NameString = "County" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Row, new() { NameString = "Cause of Order fgh15-23e94241-5033-4441-8494-843bd0754dd6" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Cause of Order" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Row, new() { NameString = "Requirements of Order fgh15-dd999dee-3dd8-4ece-a9ba-4822ada1bb05" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Requirements of Order" })).ToBeVisibleAsync();
        await Expect(Page.Locator("tbody:has-text(\"Facility fgh15-554cc092-5ebe-4784-9fb5-b5ea7c3cf0c6 County Stephens County Cause\")").GetByRole(AriaRole.Rowheader, new() { NameString = "Proposed Settlement Amount" })).ToBeVisibleAsync();
        await Expect(Page.Locator("tbody:has-text(\"Facility fgh15-554cc092-5ebe-4784-9fb5-b5ea7c3cf0c6 County Stephens County Cause\")").GetByRole(AriaRole.Rowheader, new() { NameString = "Legal Authority" })).ToBeVisibleAsync();
        await Expect(Page.Locator("tbody:has-text(\"Facility fgh15-554cc092-5ebe-4784-9fb5-b5ea7c3cf0c6 County Stephens County Cause\")").GetByRole(AriaRole.Rowheader, new() { NameString = "Publication Date For Proposed Order" })).ToBeVisibleAsync();
        await Expect(Page.Locator("tbody:has-text(\"Facility fgh15-554cc092-5ebe-4784-9fb5-b5ea7c3cf0c6 County Stephens County Cause\")").GetByRole(AriaRole.Rowheader, new() { NameString = "Date Comment Period Closes" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Row, new() { NameString = "Send Comments To A. Jones Chief, Air Protection Branch Environmental Protection Division 4244 International Parkway Suite 120 Atlanta, GA 30354 example@example.com 404-555-1212" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Send Comments To" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Row, new() { NameString = "Public Hearing Scheduled Hearing Date/Time 18‑Nov‑2022, 12:00 AM Hearing Location fgh15-f65b4afa-d534-4f86-b97d-13871e82fc0f Date Hearing Comment Period Closes 18‑Nov‑2022 Hearing Information Contact A. Jones Chief, Air Protection Branch Environmental Protection Division 4244 International Parkway Suite 120 Atlanta, GA 30354 example@example.com 404-555-1212" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Public Hearing Scheduled" })).ToBeVisibleAsync();
        */
    }
}
