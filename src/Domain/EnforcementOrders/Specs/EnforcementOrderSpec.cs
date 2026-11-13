using System.Globalization;
using Enfo.Domain.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace Enfo.Domain.EnforcementOrders.Specs;

public class EnforcementOrderSpec
{
    [DisplayName("Facility")]
    public string Facility { get; set; }

    public string County { get; set; }

    [DisplayName("Order Number")]
    public string OrderNumber { get; set; }

    [DisplayName("Legal Authority")]
    public int? LegalAuth { get; set; }

    [Display(Name = "Beginning Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.DateSearch, ApplyFormatInEditMode = true)]
    [SwaggerSchema(Format = "date")]
    public DateTime? FromDate { get; set; }

    [Display(Name = "Ending Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.DateSearch, ApplyFormatInEditMode = true)]
    [SwaggerSchema(Format = "date")]
    public DateTime? TillDate { get; set; }

    [DisplayName("Order Status")]
    public ActivityState Status { get; set; } = ActivityState.All;

    public OrderSorting Sort { get; set; } = OrderSorting.DateDesc;

    public void TrimAll()
    {
        County = County?.Trim();
        Facility = Facility?.Trim();
        OrderNumber = OrderNumber?.Trim();
    }

    public IDictionary<string, string> AsRouteValues() => new Dictionary<string, string>
    {
        { nameof(Facility), Facility },
        { nameof(County), County },
        { nameof(LegalAuth), LegalAuth?.ToString() },
        { nameof(FromDate), FromDate?.ToString(DisplayFormats.RouteValue, CultureInfo.InvariantCulture) },
        { nameof(TillDate), TillDate?.ToString(DisplayFormats.RouteValue, CultureInfo.InvariantCulture) },
        { nameof(Status), Status.ToString() },
        { nameof(OrderNumber), OrderNumber },
    };
}
