using Enfo.Domain.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Platform.RazorHelpers
{
    public static class CommonSelectLists
    {
        public static SelectList CountiesSelectList => new(DomainData.Counties);
    }
}