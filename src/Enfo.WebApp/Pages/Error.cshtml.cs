using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class Error : PageModel
{
    public string RequestId { get; private set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    [UsedImplicitly]
    public void OnGet() => RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
}
