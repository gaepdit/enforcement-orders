using JetBrains.Annotations;
using System.Reflection;

namespace Enfo.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static string Version { get; } = GetVersion();
    public static Support SupportSettings { get; } = new();
    public static Raygun RaygunSettings { get; } = new();
    public static string OrgNotificationsApiUrl { get; set; }

    public record Support
    {
        public string TechnicalSupportEmail { get; init; }
        public string TechnicalSupportSite { get; init; }
    }

    public record Raygun
    {
        public string ApiKey { get; [UsedImplicitly] init; }
    }

    private static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }
}
