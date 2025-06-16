namespace Enfo.WebApp.Models;

public class DisplayMessage(Context context, string message, bool showCloseButton = true)
{
    // ReSharper disable once MemberCanBePrivate.Global
    // Context must be public so it works with deserialization in TempDataExtensions class
    public Context Context { get; } = context;
    public string Message { get; } = message;
    public bool ShowCloseButton { get; } = showCloseButton;

    public string AlertClass => Context switch
    {
        Context.Success => "usa-alert-success",
        Context.Warning => "usa-alert-warning",
        Context.Error => "usa-alert-error",
        Context.Info => "usa-alert-info",
        _ => string.Empty,
    };
}

public enum Context
{
    Success,
    Warning,
    Error,
    Info,
}
