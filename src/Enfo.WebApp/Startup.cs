using System;
using System.IO;
using Enfo.Domain.Entities.Users;
using Enfo.Domain.Repositories;
using Enfo.Domain.Services;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Enfo.Infrastructure.Services;
using Enfo.WebApp.Platform.DevHelpers;
using Enfo.WebApp.Services;
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
using Mindscape.Raygun4Net.AspNetCore;

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
                    x => x.MigrationsAssembly("Enfo.Infrastructure")));

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
            var keysFolder = Path.Combine(Configuration["PersistedFilesBasePath"], "DataProtectionKeys");
            services.AddDataProtection().PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));

            // Configure authorization policies
            services.AddAuthorization();

            // Configure UI
            services.AddRazorPages();

            // Configure HSTS
            services.AddHsts(opts => opts.MaxAge = TimeSpan.FromDays(30));

            // Configure Raygun
            services.AddRaygun(Configuration,
                new RaygunMiddlewareSettings {ClientProvider = new RaygunClientProvider()});

            // Register IHttpContextAccessor (needed by RaygunScriptPartial)
            services.AddHttpContextAccessor();

            // Configure dependencies
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IEnforcementOrderRepository, EnforcementOrderRepository>();
            services.AddScoped<IEpdContactRepository, EpdContactRepository>();
            services.AddScoped<ILegalAuthorityRepository, LegalAuthorityRepository>();

            // Initialize database
            services.AddHostedService<MigratorHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsLocalDev())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseRaygun();
                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseRaygun();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}