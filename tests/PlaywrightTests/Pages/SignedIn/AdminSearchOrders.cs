using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests.Pages.SignedIn;

[Parallelizable(ParallelScope.None)]
[TestFixture]
public class AdminSearchOrders : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ColorScheme = ColorScheme.Light,
            IgnoreHTTPSErrors = true
        };
    }

    [TearDown]
    public async Task TearDown()
    {
        await LogOutAsync();
    }
    
    private async Task LogOutAsync()
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
    public async Task TestSearchOrdersDefaultTable()
    {
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

        // check the value of Order ID
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0013 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0023 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0003 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0015 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0025 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0005 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0002 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0001 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0011 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0021 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0012 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0022 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0017 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0027 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0007 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0016 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0026 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0006 Air Quality Act" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestSearchOrdersSortTableByAscendingStatusDate()
    {
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

        // click on the Status/Date to filter in ascending order
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Status/Date ▼" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search?Status=All&Progress=All&WithAttachments=False&Sort=DateAsc&handler=search#search-results");

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

        // check the value of Order ID
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0013 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0023 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0003 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0015 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0025 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0005 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0002 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0001 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0011 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0021 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0012 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0022 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0017 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0027 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0007 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0016 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0026 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0006 Air Quality Act" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestSearchOrdersSortTableByDescendingStatusDate() {
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

        // click on the Status/Date to filter in descending order
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Status/Date ▼" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search?Status=All&Progress=All&WithAttachments=False&Sort=DateAsc&handler=search#search-results");
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Status/Date ▲" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search?Status=All&Progress=All&WithAttachments=False&Sort=DateDesc&handler=search#search-results");

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

        // check the value of Order ID
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0013 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0023 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0003 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0015 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0025 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0005 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0002 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0001 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0011 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0021 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0012 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0022 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0017 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0027 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0007 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0016 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0026 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0006 Air Quality Act" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestSearchOrdersSortTableByAscendingFacility() {
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

        // click on the Facility to sort ascendingly
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Facility" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search?Status=All&Progress=All&WithAttachments=False&Sort=FacilityAsc&handler=search#search-results");

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

        // check the value of Order ID
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0013 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0023 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0003 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0015 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0025 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0005 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0002 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0001 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0011 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0021 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0012 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0022 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0017 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0027 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0007 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0016 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0026 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0006 Air Quality Act" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestSearchOrdersSortTableByDescendingFacility() {
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

        // Click on the Facility to sort the orders descendingly
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Facility" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search?Status=All&Progress=All&WithAttachments=False&Sort=FacilityAsc&handler=search#search-results");
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Facility ▲" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search?Status=All&Progress=All&WithAttachments=False&Sort=FacilityDesc&handler=search#search-results");

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

        // check the value of Order ID
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0013 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0023 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-AQ-0003 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0015 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0025 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0005 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0002 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0001 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0011 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0021 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0012 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0022 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0017 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0027 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-WP-0007 Asbestos Safety Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0016 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0026 Air Quality Act" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = "EPD-SW-WQ-0006 Air Quality Act" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestSearchOrdersClearForm() {
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

        // clear form
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Clear Form" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search");

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(0));
    }

    [Test]
    public async Task TestSearchOrdersShowDeletedRecords() {
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
        
        // show deleted record checkbox
        await Page.GetByText("Show deleted records").ClickAsync();

        // search table with no values
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Search?Status=All&Progress=All&handler=search&ShowDeleted=true#search-results");

        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows in the first table
        int tableRows = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows, Is.GreaterThanOrEqualTo(3));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Expect(Page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0014");
        await Expect(Page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0024");
        await Expect(Page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0004");
    }


}
