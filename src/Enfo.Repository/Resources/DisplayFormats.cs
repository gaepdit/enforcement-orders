namespace Enfo.Repository.Resources
{
    public static class DisplayFormats
    {
        // DateTime display formats
        public const string EditDate = "{0:d}";
        public const string EditDateTime = "{0:g}";
        public const string LongDate = "dddd, MMMM\u00a0d, yyyy";
        public const string ShortDate = "d\u2011MMM\u2011yyyy";
        public const string ShortDateComposite = "{0:d\u2011MMM\u2011yyyy}";
        public const string DateTimeComposite = "{0:d\u2011MMM\u2011yyyy, h:mm tt}";
    }
}