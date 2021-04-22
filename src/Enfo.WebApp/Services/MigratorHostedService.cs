﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Enfo.Infrastructure.Contexts;
using Microsoft.AspNetCore.Hosting;
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

        [SuppressMessage("ReSharper", "S125")]
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a new scope to retrieve scoped services
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EnfoDbContext>();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            if (env.IsEnvironment("Local"))
            {
                // Initialize database
                await context.Database.EnsureDeletedAsync(cancellationToken);
                await context.Database.MigrateAsync(cancellationToken);
                // await context.Database.EnsureCreatedAsync(cancellationToken);

                // Seed initial data
                await context.SeedTempDataAsync(cancellationToken);

                // Initialize roles
                // var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                // foreach (var role in UserRole.AllRoles)
                // {
                //     if (!await context.Roles.AnyAsync(e => e.Name == role.Name, cancellationToken))
                //     {
                //         await roleManager.CreateAsync(new IdentityRole<Guid>(role.Name));
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