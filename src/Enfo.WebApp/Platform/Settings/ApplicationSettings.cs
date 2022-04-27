using JetBrains.Annotations;

namespace Enfo.WebApp.Platform.Settings;

public static class ApplicationSettings
{
    public const string RaygunSettingsSection = "RaygunSettings";
    public static RaygunSettings Raygun { get; } = new();
}

public class RaygunSettings
{
    public string ApiKey { get; [UsedImplicitly] init; }
}
