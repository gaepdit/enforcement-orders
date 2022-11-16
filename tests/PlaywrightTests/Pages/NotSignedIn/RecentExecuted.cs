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

        // await Page.PauseAsync();

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Recently Executed Enforcement Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("(Notices that change weekly)")).ToBeVisibleAsync();

        // check to see if there are these text in the table
        // get number of table rows
        string nameRow1 = await Page.Locator("//table[1]/tbody/tr[1]/th").InnerTextAsync();
        string nameRow2 = await Page.Locator("//table[1]/tbody/tr[2]/th").InnerTextAsync();
        string nameRow3 = await Page.Locator("//table[1]/tbody/tr[3]/th").InnerTextAsync();
        string nameRow4 = await Page.Locator("//table[1]/tbody/tr[4]/th").InnerTextAsync();
        string nameRow5 = await Page.Locator("//table[1]/tbody/tr[5]/th").InnerTextAsync();
        string nameRow6 = await Page.Locator("//table[1]/tbody/tr[6]/th").InnerTextAsync();
        string nameRow7 = await Page.Locator("//table[1]/tbody/tr[7]/th").InnerTextAsync();

        // assertion for the value
        Assert.AreEqual("Facility", nameRow1);
        Assert.AreEqual("County", nameRow2);
        Assert.AreEqual("Cause of Order", nameRow3);
        Assert.AreEqual("Requirements of Order", nameRow4);
        Assert.AreEqual("Settlement Amount", nameRow5);
        Assert.AreEqual("Legal Authority", nameRow6);
        Assert.AreEqual("Date Executed", nameRow7);
    }
}