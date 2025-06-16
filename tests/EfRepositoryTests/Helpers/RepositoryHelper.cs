using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EpdContacts.Repositories;
using Enfo.Domain.LegalAuthorities.Repositories;
using Enfo.Domain.Services;
using Enfo.EfRepository.Contexts;
using Enfo.EfRepository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TestSupport.EfHelpers;

namespace EfRepositoryTests.Helpers;

public sealed class RepositoryHelper : IDisposable, IAsyncDisposable
{
    private DbContextOptions<EnfoDbContext> Options { get; set; }
    public EnfoDbContext Context { get; private set; }
    private RepositoryHelper() { }

    public static async Task<RepositoryHelper> CreateRepositoryHelperAsync()
    {
        var helper = GetRepositoryHelper();
        await helper.Context.Database.EnsureDeletedAsync();
        await helper.Context.Database.EnsureCreatedAsync();
        return helper;
    }

    private static RepositoryHelper GetRepositoryHelper()
    {
        var helper = new RepositoryHelper();
        helper.Options = SqliteInMemory.CreateOptions<EnfoDbContext>(builder => builder
            .UseAsyncSeeding((context, _, token) => SeedDataHelper.SeedAllDataAsync(context, token))
            .LogTo(Console.WriteLine, events: [RelationalEventId.CommandExecuted]));
        helper.Context = new EnfoDbContext(helper.Options, null);
        return helper;
    }

    public void ClearChangeTracker() => Context.ChangeTracker.Clear();


    public ILegalAuthorityRepository GetLegalAuthorityRepository()
    {
        Context = new EnfoDbContext(Options, null);
        return new LegalAuthorityRepository(Context);
    }

    public IEpdContactRepository GetEpdContactRepository()
    {
        Context = new EnfoDbContext(Options, null);
        return new EpdContactRepository(Context);
    }

    public IEnforcementOrderRepository GetEnforcementOrderRepository()
    {
        Context = new EnfoDbContext(Options, null);
        return new EnforcementOrderRepository(Context, Substitute.For<IAttachmentStore>(),
            Substitute.For<IErrorLogger>()
        );
    }

    public void Dispose() => Context?.Dispose();

    public async ValueTask DisposeAsync()
    {
        if (Context != null) await Context.DisposeAsync();
    }
}
