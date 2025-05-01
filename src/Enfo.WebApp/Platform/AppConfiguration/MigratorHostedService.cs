using Enfo.Domain.Users.Entities;
using Enfo.Infrastructure.Contexts;
using Enfo.WebApp.Platform.Utilities;
using EnfoTests.TestData;
using GaEpd.FileService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Platform.AppConfiguration;

/// <summary>
/// Sets up the database. 
/// ref: https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/
/// </summary>
internal class MigratorHostedService : IHostedService
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

        // Initialize database and file store.
        if (env.IsLocalEnv())
        {
            // Delete and re-create database as currently defined.
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await SeedFileStore(scope, cancellationToken);
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

    private static async Task SeedFileStore(IServiceScope scope, CancellationToken cancellationToken)
    {
        // Initialize the attachments store
        var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();
        foreach (var attachment in AttachmentData.GetAttachmentFiles())
        {
            var fileBytes = attachment.Base64EncodedFile == null
                ? []
                : Convert.FromBase64String(attachment.Base64EncodedFile);

            await using var fileStream = new MemoryStream(fileBytes);
            await fileService.SaveFileAsync(fileStream, attachment.FileName, token: cancellationToken);
        }
    }

    // noop
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
