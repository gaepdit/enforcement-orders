# Enforcement Orders Application

Enforcement Orders (ENFO) is an online application to allow EPD staff to publish enforcement order notices online and to allow the public to view and search those orders.

[![.NET Test](https://github.com/gaepdit/enforcement-orders/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gaepdit/enforcement-orders/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/gaepdit/enforcement-orders/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/enforcement-orders/actions/workflows/codeql-analysis.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_enforcement-orders&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_enforcement-orders)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_enforcement-orders&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_enforcement-orders)

## Background

The Georgia Environmental Protection Division is mandated by Rule 391-1-3-.01 to "issue notices of proposed or final administrative orders and proposed or final administrative consent orders." The Rule requires these notices to be posted on the Environmental Protection Division's Internet Website.

### General project requirements

Enforcement order notices are time-critical and high-profile public information. EPD staff must have complete control over the posting of the orders, and not require IT support to publish, modify, or remove them.

* EPD staff can input new orders, review and update existing orders, and remove orders.
* The public can view and search for proposed and executed orders.

## Info for developers

ENFO is written using .NET 6.

### Prerequisites

+ [Visual Studio](https://www.visualstudio.com/vs/) or similar
+ [.NET 6.0 SDK](https://dotnet.microsoft.com/download)
+ [NPM](https://www.npmjs.com/) or [PNPM](https://pnpm.io/)

### Project organization

The solution contains the following projects:

* **Domain** — A class library containing the data models and business logic.
* **LocalRepository** — A class library implementing the Domain & Repository with test data.
* **Infrastructure** — A class library implementing the Domain & Repository using Entity Framework.
* **Enfo.WebApp** — The front end web application written in ASP.NET Razor Pages.

There are also corresponding unit test projects for each, plus a **TestData** project containing test data for development/testing purposes.

### Launch profiles

There are two launch profiles:

* **WebApp Local** — This profile uses data in the "tests/TestData" project and does not connect to a remote database. A local user account can be used to simulate authentication, or an Azure AD account can be configured.

    You can modify some development settings by creating an "appsettings.Local.json" file to test various scenarios:

    - *AuthenticatedUser* — Set to `true` to simulate a successful login with the test account. Set to `false` to simulate a failed login.
    - *BuildLocalDb* — Uses LocalDB when `true` or in-memory data when `false`.
    - *UseLocalFileSystem* — If `true`, attachment files are saved/loaded from the file system. If `false`, files are seeded from the TestData project and stored in memory.
    - *UseSecurityHeadersLocally* — Sets whether to include HTTP security headers (when running locally).
    - *UseAzureAd* — If `true`, the app must be registered in the Azure portal, and configuration settings added in the "AzureAd" settings section. If `false`, authentication is simulated using test user data. (*Note:* `UseAzureAd` is not compatible with in-memory data. If `BuildLocalDb` is `false`, then `UseAzureAd` must be `false`.)

* **WebApp Dev Server** — This profile connects to the remote Dev database server for data and requires an SOG account to log in. *To use this profile, you must add the "appsettings.Development.json" file from the "app-config" repo.*

Most development should be done using the Local profile. The Dev Server profile is only needed when specifically troubleshooting issues with the database server or SOG account.

### Entity Framework database migrations

Instructions for adding a new Entity Framework database migration:

1. Open a command prompt to "./src/Infrastructure/" folder.

2. Run the following command with an appropriate migration name:

   `dotnet ef migrations add NAME_OF_MIGRATION --msbuildprojectextensionspath ..\..\.artifacts\Infrastructure\obj\`
