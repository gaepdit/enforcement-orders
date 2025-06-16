using Enfo.Domain.Users;
using Enfo.EfRepository.Contexts;
using Enfo.LocalRepository.Identity;
using Enfo.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;

namespace Enfo.WebApp.Platform.AppConfiguration;

public static class IdentityStores
{
    public static void AddIdentityStores(this IServiceCollection services)
    {
        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole<Guid>>();

        if (AppSettings.DevSettings.BuildDatabase)
        {
            // Add EF identity stores.
            identityBuilder.AddRoles<IdentityRole<Guid>>().AddEntityFrameworkStores<EnfoDbContext>();
        }
        else
        {
            // Add local UserStore and RoleStore.
            services.AddSingleton<IUserStore<ApplicationUser>, LocalUserStore>();
            services.AddSingleton<IRoleStore<IdentityRole<Guid>>, LocalRoleStore>();
        }
    }
}
