using Enfo.Domain.Services;
using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net.AspNetCore;

namespace Enfo.WebApp.Platform.Raygun;

/// <inheritdoc />
public class ErrorLogger : IErrorLogger
{
    private readonly IRaygunAspNetCoreClientProvider _clientProvider;
    private readonly IOptions<RaygunSettings> _settings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ErrorLogger(
        IRaygunAspNetCoreClientProvider clientProvider, 
        IOptions<RaygunSettings> settings,
        IHttpContextAccessor httpContextAccessor)
    {
        _clientProvider = clientProvider;
        _settings = settings;
        _httpContextAccessor = httpContextAccessor;
    }
    public Task LogErrorAsync(Exception exception, Dictionary<string, object> customData = null) => 
        _clientProvider.GetClient(_settings.Value, _httpContextAccessor.HttpContext)
            .SendInBackground(exception, null, customData);
}
