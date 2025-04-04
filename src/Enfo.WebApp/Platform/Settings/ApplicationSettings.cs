using JetBrains.Annotations;

namespace Enfo.WebApp.Platform.Settings;

public static class ApplicationSettings
{
    public const string RaygunSettingsSection = "RaygunSettings";
    public static RaygunClientSettings RaygunClientSettings { get; } = new();

    public const string LocalDevSettingsSection = "LocalDevSettings";
    public static LocalDevSettings LocalDevSettings { get; } = new();

    public static string OrgNotificationsApiUrl { get; set; }
}

public class RaygunClientSettings
{
    public string ApiKey { get; [UsedImplicitly] init; }
}

public class LocalDevSettings
{
    public bool UseAzureAd { get; [UsedImplicitly] init; }
    public bool AuthenticatedUser { get; [UsedImplicitly] init; }
    public bool BuildLocalDb { get; [UsedImplicitly] init; }
    public bool UseSecurityHeadersLocally { get; [UsedImplicitly] init; }
}
