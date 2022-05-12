using Enfo.Domain.EnforcementOrders.Resources;
using JetBrains.Annotations;

namespace Enfo.WebApp.Api;

public class EnforcementOrderApiView
{
    public EnforcementOrderApiView(EnforcementOrderDetailedView e, string baseUrl) =>
        (EnforcementOrder, BaseUrl) = (e, baseUrl);

    private string BaseUrl { get; }

    [UsedImplicitly]
    public string Link => $"{BaseUrl}/Details/{EnforcementOrder.Id}";

    [UsedImplicitly]
    public EnforcementOrderDetailedView EnforcementOrder { get; }
}
