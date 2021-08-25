using System.Text.Json.Serialization;

namespace Enfo.Domain.Specs
{
    /// <summary>
    /// ActivityStatus enum is used for searching/filtering.
    /// It relates to the IsProposedOrder and IsExecutedOrder booleans.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
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
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PublicationState
    {
        All,
        Draft,
        Published,
    }

    /// <summary>
    /// EnforcementOrderSorting specifies the sort order for Enforcement Orders searches.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderSorting
    {
        DateDesc,
        DateAsc,
        FacilityDesc,
        FacilityAsc,
    }
}
