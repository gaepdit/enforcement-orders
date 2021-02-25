namespace Enfo.Repository.Resources
{
    public static class ResourceRegex
    {
        public const string PostalCode = "^\\d{5}(-\\d{4})?$";
        public const string Telephone = "^\\D?(\\d{3})\\D?\\D?(\\d{3})\\D?(\\d{4})$";

        public const string Email =
            "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
    }
}