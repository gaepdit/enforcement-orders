using System.Diagnostics.CodeAnalysis;

namespace PlaywrightTests.Pages.SignedIn;

[Parallelizable(ParallelScope.None)]
[TestFixture]
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
public class AdminIndexPage : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions() => PlaywrightHelpers.DefaultContextOptions();

    [SetUp]
    public async Task SetUp() => await PlaywrightHelpers.SignInAsync(Page);

    [TearDown]
    public async Task TearDown() => await PlaywrightHelpers.LogOutAsync(Page);

    [Test]
    public async Task TestTextAdminIndexPage()
    {
        await Page.GotoAsync("/Admin/Index");

        // check dashboard menus
        await Expect(Page.Locator("nav[role=\"navigation\"]:has-text(\"+ New Order Search More\")")).ToBeVisibleAsync();

        // check the text in the page
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Admin Dashboard" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Quick Search" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Currently Published Orders" }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByText("These Enforcement Orders are currently displayed on the Public Website. Date of"))
            .ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Proposed Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Executed Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Unpublished Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Pending Orders" })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { NameString = "Draft Orders" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task TestTableAdminIndexPage()
    {
        await Page.GotoAsync("/Admin/Index");

        // check the number of tables
        var numTables = await Page.Locator("//table").CountAsync();
        Assert.That(numTables, Is.EqualTo(4));

        // Proposed Orders
        var tableRows1 = await Page.Locator("//section[1]/div/section[1]/table/tbody/tr").CountAsync();
        Assert.That(tableRows1, Is.GreaterThanOrEqualTo(3));
        // check the column labels of the first table
        await Expect(Page.Locator("//section[1]/div/section[1]/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[1]/div/section[1]/table/thead/tr/th[2]"))
            .ToContainTextAsync("Date Comment Period Closes");
        await Expect(Page.Locator("//section[1]/div/section[1]/table/thead/tr/th[3]")).ToContainTextAsync("");

        // Executed Orders
        var tableRows2 = await Page.Locator("//section[1]/div/section[2]/table/tbody/tr").CountAsync();
        Assert.That(tableRows2, Is.GreaterThanOrEqualTo(3));
        // check the column labels of the second table
        await Expect(Page.Locator("//section[1]/div/section[2]/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[1]/div/section[2]/table/thead/tr/th[2]"))
            .ToContainTextAsync("Date Executed");
        await Expect(Page.Locator("//section[1]/div/section[2]/table/thead/tr/th[3]")).ToContainTextAsync("");

        // Pending Orders
        var tableRows3 = await Page.Locator("//section[2]/div/section[1]/table/tbody/tr").CountAsync();
        Assert.That(tableRows3, Is.GreaterThanOrEqualTo(3));
        // check the column labels of the third table
        await Expect(Page.Locator("//section[2]/div/section[1]/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[2]/div/section[1]/table/thead/tr/th[2]"))
            .ToContainTextAsync("Pending Publication Date");
        await Expect(Page.Locator("//section[2]/div/section[1]/table/thead/tr/th[3]")).ToContainTextAsync("");

        // Draft Orders
        var tableRows4 = await Page.Locator("//section[2]/div/section[2]/table/tbody/tr").CountAsync();
        Assert.That(tableRows4, Is.GreaterThanOrEqualTo(3));
        // check the column labels of the fourth table 
        await Expect(Page.Locator("//section[2]/div/section[2]/table/thead/tr/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//section[2]/div/section[2]/table/thead/tr/th[2]")).ToContainTextAsync("");
    }
}
