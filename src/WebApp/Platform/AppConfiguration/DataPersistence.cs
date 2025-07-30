using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.Users;
using Enfo.EfRepository.Contexts;
using Enfo.EfRepository.Repositories;
using Enfo.LocalRepository.Repositories;
using Enfo.WebApp.Platform.Settings;
using EnfoTests.TestData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Enfo.WebApp.Platform.AppConfiguration;

public static class DataPersistence
{
    public static async Task ConfigureDataPersistence(this IHostApplicationBuilder builder)
    {
        if (AppSettings.DevSettings.UseDevSettings)
        {
            await builder.ConfigureDevDataPersistence();
            return;
        }

        builder.ConfigureDatabaseServices();
        await using var migrationContext = new EnfoDbContext(GetMigrationDbOpts(builder.Configuration).Options, null);
        await migrationContext.Database.MigrateAsync();
        await migrationContext.CreateMissingRolesAsync(builder.Services);
    }

    private static void ConfigureDatabaseServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("No connection string found.");

        // Entity Framework context
        builder.Services
            .AddDbContext<EnfoDbContext>(db =>
            {
                db.UseSqlServer(connectionString, sqlServerOpts => sqlServerOpts.EnableRetryOnFailure());
                db.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
            });

        builder.Services
            .AddScoped<IEnforcementOrderRepository, EnforcementOrderRepository>()
            .AddScoped<IEpdContactRepository, EpdContactRepository>()
            .AddScoped<ILegalAuthorityRepository, LegalAuthorityRepository>();
    }

    private static DbContextOptionsBuilder<EnfoDbContext> GetMigrationDbOpts(IConfiguration configuration)
    {
        var migConnString = configuration.GetConnectionString("MigrationConnection");
        if (string.IsNullOrEmpty(migConnString))
            throw new InvalidOperationException("No migration connection string found.");

        return new DbContextOptionsBuilder<EnfoDbContext>()
            .UseSqlServer(migConnString, sqlServerOpts => sqlServerOpts.MigrationsAssembly(nameof(EfRepository)));
    }

    private static async Task CreateMissingRolesAsync(this EnfoDbContext migrationContext, IServiceCollection services)
    {
        // Initialize any new roles.
        var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        foreach (var role in AppRole.AllRoles.Keys)
            if (!await migrationContext.Roles.AnyAsync(idRole => idRole.Name == role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
    }

    private static async Task ConfigureDevDataPersistence(this IHostApplicationBuilder builder)
    {
        // When configured, build a SQL Server database; otherwise, use in-memory data.
        if (AppSettings.DevSettings.BuildDatabase)
        {
            builder.ConfigureDatabaseServices();

            await using var migrationContext =
                new EnfoDbContext(GetMigrationDbOpts(builder.Configuration).Options, null);

            await migrationContext.Database.EnsureDeletedAsync();

            if (AppSettings.DevSettings.UseEfMigrations)
                await migrationContext.Database.MigrateAsync();
            else
                await migrationContext.Database.EnsureCreatedAsync();

            migrationContext.SeedAllData();
        }
        else
        {
            builder.Services
                .AddSingleton<IEnforcementOrderRepository, LocalEnforcementOrderRepository>()
                .AddSingleton<IEpdContactRepository, LocalEpdContactRepository>()
                .AddSingleton<ILegalAuthorityRepository, LocalLegalAuthorityRepository>();
        }
    }
}
