using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Enfo.Repository.Repositories;
using Enfo.WebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
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

            // Configure authentication
            // services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            //     .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

            // services.AddAuthorization(options =>
            // {
            //     // By default, all incoming requests will be authorized according to the default policy
            //     options.FallbackPolicy = options.DefaultPolicy;
            // });
            // services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(_dataProtectionKeysFolder));

            // Configure UI
            services.AddRazorPages();
            // .AddMicrosoftIdentityUI();

            // Configure HSTS
            // services.AddHsts(opts => opts.MaxAge = TimeSpan.FromDays(365 * 2));

            // Configure Raygun
            services.AddRaygun(Configuration,
                new RaygunMiddlewareSettings {ClientProvider = new RaygunClientProvider()});
            
            // Register IHttpContextAccessor (needed by RaygunScriptPartial)
            services.AddHttpContextAccessor();

            // Configure data repositories
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}