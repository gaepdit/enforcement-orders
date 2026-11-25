namespace Enfo.WebApp.Pages;

[AllowAnonymous]
public class Support : PageModel
{
    public bool ActiveUser { get; private set; }

    public void OnGet() => ActiveUser = User.Identity?.IsAuthenticated ?? false;
}
