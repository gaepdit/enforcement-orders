using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Enfo.Domain;

public static class AppServiceRegistration
{
    public static void RegisterServices(this IServiceCollection services) => services
        .AddValidatorsFromAssemblyContaining(typeof(AppServiceRegistration));
}
