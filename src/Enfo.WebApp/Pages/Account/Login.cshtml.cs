using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Account
{
    [AllowAnonymous]
    public class Login : PageModel
    {
        public string ReturnUrl { get; private set; }
        public DisplayMessage Message { get; private set; }

        [UsedImplicitly]
        public IActionResult OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? "/Admin/Index";

            if (User.Identity?.IsAuthenticated ?? false)
            {
                return LocalRedirect(ReturnUrl);
            }

            Message = TempData?.GetDisplayMessage();
            return Page();
        }
    }
}