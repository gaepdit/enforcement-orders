using Enfo.Domain.Services;
using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net.AspNetCore;

namespace Enfo.WebApp.Platform.Raygun;

/// <inheritdoc />
public class ErrorLogger(IServiceProvider serviceProvider) : IErrorLogger
{
    public Task LogErrorAsync(Exception exception, Dictionary<string, object> customData = null) =>
         serviceProvider.GetService<RaygunClient>().SendInBackground(exception, null, customData); 
}
