using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests.Pages.NotSignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SearchOrders : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions() =>
        new()
        {
            BaseURL = "https://localhost:44331",
            IgnoreHTTPSErrors = true,
        };

    [Test]
    public async Task TestSearchOrdersDefaultTable()
    {
        await Page.GotoAsync("/");

        // click on the link
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await Page.WaitForURLAsync("/Search");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" }))
            .ToBeVisibleAsync();

        // search table with no values
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Expect(Page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Expect(Page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Expect(Page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Expect(Page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Expect(Page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Expect(Page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Expect(Page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Expect(Page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Expect(Page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Expect(Page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Expect(Page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Expect(Page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
        await Expect(Page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("");
    }

    [Test]
    public async Task TestSearchOrdersSortTableByAscendingStatusDate()
    {
        await Page.GotoAsync("/");

        // click on the link
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await Page.WaitForURLAsync("/Search");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" }))
            .ToBeVisibleAsync();

        // search table with no values
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // change the filter from descending to ascending order
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Status/Date ▼" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&Sort=DateAsc&handler=search#search-results");

        // check the number of tables
        numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Expect(Page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("");
        await Expect(Page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Expect(Page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Expect(Page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
        await Expect(Page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Expect(Page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Expect(Page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Expect(Page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Expect(Page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Expect(Page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Expect(Page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Expect(Page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Expect(Page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
    }

    [Test]
    public async Task TestSearchOrdersSortTableByDescendingStatusDate()
    {
        await Page.GotoAsync("/");

        // click on the link
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await Page.WaitForURLAsync("/Search");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" }))
            .ToBeVisibleAsync();

        // search table with no values
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // change the filter from descending to ascending order
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Status/Date ▼" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&Sort=DateAsc&handler=search#search-results");
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Status/Date ▲" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&Sort=DateDesc&handler=search#search-results");

        // check the number of tables
        numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Expect(Page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("");
        await Expect(Page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
        await Expect(Page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Expect(Page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Expect(Page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Expect(Page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Expect(Page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Expect(Page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Expect(Page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Expect(Page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Expect(Page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Expect(Page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Expect(Page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
    }

    [Test]
    public async Task TestSearchOrdersSortTableByAscendingFacility()
    {
        await Page.GotoAsync("/");

        // click on the link
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await Page.WaitForURLAsync("/Search");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" }))
            .ToBeVisibleAsync();

        // search table with no values
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // filter the table to be ascending facility
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Facility" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&Sort=FacilityAsc&handler=search#search-results");

        // check the number of tables
        numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Expect(Page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Expect(Page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Expect(Page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Expect(Page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Expect(Page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Expect(Page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Expect(Page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("");
        await Expect(Page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Expect(Page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Expect(Page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Expect(Page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Expect(Page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Expect(Page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
    }

    [Test]
    public async Task TestSearchOrdersSortTableByDescendingFacility()
    {
        await Page.GotoAsync("/");

        // click on the link
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await Page.WaitForURLAsync("/Search");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" }))
            .ToBeVisibleAsync();

        // search table with no values
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // filter the table to be descending facility
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Facility" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&Sort=FacilityAsc&handler=search#search-results");
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Facility ▲" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&Sort=FacilityDesc&handler=search#search-results");

        // check the number of tables
        numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Expect(Page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Expect(Page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Expect(Page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Expect(Page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Expect(Page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Expect(Page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Expect(Page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("");
        await Expect(Page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Expect(Page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Expect(Page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Expect(Page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Expect(Page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Expect(Page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
    }

    [Test]
    public async Task TestSearchOrdersClearForm()
    {
        await Page.GotoAsync("/");

        // click on the link
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await Page.WaitForURLAsync("/Search");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the Page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" }))
            .ToBeVisibleAsync();

        // search table with no values
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("**/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // click on the clear form button
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Clear Form" }).ClickAsync();
        await Page.WaitForURLAsync("/Search");

        // check the number of tables
        numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(0));
    }
}
