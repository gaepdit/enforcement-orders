using JetBrains.Annotations;

namespace Enfo.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static string Version { get; private set; }
    public static Support SupportSettings { get; } = new();
    public static Raygun RaygunSettings { get; } = new();
    public static string OrgNotificationsApiUrl { get; private set; }

    public record Support
    {
        public string TechnicalSupportEmail { get; init; }
        public string TechnicalSupportSite { get; init; }
    }

    public record Raygun
    {
        public string ApiKey { get; [UsedImplicitly] init; }
    }
}
