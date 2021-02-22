using System;
using static Enfo.Domain.Entities.Enums;

namespace Enfo.Repository.Resources.EnforcementOrder
{
    public class EnforcementOrderSpec
    {
        string FacilityFilter { get; set; }
        string County { get; set; }
        int? LegalAuth { get; set; }
        DateTime? FromDate { get; set; }
        DateTime? TillDate { get; set; }
        ActivityStatus Status { get; set; }
        PublicationStatus PublicationStatus { get; set; }
        string OrderNumber { get; set; }
        string TextContains { get; set; }
        bool OnlyIfPublic { get; set; }
        bool Deleted { get; set; }
        EnforcementOrderSorting SortOrder { get; set; }
    }
}