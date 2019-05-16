# Enforcement Orders Application

Enforcement Orders (ENFO) is an online application to allow EPD staff to publish enforcement order notices online and to allow the public to view and search those orders.

## Background

The Georgia Environmental Protection Division is mandated by Rule 391-1-3-.01 to "issue notices of proposed or final administrative orders and proposed or final administrative consent orders." The Rule requires these notices to be posted on the Environmental Protection Division's Internet Web Site.

## General project requirements

Enforcement order notices are time-critical and high-profile public information. EPD staff must have complete control over the posting of the orders, and not require IT support to publish, modify, or remove them.
* EPD staff can input new orders, review and update existing orders, and remove orders.
* The public can view and search for proposed and executed orders.

# Info for developers

## Prerequisites

* [Visual Studio](https://www.visualstudio.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)
* [.NET Core 2.2 SDK](https://dotnet.microsoft.com/download)
* [Postman](https://www.getpostman.com/) for testing the API

## Project organization

The Visual Studio solution contains eight projects:

* Entities - A class library containing the data models and business logic
* DataAccess - A class library containing the database interaction
* API - ASP.NET Core Web API
* UI - ASP.NET Core Web App with Razor Pages
