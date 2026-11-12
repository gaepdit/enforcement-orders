using System.Text.Json.Serialization;

namespace Enfo.WebApp.Models;

[method: JsonConstructor]
public record DisplayMessage(DisplayMessage.AlertContext Context, string Message, List<string> Details)
{
    public DisplayMessage(AlertContext context, string message) : this(context, message, []) { }

    [JsonIgnore]
    public string AlertClass => Context switch
    {
        AlertContext.Primary => "alert-primary",
        AlertContext.Secondary => "alert-secondary",
        AlertContext.Success => "alert-success",
        AlertContext.Danger => "alert-danger",
        AlertContext.Warning => "alert-warning",
        AlertContext.Info => "alert-info",
        _ => string.Empty,
    };

    public enum AlertContext
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
    }
}
