namespace Enfo.WebApp.Models
{
    public class DisplayMessage
    {
        private Context Context { get; }
        public string Message { get; }

        public DisplayMessage(Context context, string message)
        {
            Context = context;
            Message = message;
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