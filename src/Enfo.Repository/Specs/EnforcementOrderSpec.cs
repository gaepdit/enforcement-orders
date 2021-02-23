using System;

namespace Enfo.Repository.Specs
{
    public class EnforcementOrderSpec
    {
        /// <summary>
        /// ActivityStatus enum is used for searching/filtering.
        /// It relates to the IsProposedOrder and IsExecutedOrder booleans.
        /// </summary>
        public enum ActivityState
        {
            Proposed,
            Executed,
            All
        }

        /// <summary>
        /// PublicationStatus enum is used for searching/filtering.
        /// It relates to the EnforcementOrder.PublicationState enum.
        /// </summary>
        public enum PublicationState
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

        public string FacilityFilter { get; set; }
        public string County { get; set; }
        public int? LegalAuth { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? TillDate { get; set; }
        public ActivityState Status { get; set; }
        public PublicationState PublicationStatus { get; set; }
        public string OrderNumber { get; set; }
        public string TextContains { get; set; }
        public bool OnlyIfPublic { get; set; }
        public bool Deleted { get; set; }
        public EnforcementOrderSorting SortOrder { get; set; }
    }
}