namespace Enfo.Domain.Specs
{
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
    public enum OrderSorting
    {
        DateDesc,
        DateAsc,
        FacilityDesc,
        FacilityAsc,
    }
}
