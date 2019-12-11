using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Contexts;
using Enfo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Enfo.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHsts(options =>
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                options.MaxAge = TimeSpan.FromSeconds(300);
            });

            services.AddDbContext<EnfoDbContext>(options => options.UseSqlite("Data Source=EnfoSqliteDatabase.db"));
            //services.AddDbContext<EnfoDbContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Enfo;Trusted_Connection=True;"));

            services.AddScoped(typeof(IAsyncWritableRepository<>), typeof(WritableRepository<>));
            services.AddScoped(typeof(IAsyncReadableRepository<>), typeof(ReadableRepository<>));

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
                // DescribeAllEnumsAsStrings() is obsolete but cannot be removed until this Swashbuckle issue is fixed:
                // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1269#issuecomment-534298112
                c.DescribeAllEnumsAsStrings();
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
