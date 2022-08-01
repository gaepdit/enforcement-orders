# Enforcement Orders Application

Enforcement Orders (ENFO) is an online application to allow EPD staff to publish enforcement order notices online and to allow the public to view and search those orders.

[![.NET Test](https://github.com/gaepdit/enforcement-orders/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gaepdit/enforcement-orders/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/gaepdit/enforcement-orders/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/enforcement-orders/actions/workflows/codeql-analysis.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_enforcement-orders&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_enforcement-orders)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_enforcement-orders&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_enforcement-orders)

## Background

The Georgia Environmental Protection Division is mandated by Rule 391-1-3-.01 to "issue notices of proposed or final administrative orders and proposed or final administrative consent orders." The Rule requires these notices to be posted on the Environmental Protection Division's Internet Web Site.

## General project requirements

Enforcement order notices are time-critical and high-profile public information. EPD staff must have complete control over the posting of the orders, and not require IT support to publish, modify, or remove them.
* EPD staff can input new orders, review and update existing orders, and remove orders.
* The public can view and search for proposed and executed orders.

# Info for developers

ENFO is written using .NET 6.

## Prerequisites

* [Visual Studio](https://www.visualstudio.com/vs/), [VS Code](https://code.visualstudio.com/), or equivalent IDE.

* Either the [pnpm](https://pnpm.io/) (recommended) or [npm](https://www.npmjs.com/) package manager.

## Project organization

The solution contains three projects:

* **Domain** — A class library containing the data models and business logic
* **LocalRepository** — A class library implementing the Domain & Repository with test data
* **Infrastructure** — A class library implementing the Domain & Repository using Entity Framework
* **Enfo.WebApp** — The front end web application written in ASP.NET Razor Pages.

There are also corresponding unit test projects.

## Setup

In the web app folder, run `pnpm install` (or `npm install`) to install the dependencies.

There are two launch profiles:

* **WebApp Local** — This profile uses the LocalRepository project for repository implementation and the TestData project for initial seed data. A local test user account is used for authentication. No external dependencies are required. 

    You can modify these settings in an "appsettings.Local.json" file to test various scenarios:

    - *AuthenticatedUser* — Set to `true` to successfully authenticate with a test account. Set to `false` to simulate a failed login. (Either way, only the Identity tables are used by the application. The application data tables are created, but application data comes from the TestData project.)
    - *BuildLocalDb* — Uses LocalDB when `true` or an in memory DB when `false`.
    - *UseLocalFileSystem* — If `true`, attachment files are saved/loaded from the file system. If `false`, files are seeded from the TestData project and stored in memory.
    - *UseSecurityHeadersLocally* — Sets whether to include HTTP security headers (when running locally).

* **WebApp Dev Server** — This profile uses the Infrastructure project for repository implementation and accesses the remote Dev database server for data. In order to use this profile, copy the "appsettings.Development.json" file from the app config repository. VPN or internal network access is required to connect to the database. An SOG account is required for authentication.
