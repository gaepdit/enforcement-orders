namespace Enfo.WebApp.Models
{
    public class DisplayMessage
    {
        // ReSharper disable once MemberCanBePrivate.Global
        // Context must be public so it works with deserialization in TempDataExtensions class
        public Context Context { get; }
        public string Message { get; }
        public bool ShowCloseButton { get; }

        public DisplayMessage(Context context, string message, bool showCloseButton = true) =>
            (Context, Message, ShowCloseButton) = (context, message, showCloseButton);

        public string AlertClass => Context switch
        {
            Context.Success => "usa-alert-success",
            Context.Warning => "usa-alert-warning",
            Context.Error => "usa-alert-error",
            Context.Info => "usa-alert-info",
            _ => string.Empty
        };
    }

    public enum Context
    {
        Success,
        Warning,
        Error,
        Info,
    }
}