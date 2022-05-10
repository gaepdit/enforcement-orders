using Enfo.Domain.Users.Entities;
using Enfo.Infrastructure.Contexts;
using Enfo.WebApp.Platform.Local;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Platform.Migrator;

/// <summary>
/// Sets up the database. 
/// ref: https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/
/// </summary>
public class MigratorHostedService : IHostedService
{
    // We need to inject the IServiceProvider so we can create the DbContext scoped service
    private readonly IServiceProvider _serviceProvider;
    public MigratorHostedService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Create a new scope to retrieve scoped services.
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EnfoDbContext>();

        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        // Initialize database.
        if (env.IsLocalEnv())
        {
            // Delete and re-create database as currently defined.
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }
        else
        {
            // Initialize database/run database migrations.
            await context.Database.MigrateAsync(cancellationToken);
        }

        // Initialize any new roles.
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        foreach (var role in UserRole.AllRoles.Keys)
            if (!await context.Roles.AnyAsync(e => e.Name == role, cancellationToken))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
    }

    // noop
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
