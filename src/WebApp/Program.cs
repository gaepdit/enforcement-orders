using Enfo.Domain.LegalAuthorities.Resources.Validation;
using Enfo.WebApp.Platform.AppConfiguration;
using Enfo.WebApp.Platform.OrgNotifications;
using Enfo.WebApp.Platform.Settings;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Configure basic settings.
builder.BindAppSettings().AddHttpSecurity().AddErrorLogging();
builder.Services.AddDataProtection();

// Configure Identity stores.
builder.Services.AddIdentityStores();

// Configure authentication and authorization.
builder.ConfigureAuthentication();

// Configure UI services.
builder.Services.AddRazorPages();

// Add data stores and initialize the database.
await builder.ConfigureDataPersistence();

// Configure file/attachment storage.
await builder.ConfigureFileStorage();

// Add common services.
builder.Services
    .AddApiDocumentation()
    .AddWebOptimizer()
    .AddOrgNotifications()
    .AddValidatorsFromAssemblyContaining<LegalAuthorityValidator>();

// Build the application.
var app = builder.Build();

// Configure the application pipeline.
app
    .UseSecurityHeaders()
    .UseErrorHandling()
    .UseStatusCodePages()
    .UseHttpsRedirection()
    .UseWebOptimizer()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseApiDocumentation();

// Map endpoints.
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();
