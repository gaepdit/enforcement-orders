using System;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.API.QueryStrings
{
    public class EnforcementOrderFilter
    {
        public string FacilityFilter { get; set; } = null;
        public string County { get; set; } = null;
        public int? LegalAuth { get; set; } = null;
        public DateTime? FromDate { get; set; } = null;
        public DateTime? TillDate { get; set; } = null;
        public ActivityStatus Status { get; set; } = ActivityStatus.All;
        public PublicationStatus PublicationStatus { get; set; } = PublicationStatus.Published;
        public string OrderNumber { get; set; } = null;
        public string TextContains { get; set; } = null;
        public EnforcementOrderSorting SortOrder { get; set; } = EnforcementOrderSorting.FacilityAsc;
        public bool Deleted { get; set; } = false;
    }
}
