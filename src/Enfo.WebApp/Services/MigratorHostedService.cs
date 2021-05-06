using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Enfo.Domain.Entities.Users;
using Enfo.Infrastructure.Contexts;
using Enfo.WebApp.Platform.DevHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enfo.WebApp.Services
{
    /// <summary>
    /// Sets up the database. 
    /// ref: https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/
    /// </summary>
    public class MigratorHostedService : IHostedService
    {
        // We need to inject the IServiceProvider so we can create the DbContext scoped service
        private readonly IServiceProvider _serviceProvider;
        public MigratorHostedService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        [SuppressMessage("ReSharper", "S125", Justification = "Local dev code has optional EF migration call.")]
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a new scope to retrieve scoped services
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EnfoDbContext>();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            // Initialize database
            if (env.IsLocalDev())
            {
                if (Environment.GetEnvironmentVariable("ENABLE_EF_MIGRATIONS") == "true")
                {
                    // If EF migrations are enabled, preserve database and run migrations.
                    await context.Database.MigrateAsync(cancellationToken);
                }
                else
                {
                    // Otherwise, delete and re-create database as currently defined.
                    await context.Database.EnsureDeletedAsync(cancellationToken);
                    await context.Database.EnsureCreatedAsync(cancellationToken);
                }

                // Seed initial data
                await context.SeedTempDataAsync(cancellationToken);
            }
            else
            {
                // Run database migrations
                await context.Database.MigrateAsync(cancellationToken);
            }

            // Initialize roles
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            foreach (var role in UserRole.AllRoles.Keys)
                if (!await context.Roles.AnyAsync(e => e.Name == role, cancellationToken))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        // noop
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}