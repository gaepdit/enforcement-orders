using Enfo.WebApp.Platform.Settings;
using Microsoft.OpenApi.Models;

namespace Enfo.WebApp.Platform.AppConfiguration;

public static class ApiDocumentation
{
    public static void AddApiDocumentation(this IServiceCollection services)
    {
        services.AddMvcCore().AddApiExplorer();
        services
            .AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.IgnoreObsoleteActions();
                c.SwaggerDoc("v3", new OpenApiInfo
                {
                    Version = "v3",
                    Title = "Georgia EPD Enforcement Orders API",
                    Contact = new OpenApiContact
                    {
                        Name = "Enforcement Orders Application Technical Support",
                        Email = AppSettings.SupportSettings.TechnicalSupportEmail,
                    },
                });
            });
    }

    public static void UseApiDocumentation(this IApplicationBuilder app) => app
        .UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/openapi.json"; })
        .UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/api-docs/v3/openapi.json", "ENFO API v3");
            options.RoutePrefix = "api-docs";
            options.DocumentTitle = "Georgia EPD Enforcement Orders API";
        });
}
