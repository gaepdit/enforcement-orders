using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources.Validation;
using Enfo.Domain.Services;
using Enfo.Domain.Users.Entities;
using Enfo.Domain.Users.Services;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Enfo.Infrastructure.Services;
using Enfo.LocalRepository;
using Enfo.WebApp.Platform.Local;
using Enfo.WebApp.Platform.Migrator;
using Enfo.WebApp.Platform.Raygun;
using Enfo.WebApp.Platform.SecurityHeaders;
using Enfo.WebApp.Platform.Settings;
using FluentValidation;
using GaEpd.FileService;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Mindscape.Raygun4Net.AspNetCore;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Persist data protection keys
var directory =
    Directory.CreateDirectory(builder.Configuration["DataProtectionKeysPath"]!);
var dataProtectionBuilder = builder.Services.AddDataProtection().PersistKeysToFileSystem(directory);
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    dataProtectionBuilder.ProtectKeysWithDpapi(protectToLocalMachine: true);

// Bind Application Settings
builder.Configuration.GetSection(ApplicationSettings.RaygunSettingsSection)
    .Bind(ApplicationSettings.RaygunClientSettings);
builder.Configuration.GetSection(ApplicationSettings.LocalDevSettingsSection)
    .Bind(ApplicationSettings.LocalDevSettings);

// Configure Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<EnfoDbContext>();

// Configure authentication/authorization
var authenticationBuilder = builder.Services.AddAuthentication();

if (!builder.Environment.IsLocalEnv() || ApplicationSettings.LocalDevSettings.UseAzureAd)
{
    // When running on the server, requires an Azure AD login account (configured in the app settings file).
    authenticationBuilder.AddMicrosoftIdentityWebApp(builder.Configuration, cookieScheme: null);
    // Note: `cookieScheme: null` is mandatory. See https://github.com/AzureAD/microsoft-identity-web/issues/133#issuecomment-739550416
}

builder.Services.AddAuthorization();

// Configure UI
builder.Services.AddRazorPages();

// Add API documentation
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services
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
                Name = "Georgia EPD-IT Support",
                Email = builder.Configuration["SupportEmail"],
            },
        });
    });

// Configure HSTS (max age: two years)
builder.Services.AddHsts(opts => opts.MaxAge = TimeSpan.FromDays(730));

// Configure application monitoring
builder.Services.AddTransient<IErrorLogger, ErrorLogger>();
builder.Services.AddRaygun(builder.Configuration,
    new RaygunMiddlewareSettings { ClientProvider = new RaygunClientProvider() });
builder.Services.AddHttpContextAccessor(); // needed by RaygunScriptPartial

// Configure the attachments store
builder.Services.AddFileServices(builder.Configuration);
builder.Services.AddTransient<IAttachmentStore, AttachmentStore>();

// Configure the database contexts, data repositories, and services
if (builder.Environment.IsLocalEnv())
{
    // When running locally, you have the option to build the database using LocalDB or InMemory.
    // Either way, only the Identity tables are used by the application. The data tables are created,
    // but the data comes from the LocalRepository data files.
    if (ApplicationSettings.LocalDevSettings.BuildLocalDb)
    {
        builder.Services.AddDbContext<EnfoDbContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }
    else
    {
        builder.Services.AddDbContext<EnfoDbContext>(opts =>
            opts.UseInMemoryDatabase("Enfo_DB"));
    }

    // Uses static data when running locally
    builder.Services.AddScoped<IUserService, InMemoryUserService>();
    builder.Services.AddScoped<IEnforcementOrderRepository, LocalEnforcementOrderRepository>();
    builder.Services.AddScoped<IEpdContactRepository, LocalEpdContactRepository>();
    builder.Services.AddScoped<ILegalAuthorityRepository, LocalLegalAuthorityRepository>();
}
else
{
    // When running on the server, requires a deployed database (configured in the app settings file)
    builder.Services.AddDbContext<EnfoDbContext>(opts =>
        opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            options => options.MigrationsAssembly("Infrastructure")));

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IEnforcementOrderRepository, EnforcementOrderRepository>();
    builder.Services.AddScoped<IEpdContactRepository, EpdContactRepository>();
    builder.Services.AddScoped<ILegalAuthorityRepository, LegalAuthorityRepository>();
}

// Initialize database
builder.Services.AddHostedService<MigratorHostedService>();

// Configure validators
builder.Services.AddValidatorsFromAssemblyContaining<LegalAuthorityValidator>(); // Finds all validators

// Configure bundling and minification
builder.Services.AddWebOptimizer();

// Build the application
var app = builder.Build();
var env = app.Environment;

// Configure the HTTP request pipeline.
if (env.IsDevelopment() || env.IsLocalEnv())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Production or Staging
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseRaygun();
}

// Configure security HTTP headers
if (!env.IsLocalEnv() || ApplicationSettings.LocalDevSettings.UseSecurityHeadersLocally)
    app.UseSecurityHeaders(policies => policies.AddSecurityHeaderPolicies());

// Configure the application
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseWebOptimizer();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configure API documentation
app.UseSwagger(c => { c.RouteTemplate = "api-docs/{documentName}/openapi.json"; });
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api-docs/v3/openapi.json", "ENFO API v3");
    c.RoutePrefix = "api-docs";
    c.DocumentTitle = "Georgia EPD Enforcement Orders API";
});

// Map endpoints
app.MapRazorPages();
app.MapControllers();

// Make it so
app.Run();
