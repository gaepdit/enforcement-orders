using System.Diagnostics;

namespace Enfo.WebApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[AllowAnonymous]
public class ErrorTestModel(ILogger<ErrorTestModel> logger) : PageModel
{
    public string RequestId { get; private set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public void OnGet() => RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

    public void OnGetHandledException()
    {
        const string message = "Testing handled exception.";
        Console.WriteLine(message);

        try
        {
            throw new NotImplementedException(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void OnGetUnhandledException()
    {
        const string message = "Testing unhandled exception.";
        Console.WriteLine(message);

        throw new NotImplementedException(message);
    }

    public void OnGetLoggedError()
    {
        const string message = "Testing logged error.";
        Console.WriteLine(message);

        logger.LogError(message);
    }

    public void OnGetLoggedException()
    {
        const string message = "Testing logged exception.";
        Console.WriteLine(message);

        try
        {
            throw new NotImplementedException(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            logger.LogError(e, message);
        }
    }
}
