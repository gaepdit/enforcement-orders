using System;
using System.ComponentModel;
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
        [DefaultValue(ActivityStatus.All)]
        public ActivityStatus Status { get; set; } = ActivityStatus.All;
        [DefaultValue(PublicationStatus.Published)]
        public PublicationStatus PublicationStatus { get; set; } = PublicationStatus.Published;
        public string OrderNumber { get; set; } = null;
        public string TextContains { get; set; } = null;
        [DefaultValue(EnforcementOrderSorting.FacilityAsc)]
        public EnforcementOrderSorting SortOrder { get; set; } = EnforcementOrderSorting.FacilityAsc;
        [DefaultValue(false)]
        public bool Deleted { get; set; } = false;
    }
}
