using Enfo.AppServices.Staff;
using Microsoft.Extensions.DependencyInjection;

namespace Enfo.AppServices.AuthenticationServices;

public static class AuthenticationAppServices
{
    public static IServiceCollection AddAuthenticationAppServices(this IServiceCollection services) => services
        .AddScoped<IStaffService, StaffService>()
        .AddScoped<IAuthenticationManager, AuthenticationManager>();
}
