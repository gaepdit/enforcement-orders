using Enfo.Domain.Services;
using Enfo.WebApp.Platform.Settings;
using Mindscape.Raygun4Net.AspNetCore;

namespace Enfo.WebApp.Platform.Logging;

/// <inheritdoc />
public class ErrorLogger(IServiceProvider serviceProvider) : IErrorLogger
{
    public Task LogErrorAsync(Exception exception, Dictionary<string, object> customData = null) =>
        string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)
            ? Task.CompletedTask
            : serviceProvider.GetService<RaygunClient>()!.SendInBackground(exception, null, customData);
}
