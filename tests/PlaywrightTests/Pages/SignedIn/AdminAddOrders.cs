using System.Diagnostics.CodeAnalysis;

namespace PlaywrightTests.Pages.SignedIn;

[Parallelizable(ParallelScope.None)]
[TestFixture]
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
public class AdminAddOrders : PageTest
{
    [SuppressMessage("Structure", "NUnit1028:The non-test method is public")]
    public override BrowserNewContextOptions ContextOptions() => PlaywrightHelpers.DefaultContextOptions();

    [SetUp]
    public async Task SetUp() => await PlaywrightHelpers.SignInAsync(Page);

    [TearDown]
    public async Task TearDown() => await PlaywrightHelpers.LogOutAsync(Page);

    [Test]
    public async Task TestAddOrders()
    {
        // click on add new order button
        await Page.GetByRole(AriaRole.Link, new() { NameString = "+ New Order" }).ClickAsync();
        await Page.WaitForURLAsync("/Admin/Add");

        // enter the facility value
        await Page.GetByLabel("Facility *").ClickAsync();
        await Page.GetByLabel("Facility *").FillAsync("testing");

        // select county
        await Page.GetByRole(AriaRole.Combobox, new() { NameString = "County *" })
            .SelectOptionAsync(new[] { "Bartow" });

        // enter cause of order
        await Page.GetByLabel("Cause of Order *").ClickAsync();
        await Page.GetByLabel("Cause of Order *").FillAsync("testing");

        // enter legal authority
        await Page.GetByRole(AriaRole.Combobox, new() { NameString = "Legal Authority *" })
            .SelectOptionAsync(new[] { "1" });

        // Create unique number for order
        var orderNumber = DateTime.Now.Ticks.ToString();

        // enter order number
        await Page.GetByLabel("Order Number *").ClickAsync();
        await Page.GetByLabel("Order Number *").FillAsync(orderNumber);

        // enter settlement amount
        await Page.GetByLabel("Settlement Amount").ClickAsync();
        await Page.GetByLabel("Settlement Amount").FillAsync("123");

        // select progress
        await Page.GetByRole(AriaRole.Combobox, new() { NameString = "Progress *" }).SelectOptionAsync(new[] { "0" });

        // click button to add the new order
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Add New Order" }).ClickAsync();
        await Page.WaitForURLAsync("/Admin/Details/*");

        // Expect the following text after the order has been added
        await Expect(Page.GetByRole(AriaRole.Heading,
            new() { NameString = $"Admin View: Enforcement Order {orderNumber}" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("This Order is not publicly viewable.")).ToBeVisibleAsync();

        // check the number of tables in the new order detail page
        var numTables2 = await Page.Locator("//table").CountAsync();
        Assert.That(numTables2, Is.EqualTo(1));

        // check the number of rows
        var tableRows2 = await Page.Locator("//table/tbody/tr").CountAsync();
        Assert.That(tableRows2, Is.EqualTo(12));

        // check the column labels
        await Expect(Page.Locator("//table/tbody/tr[1]/th[1]")).ToContainTextAsync("Progress");
        await Expect(Page.Locator("//table/tbody/tr[2]/th[1]")).ToContainTextAsync("Facility");
        await Expect(Page.Locator("//table/tbody/tr[3]/th[1]")).ToContainTextAsync("County");
        await Expect(Page.Locator("//table/tbody/tr[4]/th[1]")).ToContainTextAsync("Cause of Order");
        await Expect(Page.Locator("//table/tbody/tr[5]/th[1]")).ToContainTextAsync("Requirements of Order");
        await Expect(Page.Locator("//table/tbody/tr[6]/th[1]")).ToContainTextAsync("Proposed Settlement Amount");
        await Expect(Page.Locator("//table/tbody/tr[7]/th[1]")).ToContainTextAsync("Legal Authority");
        await Expect(Page.Locator("//table/tbody/tr[8]/th[1]"))
            .ToContainTextAsync("Publication Date For Proposed Order");
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

        // go back and search for the order
        await Page.GetByRole(AriaRole.Link, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync("/Admin/Search");
        await Page.GetByLabel("Order Number").FillAsync(orderNumber);
        await Page.GetByRole(AriaRole.Button, new() { NameString = "Search" }).ClickAsync();
        await Page.WaitForURLAsync(
            $"**/Admin/Search?OrderNumber={orderNumber}&Status=All&Progress=All&handler=search#search-results");

        // check the number of rows in the first table
        var tableRows3 = await Page.Locator("//table[1]/tbody/tr").CountAsync();
        Assert.That(tableRows3, Is.EqualTo(1));

        // check if the value exists in the table
        await Expect(Page.GetByRole(AriaRole.Rowheader, new() { NameString = "testing Bartow County" }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Cell, new() { NameString = $"{orderNumber} Air Quality Act" }))
            .ToBeVisibleAsync();
    }
}
