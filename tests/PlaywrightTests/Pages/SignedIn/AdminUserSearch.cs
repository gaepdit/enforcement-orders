using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PlaywrightTests.Pages.SignedIn;

[Parallelizable(ParallelScope.None)]
[TestFixture]
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
public class AdminUserSearch : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions() => PlaywrightHelpers.DefaultContextOptions();

    [SetUp]
    public async Task SetUp() => await PlaywrightHelpers.SignInAsync(Page);

    [TearDown]
    public async Task TearDown() => await PlaywrightHelpers.LogOutAsync(Page);

    [Test]
    public async Task TestGeneralUserSearch()
    {
        await Page.GetByRole(AriaRole.Button, new() { NameString = "More" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Users List" }).ClickAsync();
        await Page.WaitForURLAsync("/Admin/Users");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("User Search"));

        // Expect the following text in the home page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "ENFO User Search" })).ToBeVisibleAsync();

        // Search with no parameter
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("**/Admin/Users?handler=search#search-results");

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows
        var tableRows1 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(1));

        // check the column labels
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Name");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Email");
    }

    [Test]
    public async Task TestUserSearchFilterName()
    {
        await Page.GotoAsync("/Admin/Users");

        // enter the name
        await Page.GetByLabel("Name").ClickAsync();
        await Page.GetByLabel("Name").FillAsync("Local");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("**/Admin/Users?Name=Local&handler=search#search-results");

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows
        var tableRows1 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(1));

        // check the column labels
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Name");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Email");
    }

    [Test]
    public async Task TestUserSearchFilterEmail()
    {
        await Page.GotoAsync("/Admin/Users");

        // enter the email
        await Page.GetByLabel("Email").ClickAsync();
        await Page.GetByLabel("Email").FillAsync("local.user@example.net");

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("**/Admin/Users?Email=local.user%40example.net&handler=search#search-results");

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows
        var tableRows1 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(1));

        // check the column labels
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Name");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Email");
    }

    [Test]
    public async Task TestUserSearchFilterRole()
    {
        await Page.GotoAsync("/Admin/Users");

        // choose the role
        await Page.GetByRole(AriaRole.Combobox, new() { NameString = "Role" })
            .SelectOptionAsync(new[] { "OrderAdministrator" });

        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("**/Admin/Users?Role=OrderAdministrator&handler=search#search-results");

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        // check the number of rows
        var tableRows1 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.EqualTo(1));

        // check the column labels
        await Expect(Page.Locator("//table/thead/tr/th[1]")).ToContainTextAsync("Name");
        await Expect(Page.Locator("//table/thead/tr/th[2]")).ToContainTextAsync("Email");
    }

    [Test]
    public async Task TestUserSearchClearForm()
    {
        await Page.GotoAsync("/Admin/Users?handler=search");

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(1));

        await Page.GetByRole(AriaRole.Link, new() { NameString = "Clear Form" }).ClickAsync();
        await Page.WaitForURLAsync("/Admin/Users");

        // check the number of rows
        var tableRows2 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows2, Is.EqualTo(0));
    }
}
