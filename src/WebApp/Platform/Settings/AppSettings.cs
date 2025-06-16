using JetBrains.Annotations;

namespace Enfo.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    // Support settings
    public static SupportSettingsSection SupportSettings { get; } = new();

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record SupportSettingsSection
    {
        public string TechnicalSupportEmail { get; init; }
        public string TechnicalSupportSite { get; init; }
        public string InformationalVersion { get; set; }
        public string InformationalBuild { get; set; }
    }

    // Organizational notifications
    public static string OrgNotificationsApiUrl { get; set; }

    // Raygun client settings
    public static RaygunClientSettings RaygunSettings { get; } = new();

    public record RaygunClientSettings
    {
        public string ApiKey { get; [UsedImplicitly] init; }
    }
}
