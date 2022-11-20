using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests.Pages.SignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class AdminSearchOrders : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ColorScheme = ColorScheme.Light,
            IgnoreHTTPSErrors = true
        };
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
        Assert.That(tableRows, Is.EqualTo(19));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Expect(Page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-AQ-0013");
        await Expect(Page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-AQ-0023");
        await Expect(Page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-AQ-0003");
        await Expect(Page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Expect(Page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Expect(Page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Expect(Page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Expect(Page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Expect(Page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Expect(Page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Expect(Page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Expect(Page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Expect(Page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("EPD-WP-0017");
        await Expect(Page.Locator("//table/tbody/tr[14]/td[1]")).ToContainTextAsync("EPD-WP-0027");
        await Expect(Page.Locator("//table/tbody/tr[15]/td[1]")).ToContainTextAsync("EPD-WP-0007");
        await Expect(Page.Locator("//table/tbody/tr[16]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Expect(Page.Locator("//table/tbody/tr[17]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Expect(Page.Locator("//table/tbody/tr[18]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
        await Expect(Page.Locator("//table/tbody/tr[19]/td[1]")).ToContainTextAsync("");
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
        Assert.That(tableRows, Is.EqualTo(19));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Expect(Page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("");
        await Expect(Page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Expect(Page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Expect(Page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
        await Expect(Page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-WP-0017");
        await Expect(Page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-WP-0027");
        await Expect(Page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("EPD-WP-0007");
        await Expect(Page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Expect(Page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Expect(Page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Expect(Page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Expect(Page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Expect(Page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Expect(Page.Locator("//table/tbody/tr[14]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Expect(Page.Locator("//table/tbody/tr[15]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Expect(Page.Locator("//table/tbody/tr[16]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Expect(Page.Locator("//table/tbody/tr[17]/td[1]")).ToContainTextAsync("EPD-AQ-0013");
        await Expect(Page.Locator("//table/tbody/tr[18]/td[1]")).ToContainTextAsync("EPD-AQ-0023");
        await Expect(Page.Locator("//table/tbody/tr[19]/td[1]")).ToContainTextAsync("EPD-AQ-0003");
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
        Assert.That(tableRows, Is.EqualTo(19));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Expect(Page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-AQ-0013");
        await Expect(Page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-AQ-0023");
        await Expect(Page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-AQ-0003");
        await Expect(Page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Expect(Page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Expect(Page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Expect(Page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Expect(Page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Expect(Page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Expect(Page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Expect(Page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Expect(Page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Expect(Page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("EPD-WP-0017");
        await Expect(Page.Locator("//table/tbody/tr[14]/td[1]")).ToContainTextAsync("EPD-WP-0027");
        await Expect(Page.Locator("//table/tbody/tr[15]/td[1]")).ToContainTextAsync("EPD-WP-0007");
        await Expect(Page.Locator("//table/tbody/tr[16]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Expect(Page.Locator("//table/tbody/tr[17]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Expect(Page.Locator("//table/tbody/tr[18]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
        await Expect(Page.Locator("//table/tbody/tr[19]/td[1]")).ToContainTextAsync("");
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
        Assert.That(tableRows, Is.EqualTo(19));

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
        await Expect(Page.Locator("//table/tbody/tr[14]/td[1]")).ToContainTextAsync("EPD-WP-0017");
        await Expect(Page.Locator("//table/tbody/tr[15]/td[1]")).ToContainTextAsync("EPD-WP-0027");
        await Expect(Page.Locator("//table/tbody/tr[16]/td[1]")).ToContainTextAsync("EPD-WP-0007");
        await Expect(Page.Locator("//table/tbody/tr[17]/td[1]")).ToContainTextAsync("EPD-AQ-0013");
        await Expect(Page.Locator("//table/tbody/tr[18]/td[1]")).ToContainTextAsync("EPD-AQ-0023");
        await Expect(Page.Locator("//table/tbody/tr[19]/td[1]")).ToContainTextAsync("EPD-AQ-0003");
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
        Assert.That(tableRows, Is.EqualTo(19));

        // check the column labels of the first table
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Order");
        await Expect(Page.Locator("//table/thead/tr/th[3]")).ToContainTextAsync("Status/Date");

        // check the value of Order ID in the current order
        await Expect(Page.Locator("//table/tbody/tr[19]/td[1]")).ToContainTextAsync("EPD-WP-0001");
        await Expect(Page.Locator("//table/tbody/tr[18]/td[1]")).ToContainTextAsync("EPD-WP-0011");
        await Expect(Page.Locator("//table/tbody/tr[17]/td[1]")).ToContainTextAsync("EPD-WP-0021");
        await Expect(Page.Locator("//table/tbody/tr[16]/td[1]")).ToContainTextAsync("EPD-WP-0012");
        await Expect(Page.Locator("//table/tbody/tr[15]/td[1]")).ToContainTextAsync("EPD-WP-0002");
        await Expect(Page.Locator("//table/tbody/tr[14]/td[1]")).ToContainTextAsync("EPD-WP-0022");
        await Expect(Page.Locator("//table/tbody/tr[13]/td[1]")).ToContainTextAsync("");
        await Expect(Page.Locator("//table/tbody/tr[12]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0015");
        await Expect(Page.Locator("//table/tbody/tr[11]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0025");
        await Expect(Page.Locator("//table/tbody/tr[10]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0005");
        await Expect(Page.Locator("//table/tbody/tr[9]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0016");
        await Expect(Page.Locator("//table/tbody/tr[8]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0026");
        await Expect(Page.Locator("//table/tbody/tr[7]/td[1]")).ToContainTextAsync("EPD-SW-WQ-0006");
        await Expect(Page.Locator("//table/tbody/tr[6]/td[1]")).ToContainTextAsync("EPD-WP-0017");
        await Expect(Page.Locator("//table/tbody/tr[5]/td[1]")).ToContainTextAsync("EPD-WP-0027");
        await Expect(Page.Locator("//table/tbody/tr[4]/td[1]")).ToContainTextAsync("EPD-WP-0007");
        await Expect(Page.Locator("//table/tbody/tr[3]/td[1]")).ToContainTextAsync("EPD-AQ-0013");
        await Expect(Page.Locator("//table/tbody/tr[2]/td[1]")).ToContainTextAsync("EPD-AQ-0023");
        await Expect(Page.Locator("//table/tbody/tr[1]/td[1]")).ToContainTextAsync("EPD-AQ-0003");
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
        Assert.That(tableRows, Is.EqualTo(3));

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
