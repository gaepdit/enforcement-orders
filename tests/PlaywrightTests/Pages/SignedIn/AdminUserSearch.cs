using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests.Pages.SignedIn;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class AdminUserSearch : PageTest
{
    [SuppressMessage("SonarLint", "external_roslyn:NUnit1028")]
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
    public async Task TestGeneralUserSearch()
    {
        await Page.GotoAsync("https://localhost:44331/");

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Account/Login");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Index");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "More" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Users List" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Expect the following text in the home page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "ENFO User Search" })).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users?handler=search#search-results");
        
        //// Search with no parameter
        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));
        // check the number of rows
        int tableRows1 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(1));
        // check the column labels
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Name");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Email");
    }

    [Test]
    public async Task TestUserSearchFilterName()
    {
        await Page.GotoAsync("https://localhost:44331/");

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Account/Login");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Index");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "More" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Users List" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Expect the following text in the home page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "ENFO User Search" })).ToBeVisibleAsync();

        // enter the name
        await Page.GetByLabel("Name").ClickAsync();
        await Page.GetByLabel("Name").FillAsync("Local");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users?Name=Local&handler=search#search-results");
        
        //// Search with no parameter
        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));
        // check the number of rows
        int tableRows1 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(1));
        // check the column labels
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Name");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Email");
    }

    [Test]
    public async Task TestUserSearchFilterEmail()
    {
        await Page.GotoAsync("https://localhost:44331/");

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Account/Login");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Index");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "More" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Users List" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Expect the following text in the home page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "ENFO User Search" })).ToBeVisibleAsync();

        // enter the name
        await Page.GetByLabel("Email").ClickAsync();
        await Page.GetByLabel("Email").FillAsync("local.user@example.net");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users?Email=local.user%40example.net&handler=search#search-results");
        
        //// Search with no parameter
        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));
        // check the number of rows
        int tableRows1 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(1));
        // check the column labels
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Name");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Email");
    }

    [Test]
    public async Task TestUserSearchFilterRole()
    {
        await Page.GotoAsync("https://localhost:44331/");

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Account/Login");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Index");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "More" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Users List" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Expect the following text in the home page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "ENFO User Search" })).ToBeVisibleAsync();

        // choose the role
        await Page.GetByRole(AriaRole.Combobox, new() { NameString = "Role" }).SelectOptionAsync(new[] { "OrderAdministrator" });
        
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users?Role=OrderAdministrator&handler=search#search-results");

        //// Search with no parameter
        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));
        // check the number of rows
        int tableRows1 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(1));
        // check the column labels
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Name");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Email");
    }

    [Test]
    public async Task TestUserSearchClearForm()
    {
        await Page.GotoAsync("https://localhost:44331/");

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Account/Login");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Index");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "More" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Users List" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("EPD Enforcement Orders"));

        // Expect the following text in the home page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "ENFO User Search" })).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users?handler=search#search-results");
        
        //// Search with no parameter
        // check the number of tables
        int numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));
        // check the number of rows
        int tableRows1 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(1));
        // check the column labels
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Name");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Email");

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Clear Form" }).ClickAsync();
        await Page.WaitForURLAsync("https://localhost:44331/Admin/Users");

        // check the number of rows
        int tableRows2 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows2, Is.EqualTo(0));
    }
}
