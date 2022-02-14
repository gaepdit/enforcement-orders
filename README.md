# Enforcement Orders Application

Enforcement Orders (ENFO) is an online application to allow EPD staff to publish enforcement order notices online and to allow the public to view and search those orders.

[![.NET build and test](https://github.com/gaepdit/enforcement-orders/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gaepdit/enforcement-orders/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/gaepdit/enforcement-orders/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/enforcement-orders/actions/workflows/codeql-analysis.yml)
[![Raygun Deployment Tracking](https://github.com/gaepdit/enforcement-orders/actions/workflows/raygun-deployment.yml/badge.svg)](https://github.com/gaepdit/enforcement-orders/actions/workflows/raygun-deployment.yml)

## Background

The Georgia Environmental Protection Division is mandated by Rule 391-1-3-.01 to "issue notices of proposed or final administrative orders and proposed or final administrative consent orders." The Rule requires these notices to be posted on the Environmental Protection Division's Internet Web Site.

## General project requirements

Enforcement order notices are time-critical and high-profile public information. EPD staff must have complete control over the posting of the orders, and not require IT support to publish, modify, or remove them.
* EPD staff can input new orders, review and update existing orders, and remove orders.
* The public can view and search for proposed and executed orders.

# Info for developers

ENFO is written using .NET 5.

## Prerequisites

* [Visual Studio](https://www.visualstudio.com/vs/), [VS Code](https://code.visualstudio.com/), or equivalent IDE.

* [pnpm](https://pnpm.io/) or [npm](https://www.npmjs.com/) package manager.

## Project organization

The solution contains three projects:

* Enfo.Domain - A class library containing the data models and business logic
* Enfo.Infrastructure - A class library implementing the Domain & Repository using Entity Framework
* Enfo.WebApp - Front end web application

There are also three corresponding unit test projects.
