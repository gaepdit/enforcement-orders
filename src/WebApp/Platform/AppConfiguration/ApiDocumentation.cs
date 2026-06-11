using Enfo.WebApp.Platform.Settings;
using Microsoft.OpenApi;

namespace Enfo.WebApp.Platform.AppConfiguration;

public static class ApiDocumentation
{
    private const string ApiVersion = "v3";
    private const string ApiTitle = "Georgia EPD Enforcement Orders API";

    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        services.AddMvcCore().AddApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.IgnoreObsoleteActions();
            c.SwaggerDoc(ApiVersion, new OpenApiInfo
            {
                Version = ApiVersion,
                Title = ApiTitle,
                Contact = new OpenApiContact
                {
                    Name = $"{ApiTitle} Support",
                    Email = AppSettings.Support.TechnicalSupportEmail,
                    Url = new Uri(AppSettings.Support.TechnicalSupportSite),
                },
            });
        });

        return services;
    }

    public static void UseApiDocumentation(this IApplicationBuilder app) => app
        .UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/openapi.json"; })
        .UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"{ApiVersion}/openapi.json", $"{ApiTitle} {ApiVersion}");
            options.RoutePrefix = "api-docs";
            options.DocumentTitle = ApiTitle;
        });
}
