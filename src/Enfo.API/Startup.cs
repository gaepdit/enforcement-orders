using System;
using System.Text.Json.Serialization;
using Enfo.Repository.Repositories;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Enfo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddHsts(options =>
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                options.MaxAge = TimeSpan.FromSeconds(300);
            });

            services.AddDbContext<EnfoDbContext>(opts => 
                opts.UseSqlite(Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("Enfo.API")));

            services.AddScoped(typeof(IWritableRepository<>), typeof(WritableRepository<>));
            services.AddScoped(typeof(IReadableRepository<>), typeof(ReadableRepository<>));
            services.AddScoped(typeof(IEnforcementOrderRepository), typeof(EnforcementOrderRepository));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Georgia EPD Enforcement Orders API",
                    Version = "v2",
                    Contact = new OpenApiContact
                    {
                        Name = "Douglas Waldron",
                        Email = "douglas.waldron@dnr.ga.gov"
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            EnfoDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger(c => { c.RouteTemplate = "api-docs/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-docs";
                c.SwaggerEndpoint("/api-docs/v2/swagger.json", "API v2");
            });

            app.UseRouting();
            // app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Initialize database
            if (env.IsDevelopment())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                Infrastructure.SeedData.DevSeedData.SeedTestData(context);
            }
            else
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}
