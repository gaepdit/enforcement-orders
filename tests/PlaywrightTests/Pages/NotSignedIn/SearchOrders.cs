using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightTests.pages.NotSignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SearchOrders
{
    [Test]
    public async Task TestSearchOrdersDefaultTable()
    {
        // launches Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        // Create a new incognito browser context that ignores HTTPS errors
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        // Create a new page inside context.
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:44331/");

        // click on the link
        await page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search");

        // Expect a title "to contain" a substring.
        await Assertions.Expect(page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" })).ToBeVisibleAsync();
        
        // search table with no values
        await page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // check the column labels of the first table
        await Assertions.Expect(page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Assertions.Expect(page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Assertions.Expect(page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Assertions.Expect(page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Assertions.Expect(page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Assertions.Expect(page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Assertions.Expect(page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Assertions.Expect(page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Assertions.Expect(page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Assertions.Expect(page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Assertions.Expect(page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Assertions.Expect(page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Assertions.Expect(page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Assertions.Expect(page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Assertions.Expect(page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
        await Assertions.Expect(page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("");
    }

    [Test]
    public async Task TestSearchOrdersSortTableByAscendingStatusDate()
    {
        // launches Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        // Create a new incognito browser context that ignores HTTPS errors
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        // Create a new page inside context.
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:44331/");

        // click on the link
        await page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search");

        // Expect a title "to contain" a substring.
        await Assertions.Expect(page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" })).ToBeVisibleAsync();
        
        // search table with no values
        await page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // change the filter from descending to ascending order
        await page.GetByRole(AriaRole.Link, new() { NameString = "Status/Date ▼" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&Sort=DateAsc&handler=search#search-results");

        // check the number of tables
        numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // check the column labels of the first table
        await Assertions.Expect(page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Assertions.Expect(page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Assertions.Expect(page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Assertions.Expect(page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("");
        await Assertions.Expect(page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Assertions.Expect(page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Assertions.Expect(page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
        await Assertions.Expect(page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Assertions.Expect(page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Assertions.Expect(page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Assertions.Expect(page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Assertions.Expect(page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Assertions.Expect(page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Assertions.Expect(page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Assertions.Expect(page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Assertions.Expect(page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
    }

    [Test]
    public async Task TestSearchOrdersSortTableByDescendingStatusDate()
    {
        // launches Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        // Create a new incognito browser context that ignores HTTPS errors
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        // Create a new page inside context.
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:44331/");

        // click on the link
        await page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search");

        // Expect a title "to contain" a substring.
        await Assertions.Expect(page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" })).ToBeVisibleAsync();
        
        // search table with no values
        await page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // change the filter from descending to ascending order
        await page.GetByRole(AriaRole.Link, new() { NameString = "Status/Date ▼" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&Sort=DateAsc&handler=search#search-results");
        await page.GetByRole(AriaRole.Link, new() { NameString = "Status/Date ▲" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&Sort=DateDesc&handler=search#search-results");

        // check the number of tables
        numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // check the column labels of the first table
        await Assertions.Expect(page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Assertions.Expect(page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Assertions.Expect(page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Assertions.Expect(page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("");
        await Assertions.Expect(page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
        await Assertions.Expect(page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Assertions.Expect(page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Assertions.Expect(page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Assertions.Expect(page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Assertions.Expect(page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Assertions.Expect(page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Assertions.Expect(page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Assertions.Expect(page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Assertions.Expect(page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Assertions.Expect(page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Assertions.Expect(page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
    }

    [Test]
    public async Task TestSearchOrdersSortTableByAscendingFacility()
    {
        // launches Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        // Create a new incognito browser context that ignores HTTPS errors
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        // Create a new page inside context.
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:44331/");

        // click on the link
        await page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search");

        // Expect a title "to contain" a substring.
        await Assertions.Expect(page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" })).ToBeVisibleAsync();
        
        // search table with no values
        await page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // filter the table to be ascending facility
        await page.GetByRole(AriaRole.Link, new() { NameString = "Facility" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&Sort=FacilityAsc&handler=search#search-results");

        // check the number of tables
        numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // check the column labels of the first table
        await Assertions.Expect(page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Assertions.Expect(page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Assertions.Expect(page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Assertions.Expect(page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Assertions.Expect(page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Assertions.Expect(page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Assertions.Expect(page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Assertions.Expect(page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Assertions.Expect(page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Assertions.Expect(page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("");
        await Assertions.Expect(page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Assertions.Expect(page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Assertions.Expect(page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Assertions.Expect(page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Assertions.Expect(page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Assertions.Expect(page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
    }

    [Test]
    public async Task TestSearchOrdersSortTableByDescendingFacility()
    {
        // launches Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        // Create a new incognito browser context that ignores HTTPS errors
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        // Create a new page inside context.
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:44331/");

        // click on the link
        await page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search");

        // Expect a title "to contain" a substring.
        await Assertions.Expect(page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" })).ToBeVisibleAsync();
        
        // search table with no values
        await page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // filter the table to be descending facility
        await page.GetByRole(AriaRole.Link, new() { NameString = "Facility" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&Sort=FacilityAsc&handler=search#search-results");
        await page.GetByRole(AriaRole.Link, new() { NameString = "Facility ▲" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&Sort=FacilityDesc&handler=search#search-results");

        // check the number of tables
        numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // check the column labels of the first table
        await Assertions.Expect(page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Assertions.Expect(page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Assertions.Expect(page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Assertions.Expect(page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Assertions.Expect(page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Assertions.Expect(page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Assertions.Expect(page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Assertions.Expect(page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Assertions.Expect(page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Assertions.Expect(page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("");
        await Assertions.Expect(page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Assertions.Expect(page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Assertions.Expect(page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Assertions.Expect(page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Assertions.Expect(page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Assertions.Expect(page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
    }

    [Test]
    public async Task TestSearchOrdersClearForm()
    {
        // launches Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        // Create a new incognito browser context that ignores HTTPS errors
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        // Create a new page inside context.
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:44331/");

        // click on the link
        await page.GetByRole(AriaRole.Link, new() { NameString = "Search Orders" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search");

        // Expect a title "to contain" a substring.
        await Assertions.Expect(page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Check for text in the front of the page
        await Assertions.Expect(page.GetByRole(AriaRole.Heading, new() { NameString = "Search Enforcement Orders" })).ToBeVisibleAsync();
        
        // search table with no values
        await page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search?Status=All&handler=search#search-results");

        // check the number of tables
        int numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.EqualTo(13));

        // click on the clear form button
        await page.GetByRole(AriaRole.Link, new() { NameString = "Clear Form" }).ClickAsync();
        await page.WaitForURLAsync("https://localhost:44331/Search");

        // check the number of tables
        numTables = await page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(0));
    }
}
