using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Resources.Validation;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.LegalAuthorities.Resources;
using Enfo.Domain.LegalAuthorities.Resources.Validation;
using Enfo.Domain.Users.Entities;
using Enfo.Domain.Users.Services;
using Enfo.Infrastructure.Contexts;
using Enfo.LocalRepository.EnforcementOrders;
using Enfo.LocalRepository.EpdContacts;
using Enfo.LocalRepository.LegalAuthorities;
using Enfo.LocalRepository.Users;
using Enfo.WebApp.Platform.Local;
using Enfo.WebApp.Platform.Raygun;
using Enfo.WebApp.Platform.SecurityHeaders;
using Enfo.WebApp.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mindscape.Raygun4Net.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<EnfoDbContext>();

// Configure cookies
// SameSiteMode.None is needed to get single sign-out to work
builder.Services.Configure<CookiePolicyOptions>(opts => opts.MinimumSameSitePolicy = SameSiteMode.None);

// Configure authentication
if (builder.Environment.IsLocalEnv())
{
    builder.Services.AddAuthentication();
}
else
{
    // When running on the server, requires an Azure AD login account (configured in the app settings file).
    // (AddAzureAD is marked as obsolete and will be removed in a future version, but
    // the replacement, Microsoft Identity Web, is net yet compatible with RoleManager.)
    // Follow along at https://github.com/AzureAD/microsoft-identity-web/issues/1091
#pragma warning disable 618
    builder.Services
        .AddAuthentication()
        .AddAzureAD(opts =>
        {
            builder.Configuration.Bind(AzureADDefaults.AuthenticationScheme, opts);
            opts.CallbackPath = "/signin-oidc";
            opts.CookieSchemeName = "Identity.External";
        });
    builder.Services
        .Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, opts =>
        {
            opts.Authority += "/v2.0/";
            opts.TokenValidationParameters.ValidateIssuer = true;
            opts.UsePkce = true;
        });
#pragma warning restore 618
}

// Persist data protection keys
var keysFolder = Path.Combine(builder.Configuration["PersistedFilesBasePath"], "DataProtectionKeys");
builder.Services.AddDataProtection().PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));
builder.Services.AddAuthorization();

// Configure UI
builder.Services.AddRazorPages().AddFluentValidation();

// Add API documentation
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services
    .AddSwaggerGen(c =>
    {
        c.EnableAnnotations();
        c.IgnoreObsoleteActions();
        c.SwaggerDoc("v2", new OpenApiInfo
        {
            Version = "v2",
            Title = "Georgia EPD Enforcement Orders API",
            Contact = new OpenApiContact
            {
                Name = "Georgia EPD-IT Support",
                Email = builder.Configuration["SupportEmail"]
            }
        });
    });

// Configure application monitoring
builder.Services.AddHttpContextAccessor() // needed by RaygunScriptPartial
    .AddRaygun(builder.Configuration,
        new RaygunMiddlewareSettings { ClientProvider = new RaygunClientProvider() });

// Configure the data repositories
if (builder.Environment.IsLocalEnv())
{
    // When running locally, use an in-memory database (only used for Identity)
    builder.Services.AddDbContext<EnfoDbContext>(opts =>
        opts.UseInMemoryDatabase("Local_DB"));

    // Uses static data when running locally
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IEnforcementOrderRepository, EnforcementOrderRepository>();
    builder.Services.AddScoped<IEpdContactRepository, EpdContactRepository>();
    builder.Services.AddScoped<ILegalAuthorityRepository, LegalAuthorityRepository>();
}
else
{
    // When running on the server, requires a deployed database (configured in the app settings file)
    builder.Services.AddDbContext<EnfoDbContext>(opts =>
        opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsAssembly("Infrastructure")));

    builder.Services.AddScoped<IUserService,
        Enfo.Infrastructure.Services.UserService>();
    builder.Services.AddScoped<IEnforcementOrderRepository,
        Enfo.Infrastructure.Repositories.EnforcementOrderRepository>();
    builder.Services.AddScoped<IEpdContactRepository,
        Enfo.Infrastructure.Repositories.EpdContactRepository>();
    builder.Services.AddScoped<ILegalAuthorityRepository,
        Enfo.Infrastructure.Repositories.LegalAuthorityRepository>();
}

// Initialize database
builder.Services.AddHostedService<MigratorHostedService>();

// Configure validators
builder.Services.AddTransient<IValidator<EnforcementOrderCreate>, EnforcementOrderCreateValidator>();
builder.Services.AddTransient<IValidator<EnforcementOrderUpdate>, EnforcementOrderUpdateValidator>();
builder.Services.AddTransient<IValidator<LegalAuthorityCommand>, LegalAuthorityValidator>();

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
    app.UseRaygun();
}

// Configure security HTTP headers
app.UseSecurityHeaders(policies => policies.AddEnfoSecurityHeaderPolicies());

// Configure the application
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configure API documentation
app.UseSwagger(c => { c.RouteTemplate = "api-docs/{documentName}/openapi.json"; });
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api-docs/v2/openapi.json", "ENFO API v2");
    c.RoutePrefix = "api-docs";
    c.DocumentTitle = "Georgia EPD Enforcement Orders API";
});

// Map endpoints
app.MapRazorPages();
app.MapControllers();

// Make it so
app.Run();
