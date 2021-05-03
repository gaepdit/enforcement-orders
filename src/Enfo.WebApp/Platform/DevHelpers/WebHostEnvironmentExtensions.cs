using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Enfo.WebApp.Platform.DevHelpers
{
    public static class WebHostEnvironmentExtensions
    {
        public static bool IsLocalDev(this IWebHostEnvironment environment) => 
            environment.IsEnvironment("Local");
    }
}