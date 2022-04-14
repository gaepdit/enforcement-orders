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
using Enfo.Infrastructure.Repositories;
using Enfo.Infrastructure.Services;
using Enfo.WebApp.Platform.DevHelpers;
using Enfo.WebApp.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mindscape.Raygun4Net.AspNetCore;
using System;
using System.IO;

namespace Enfo.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure database
            services.AddDbContext<EnfoDbContext>(opts =>
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("Infrastructure")));

            // Configure Identity
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<EnfoDbContext>();

            // Configure cookies
            // SameSiteMode.None is needed to get single sign-out to work
            services.Configure<CookiePolicyOptions>(opts => opts.MinimumSameSitePolicy = SameSiteMode.None);

            // Configure authentication
            // (AddAzureAD is marked as obsolete and will be removed in a future version, but
            // the replacement, Microsoft Identity Web, is net yet compatible with RoleManager.)
            // Follow along at https://github.com/AzureAD/microsoft-identity-web/issues/1091
#pragma warning disable 618
            services.AddAuthentication().AddAzureAD(opts =>
            {
                Configuration.Bind(AzureADDefaults.AuthenticationScheme, opts);
                opts.CallbackPath = "/signin-oidc";
                opts.CookieSchemeName = "Identity.External";
            });
            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, opts =>
            {
                opts.Authority += "/v2.0/";
                opts.TokenValidationParameters.ValidateIssuer = true;
                opts.UsePkce = true;
            });
#pragma warning restore 618

            // Persist data protection keys
            var keysFolder = Path.Combine(Configuration["PersistedFilesBasePath"], "DataProtectionKeys");
            services.AddDataProtection().PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));

            // Configure authorization policies
            services.AddAuthorization();

            // Configure UI
            services.AddRazorPages().AddFluentValidation();

            // Add API documentation
            services.AddSwaggerGen(c =>
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
                        Email = Configuration["SupportEmail"]
                    }
                });
            });

            // Configure HSTS
            services.AddHsts(opts => opts.MaxAge = TimeSpan.FromSeconds(63072000));

            // Configure Raygun
            services.AddRaygun(Configuration,
                new RaygunMiddlewareSettings { ClientProvider = new RaygunClientProvider() });

            // Register IHttpContextAccessor (needed by RaygunScriptPartial)
            services.AddHttpContextAccessor();

            // Configure repositories
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEnforcementOrderRepository, EnforcementOrderRepository>();
            services.AddScoped<IEpdContactRepository, EpdContactRepository>();
            services.AddScoped<ILegalAuthorityRepository, LegalAuthorityRepository>();

            // Initialize database
            services.AddHostedService<MigratorHostedService>();

            // Configure validators
            services.AddTransient<IValidator<EnforcementOrderCreate>, EnforcementOrderCreateValidator>();
            services.AddTransient<IValidator<EnforcementOrderUpdate>, EnforcementOrderUpdateValidator>();
            services.AddTransient<IValidator<LegalAuthorityCommand>, LegalAuthorityValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsLocalDev() || env.IsDevelopment())
            {
                // Local or Dev server
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Production or Staging
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            if (!env.IsLocalDev())
            {
                app.UseRaygun();
            }

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
