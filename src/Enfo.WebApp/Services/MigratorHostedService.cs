using System;
using System.Threading;
using System.Threading.Tasks;
using Enfo.Infrastructure.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enfo.WebApp.Services
{
    /// <summary>
    /// Set up the database 
    /// ref: https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/
    /// </summary>
    public class MigratorHostedService : IHostedService
    {
        // We need to inject the IServiceProvider so we can create the DbContext scoped service
        private readonly IServiceProvider _serviceProvider;
        public MigratorHostedService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a new scope to retrieve scoped services
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EnfoDbContext>();
            // var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            if (env.IsDevelopment())
            {
                // Initialize database
                await context.Database.EnsureDeletedAsync(cancellationToken);
                await context.Database.EnsureCreatedAsync(cancellationToken);

                // Seed initial data
                await context.SeedTempDataAsync(cancellationToken);

                // Initialize roles
                // foreach (var role in UserRoles.AllRoles)
                // {
                //     if (!await context.Roles.AnyAsync(e => e.Name == role, cancellationToken))
                //     {
                //         await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                //     }
                // }
            }
            else
            {
                // Run database migrations
                await context.Database.MigrateAsync(cancellationToken);
            }
        }

        // noop
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}