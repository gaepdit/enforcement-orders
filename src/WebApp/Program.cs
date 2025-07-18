using Enfo.Domain.LegalAuthorities.Resources.Validation;
using Enfo.WebApp.Platform.AppConfiguration;
using Enfo.WebApp.Platform.OrgNotifications;
using Enfo.WebApp.Platform.Settings;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Set default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices-regex#use-time-out-values
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Bind application settings.
builder.BindAppSettings();
builder.AddErrorLogging();

// Persist data protection keys
builder.Services.AddDataProtection();

// Configure Identity stores.
builder.Services.AddIdentityStores();

// Configure authentication and authorization.
builder.ConfigureAuthentication();

// Configure UI services.
builder.Services.AddRazorPages();

if (!builder.Environment.IsDevelopment())
{
    builder.Services
        .AddHsts(options => options.MaxAge = TimeSpan.FromDays(360))
        .AddHttpsRedirection(options => options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect);
}

// Add data stores and initialize the database.
await builder.ConfigureDataPersistence();

// Configure file/attachment storage.
await builder.ConfigureFileStorage();

// Add organizational notifications.
builder.Services.AddOrgNotifications();

// Add API documentation.
builder.Services.AddApiDocumentation();

// Configure validators.
builder.Services.AddValidatorsFromAssemblyContaining<LegalAuthorityValidator>(); // Finds all validators

// Configure bundling and minification.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddWebOptimizer(
        minifyJavaScript: AppSettings.DevSettings.EnableWebOptimizerInDev,
        minifyCss: AppSettings.DevSettings.EnableWebOptimizerInDev);
}
else
{
    builder.Services.AddWebOptimizer();
}

// Build the application.
var app = builder.Build();

// Configure security HTTP headers
if (!app.Environment.IsDevelopment() || AppSettings.DevSettings.UseSecurityHeadersInDev)
{
    app.UseHsts();
    app.UseSecurityHeaders(policyCollection => policyCollection.AddSecurityHeaderPolicies());
}

// Configure the application pipeline.
app
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
