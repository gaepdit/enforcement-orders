dotnet tool install --global PowerShell
dotnet add package Microsoft.Playwright.NUnit
dotnet build
playwright install

dotnet test -- PlayWright.LaunchOptions.Headless=false
dotnet test -- Playwright.LaunchOptions.Headless=false Playwright.LaunchOptions.SlowMo=1000