namespace Enfo.Domain.Entities
{
    public static class Enums
    {
        /// <summary>
        /// ActivityStatus enum is used for searching/filtering.
        /// It relates to the IsProposedOrder and IsExecutedOrder booleans.
        /// </summary>
        public enum ActivityStatus
        {
            Proposed,
            Executed,
            All
        }

        /// <summary>
        /// PublicationStatus enum is used for searching/filtering.
        /// It relates to the EnforcementOrder.PublicationState enum.
        /// </summary>
        public enum PublicationStatus
        {
            Draft,
            Published,
            All
        }

        /// <summary>
        /// EnforcementOrderSorting specifies the sort order for Enforcement Orders searches.
        /// </summary>
        public enum EnforcementOrderSorting
        {
            FacilityAsc,
            FacilityDesc,
            DateAsc,
            DateDesc
        }
    }
}
