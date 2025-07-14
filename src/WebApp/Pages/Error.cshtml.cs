using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace Enfo.WebApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[AllowAnonymous]
public class ErrorModel(ILogger<ErrorModel> logger) : PageModel
{
    public string RequestId { get; private set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public void OnGet() => RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

    public void OnGetHandledError()
    {
        try
        {
            throw new NotImplementedException("Testing handled exception.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void OnGetUnhandledError()
    {
        throw new NotImplementedException("Testing unhandled exception.");
    }

    public void OnGetLoggedError()
    {
        logger.LogError("Testing logged error.");
    }
}
