using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EpdContacts.Resources;
using Enfo.Domain.LegalAuthorities.Resources;

namespace EnfoTests.TestData;

public static class ResourceHelper
{
    // Enforcement Orders
    public static EnforcementOrderDetailedView GetEnforcementOrderDetailedView(int id) =>
        new(FillNavigationProperties(EnforcementOrderData.EnforcementOrders.Single(e => e.Id == id)));

    public static List<EnforcementOrderDetailedView> GetEnforcementOrderDetailedViewList() =>
        EnforcementOrderData.EnforcementOrders.Select(e => GetEnforcementOrderDetailedView(e.Id)).ToList();

    public static EnforcementOrderSummaryView GetEnforcementOrderSummaryView(int id) =>
        new(FillNavigationProperties(EnforcementOrderData.EnforcementOrders.Single(e => e.Id == id)));

    public static List<EnforcementOrderSummaryView> GetEnforcementOrderSummaryViewListOfOne() =>
        new() { GetEnforcementOrderSummaryView(EnforcementOrderData.EnforcementOrders[0].Id) };

    public static EnforcementOrderAdminView GetEnforcementOrderAdminView(int id) =>
        new(FillNavigationProperties(EnforcementOrderData.EnforcementOrders.Single(e => e.Id == id)));

    public static EnforcementOrderAdminSummaryView GetEnforcementOrderAdminSummaryView(int id) =>
        new(FillNavigationProperties(EnforcementOrderData.EnforcementOrders.Single(e => e.Id == id)));

    public static List<EnforcementOrderAdminSummaryView> GetEnforcementOrderAdminSummaryViewList() =>
        EnforcementOrderData.EnforcementOrders.Select(e => GetEnforcementOrderAdminSummaryView(e.Id)).ToList();

    public static IEnumerable<EnforcementOrderAdminView> GetEnforcementOrderAdminViewList() =>
        EnforcementOrderData.EnforcementOrders.Select(e => GetEnforcementOrderAdminView(e.Id)).ToList();

    public static List<EnforcementOrderAdminSummaryView> GetEnforcementOrderAdminSummaryViewListOfOne() =>
        new() { GetEnforcementOrderAdminSummaryView(EnforcementOrderData.EnforcementOrders[0].Id) };

    public static EnforcementOrder FillNavigationProperties(EnforcementOrder order)
    {
        order.LegalAuthority =
            LegalAuthorityData.LegalAuthorities.SingleOrDefault(e => e.Id == order.LegalAuthorityId);
        order.CommentContact = EpdContactData.EpdContacts.SingleOrDefault(e => e.Id == order.CommentContactId);
        order.HearingContact = EpdContactData.EpdContacts.SingleOrDefault(e => e.Id == order.HearingContactId);

        return order;
    }

    // Maintenance

    public static List<EpdContactView> GetEpdContactViewList() =>
        EpdContactData.EpdContacts.Select(e => new EpdContactView(e)).ToList();

    public static List<LegalAuthorityView> GetLegalAuthorityViewList() =>
        LegalAuthorityData.LegalAuthorities.Select(e => new LegalAuthorityView(e)).ToList();
}
