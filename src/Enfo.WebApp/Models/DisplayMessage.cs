namespace Enfo.WebApp.Models
{
    public class DisplayMessage
    {
        private Context Context { get; }
        public string Message { get; }
        public bool ShowCloseButton { get; }

        public DisplayMessage(Context context, string message, bool showCloseButton = true)
        {
            Context = context;
            Message = message;
            ShowCloseButton = showCloseButton;
        }

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