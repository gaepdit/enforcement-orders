using System.Diagnostics.CodeAnalysis;

namespace PlaywrightTests;

[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeNotEvident")]
public static class PlaywrightHelpers
{
    public static BrowserNewContextOptions DefaultContextOptions() =>
        new()
        {
            BaseURL = "https://localhost:44331",
            IgnoreHTTPSErrors = true,
        };

    public static async Task LogOutAsync(IPage page)
    {
        await page.GotoAsync("/");

        // The account is not signed in when there is no Account button
        if (await page.GetByRole(AriaRole.Button, new() { NameString = "Account" }).CountAsync() == 0) return;

        // Sign out
        await page.GetByRole(AriaRole.Button, new() { NameString = "Account" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { NameString = "Sign out" }).ClickAsync();
        await page.WaitForURLAsync("/");
    }

    public static async Task SignInAsync(IPage page)
    {
        await page.GotoAsync("/Account/Login");

        // The account is already signed in when there is an Account button
        if (await page.GetByRole(AriaRole.Button, new() { NameString = "Account" }).CountAsync() == 1) return;

        // Sign in
        await page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await page.WaitForURLAsync("/Admin/Index");
    }
}
