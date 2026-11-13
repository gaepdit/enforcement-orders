namespace Enfo.WebApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[AllowAnonymous]
public class ErrorModel(IAuthorizationService authorization) : PageModel
{
    public int? Status { get; private set; }
    public bool ActiveUser { get; private set; }

    public void OnGet(int? statusCode)
    {
        ActiveUser = User.Identity?.IsAuthenticated ?? false;
        Status = statusCode;
    }
}
