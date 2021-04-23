using Enfo.Domain.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Platform
{
    public static class CommonSelectLists
    {
        public static SelectList CountiesSelectList => new(DomainData.Counties);
    }
}