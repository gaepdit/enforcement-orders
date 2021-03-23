using System;

namespace Enfo.Repository.Specs
{
    public class EnforcementOrderSpec
    {
        private bool _onlyPublic = true;
        private bool _showDeleted;

        /// <summary>
        /// ActivityStatus enum is used for searching/filtering.
        /// It relates to the IsProposedOrder and IsExecutedOrder booleans.
        /// </summary>
        public enum ActivityState
        {
            All,
            Proposed,
            Executed,
        }

        /// <summary>
        /// PublicationStatus enum is used for searching/filtering.
        /// It relates to the EnforcementOrder.PublicationState enum.
        /// </summary>
        public enum PublicationState
        {
            All,
            Draft,
            Published,
        }

        /// <summary>
        /// EnforcementOrderSorting specifies the sort order for Enforcement Orders searches.
        /// </summary>
        public enum EnforcementOrderSorting
        {
            DateDesc,
            DateAsc,
            FacilityDesc,
            FacilityAsc,
        }

        public string FacilityFilter { get; set; }
        public string County { get; set; }
        public int? LegalAuthId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? TillDate { get; set; }
        public ActivityState Status { get; set; } = ActivityState.All;
        public PublicationState PublicationStatus { get; set; } = PublicationState.All;
        public string OrderNumber { get; set; }
        public string TextContains { get; set; }

        public bool OnlyPublic
        {
            get => _onlyPublic;
            set
            {
                _onlyPublic = value;
                if (value) _showDeleted = false;
            }
        }

        // Either deleted or active items are returned; not both.
        public bool ShowDeleted
        {
            get => _showDeleted;
            set
            {
                _showDeleted = value;
                if (value) _onlyPublic = false;
            }
        }

        public EnforcementOrderSorting SortOrder { get; set; } = EnforcementOrderSorting.DateDesc;

        public void TrimAll()
        {
            County = County?.Trim();
            FacilityFilter = FacilityFilter?.Trim();
            OrderNumber = OrderNumber?.Trim();
            TextContains = TextContains?.Trim();
        }
    }
}