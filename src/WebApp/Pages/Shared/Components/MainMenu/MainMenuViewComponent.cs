using Enfo.Domain.Identity;
using Enfo.WebApp.Pages.Account;

namespace Enfo.WebApp.Pages.Shared.Components.MainMenu;

public class MainMenuViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(PageModel model) =>
        View("Default", new MenuParams
        {
            IsLoginPage = model is LoginModel,
            IsSignedIn = User.Identity is { IsAuthenticated: true },
            IsOrderAdministrator =
                User.Identity is { IsAuthenticated: true } && User.IsInRole(AppRole.OrderAdministrator),
        });

    public record MenuParams
    {
        public bool IsLoginPage { get; init; }
        public bool IsSignedIn { get; init; }
        public bool IsOrderAdministrator { get; init; }
    }
}
