using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests.Pages.NotSignedIn;

public class RecentExecuted : PageTest
{

    [Test]
    public async Task TestRecentExecuted()
    {
        await Page.GotoAsync("https://localhost:44331/");

        // click the button
        await Page.Locator("div:has-text(\"Georgia EPD issues a notice of fully executed administrative orders and fully ex\")").GetByRole(AriaRole.Link, new() { NameString = "View Full Report" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/RecentExecuted");

        await Page.PauseAsync();

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Recently Executed Enforcement Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("(Notices that change weekly)")).ToBeVisibleAsync();

        // check to see if there are these text in the table
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Enforcement Order EPD-WP-0002" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Row, new() { NameString = "Facility bcd2-97464bac-dc08-4bc5-b8a1-4db5d3c6d44f" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Facility" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Table, new() { NameString = "Enforcement Order EPD-WP-0002" }).GetByRole(AriaRole.Rowheader, new() { NameString = "County" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Row, new() { NameString = "Cause of Order bcd2-b2d4e9da-1ade-4bbb-98f1-8609ad76afcf" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Cause of Order" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Row, new() { NameString = "Requirements of Order bcd2-f60afbea-ff6a-420e-8ea1-07623d9f8017" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Requirements of Order" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Table, new() { NameString = "Enforcement Order EPD-WP-0002" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Settlement Amount" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Table, new() { NameString = "Enforcement Order EPD-WP-0002" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Legal Authority" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Table, new() { NameString = "Enforcement Order EPD-WP-0002" }).GetByRole(AriaRole.Rowheader, new() { NameString = "Date Executed" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "bcd2-97464bac-dc08-4bc5-b8a1-4db5d3c6d44f" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Table, new() { NameString = "Enforcement Order EPD-WP-0002" }).GetByRole(AriaRole.Cell, new() { NameString = "Butts County" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "bcd2-b2d4e9da-1ade-4bbb-98f1-8609ad76afcf" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "bcd2-f60afbea-ff6a-420e-8ea1-07623d9f8017" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Table, new() { NameString = "Enforcement Order EPD-WP-0002" }).GetByRole(AriaRole.Cell, new() { NameString = "$1.50" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Table, new() { NameString = "Enforcement Order EPD-WP-0002" }).GetByRole(AriaRole.Cell, new() { NameString = "Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Table, new() { NameString = "Enforcement Order EPD-WP-0002" }).GetByRole(AriaRole.Cell, new() { NameString = "1‑Nov‑2022" })).ToBeVisibleAsync();

        await Page.PauseAsync();
    }
}
