using Enfo.WebApp.Platform.Settings;
using GaEpd.GuardClauses;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using ZLogger;

namespace Enfo.WebApp.Platform.OrgNotifications;

// Organizational notifications

public static class OrgNotificationsServiceExtensions
{
    public static IServiceCollection AddOrgNotifications(this IServiceCollection services) =>
        services.AddHttpClient().AddScoped<IOrgNotifications, OrgNotifications>();
}

public interface IOrgNotifications
{
    Task<List<OrgNotification>> GetOrgNotificationsAsync();
}

public record OrgNotification
{
    public required string Message { get; [UsedImplicitly] init; }
}

public class OrgNotifications(
    IHttpClientFactory http,
    IMemoryCache cache,
    ILogger<OrgNotifications> logger) : IOrgNotifications
{
    private const string ApiEndpoint = "/current";
    private const string CacheKey = nameof(OrgNotifications);
    internal static readonly EventId OrgNotificationsFetchFailure = new(2501, nameof(OrgNotificationsFetchFailure));

    public async Task<List<OrgNotification>> GetOrgNotificationsAsync()
    {
        if (string.IsNullOrEmpty(AppSettings.OrgNotificationsApiUrl)) return [];

        if (cache.TryGetValue(CacheKey, out List<OrgNotification>? notifications) && notifications != null)
            return notifications;

        try
        {
            notifications = await http.FetchApiDataAsync<List<OrgNotification>>(
                AppSettings.OrgNotificationsApiUrl, ApiEndpoint, "NotificationsClient") ?? [];
        }
        catch (Exception ex)
        {
            // If the API is unresponsive or other error occurs, no notifications will be displayed.
            logger.ZLogError(OrgNotificationsFetchFailure, ex, $"Failed to fetch organizational notifications.");
            notifications = [];
        }

        cache.Set(CacheKey, notifications, new TimeSpan(hours: 1, minutes: 0, seconds: 0));
        return notifications;
    }
}

public static class ApiExtensions
{
    public static async Task<T?> FetchApiDataAsync<T>(this IHttpClientFactory httpClientFactory,
        string apiUrl, string endpointPath, string clientName)
    {
        var client = httpClientFactory.CreateClient(clientName);
        using var response = await client.GetAsync(UrlCombine(apiUrl, endpointPath));
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    /// <summary>
    /// Combine a base URL and a relative URL path, correctly handling path separators.
    /// The base URL must be an absolute URL.
    /// </summary>
    /// <param name="baseUrl">The base URL.</param>
    /// <param name="relativeUrl">The relative path.</param>
    /// <returns>The combined URL.</returns>
    private static Uri UrlCombine(string baseUrl, string? relativeUrl) =>
        UrlCombine(new Uri(Guard.NotNullOrWhiteSpace(baseUrl)), relativeUrl);

    /// <summary>
    /// Combine a base URL and a relative URL path, correctly handling path separators.
    /// The base URL must be an absolute URL.
    /// </summary>
    /// <param name="baseUrl">The base URL.</param>
    /// <param name="relativeUrl">The relative path.</param>
    /// <returns>The combined URL.</returns>
    private static Uri UrlCombine(Uri baseUrl, string? relativeUrl)
    {
        Guard.NotNull(baseUrl);
        if (!baseUrl.IsAbsoluteUri) throw new ArgumentOutOfRangeException(nameof(baseUrl));

        if (string.IsNullOrEmpty(relativeUrl)) return baseUrl;

        const char separator = '/';
        return new Uri(baseUrl.ToString().TrimEnd(separator) + separator + relativeUrl.TrimStart(separator));
    }
}
