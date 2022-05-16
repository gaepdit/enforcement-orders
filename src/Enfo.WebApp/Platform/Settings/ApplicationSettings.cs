using JetBrains.Annotations;

namespace Enfo.WebApp.Platform.Settings;

public static class ApplicationSettings
{
    public const string RaygunSettingsSection = "RaygunSettings";
    public static RaygunClientSettings RaygunClientSettings { get; } = new();
}

public class RaygunClientSettings
{
    public string ApiKey { get; [UsedImplicitly] init; }
}
