using Enfo.Domain.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.App
{
    public static class SelectLists
    {
        public static SelectList CountiesSelectList => new(DomainData.Counties);
    }
}