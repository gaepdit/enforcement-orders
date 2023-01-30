using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests.Pages.SignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class AdminAddOrders : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ColorScheme = ColorScheme.Light,
            IgnoreHTTPSErrors = true
        };
    }
    
    [SetUp]
    public void Setup()
    {
        LogOut().Wait();
    }
    
    private async Task LogOut()
    {
        await Page.GotoAsync("https://localhost:44331/");
        // The account is signed in when there is an Account button
        var isSignedIn = await Page.Locator("text=Account").CountAsync() != 0;
        if (isSignedIn)
        {
            await Page.GetByRole(AriaRole.Button, new() { NameString = "Account" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign out" }).ClickAsync();
            await Page.WaitForURLAsync("https://localhost:44331/");
        }
    }

    [Test]
    public async Task TestAddOrders()
    {
        // Run after any tests
        await Page.GotoAsync("https://localhost:44331/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Click sign in tab
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Account/Login");

        // Expect the following text in the home page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Agency Login" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("The Enforcement Orders admin site is a State of Georgia application. It is provi")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Sign in using your work account" })).ToBeVisibleAsync();

        // click in sign in button
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Index");

        // click on the link
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Admin: Search Enforcement Orders" })).ToBeVisibleAsync();

        // search table with no values
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search?Status=All&Progress=All&handler=search#search-results");

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.GreaterThanOrEqualTo(19));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // click on add new order button
        await Page.GetByRole(AriaRole.Link, new() { NameString = "+ New Order" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Add");

        // enter the facility value
        await Page.GetByLabel("Facility *").ClickAsync();
        await Page.GetByLabel("Facility *").FillAsync("testing");

        // select county
        await Page.GetByRole(AriaRole.Combobox, new() { NameString = "County *" }).SelectOptionAsync(new[] { "Bartow" });

        // enter cause of order
        await Page.GetByLabel("Cause of Order *").ClickAsync();
        await Page.GetByLabel("Cause of Order *").FillAsync("testing");

        // enter legal authority
        await Page.GetByRole(AriaRole.Combobox, new() { NameString = "Legal Authority *" }).SelectOptionAsync(new[] { "1" });

        // enter order number
        await Page.GetByLabel("Order Number *").ClickAsync();
        await Page.GetByLabel("Order Number *").FillAsync("123456789");

        // enter settlement amount
        await Page.GetByLabel("Settlement Amount").ClickAsync();
        await Page.GetByLabel("Settlement Amount").FillAsync("123");

        // select progress
        await Page.GetByRole(AriaRole.Combobox, new() { NameString = "Progress *" }).SelectOptionAsync(new[] { "0" });

        // click button to add the new order
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Add New Order" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Details/29");

        // Expect the following text after the order has been added
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Admin View: Enforcement Order 123456789" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("This Order is not publicly viewable.")).ToBeVisibleAsync();

        // check the number of tables in the new order detail page
        int numTables2 = await Page.Locator("//table").CountAsync();
        Assert.That(numTables2, Is.EqualTo(1));
        // check the number of rows
        int tableRows2 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows2, Is.EqualTo(12));
        // check the column labels
        await Expect(Page.Locator("//table/tbody/tr[1]/th[1]")).ToContainTextAsync("Progress");
        await Expect(Page.Locator("//table/tbody/tr[2]/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/tbody/tr[3]/th[1]")).ToContainTextAsync("County");
        await Expect(Page.Locator("//table/tbody/tr[4]/th[1]")).ToContainTextAsync("Cause of Order");
        await Expect(Page.Locator("//table/tbody/tr[5]/th[1]")).ToContainTextAsync("Requirements of Order");
        await Expect(Page.Locator("//table/tbody/tr[6]/th[1]")).ToContainTextAsync("Proposed Settlement Amount");
        await Expect(Page.Locator("//table/tbody/tr[7]/th[1]")).ToContainTextAsync("Legal Authority");
        await Expect(Page.Locator("//table/tbody/tr[8]/th[1]")).ToContainTextAsync("Publication Date For Proposed Order");
        await Expect(Page.Locator("//table/tbody/tr[9]/th[1]")).ToContainTextAsync("Date Comment Period Closes");
        await Expect(Page.Locator("//table/tbody/tr[10]/th[1]")).ToContainTextAsync("Send Comments To");
        await Expect(Page.Locator("//table/tbody/tr[11]/th[1]")).ToContainTextAsync("File Attachments");
        // check the column values
        await Expect(Page.Locator("//table/tbody/tr[1]/td")).ToContainTextAsync("Draft");
        await Expect(Page.Locator("//table/tbody/tr[2]/td")).ToContainTextAsync("testing");
        await Expect(Page.Locator("//table/tbody/tr[3]/td")).ToContainTextAsync("Bartow County");
        await Expect(Page.Locator("//table/tbody/tr[4]/td")).ToContainTextAsync("testing");
        await Expect(Page.Locator("//table/tbody/tr[5]/td")).ToContainTextAsync("");
        await Expect(Page.Locator("//table/tbody/tr[6]/td")).ToContainTextAsync("123.00");
        await Expect(Page.Locator("//table/tbody/tr[7]/td")).ToContainTextAsync("Air Quality Act");
        await Expect(Page.Locator("//table/tbody/tr[8]/td")).ToContainTextAsync("");
        await Expect(Page.Locator("//table/tbody/tr[9]/td")).ToContainTextAsync("");
        await Expect(Page.Locator("//table/tbody/tr[10]/td")).ToContainTextAsync("N/A");
        await Expect(Page.Locator("//table/tbody/tr[11]/td")).ToContainTextAsync("None");

        // go back and search for the total number of orders
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search");
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search?Status=All&Progress=All&handler=search#search-results");

        // check the number of rows in the first table
        int tableRows3 = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows3, Is.EqualTo(20));
        // check if the value exists in the table
        await Expect(Page.GetByRole(AriaRole.Rowheader, new() { NameString = "testing Bartow County" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "123456789 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Row, new() { NameString = "testing Bartow County 123456789 Air Quality Act Proposed On View" }).GetByText("Proposed On")).ToBeVisibleAsync();


    }
}
