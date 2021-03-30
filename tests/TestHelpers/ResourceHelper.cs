using System.Collections.Generic;
using System.Linq;
using Enfo.Domain.Entities;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Resources.LegalAuthority;
using static TestHelpers.DataHelper;

namespace TestHelpers
{
    public static class ResourceHelper
    {
        // Enforcement Orders
        public static EnforcementOrderDetailedView GetEnforcementOrderDetailedView(int id) =>
            new(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));

        public static List<EnforcementOrderDetailedView> GetEnforcementOrderDetailedViewList() =>
            GetEnforcementOrders.Select(e => GetEnforcementOrderDetailedView(e.Id)).ToList();

        public static EnforcementOrderSummaryView GetEnforcementOrderSummaryView(int id) =>
            new(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));

        public static List<EnforcementOrderSummaryView> GetEnforcementOrderSummaryViewListOfOne() =>
            new() {GetEnforcementOrderSummaryView(GetEnforcementOrders.First().Id)};

        public static EnforcementOrderAdminView GetEnforcementOrderAdminView(int id) =>
            new(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));

        public static EnforcementOrderAdminSummaryView GetEnforcementOrderAdminSummaryView(int id) =>
            new(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));
        
        public static List<EnforcementOrderAdminSummaryView> GetEnforcementOrderAdminSummaryViewList() =>
            GetEnforcementOrders.Select(e => GetEnforcementOrderAdminSummaryView(e.Id)).ToList();

        public static List<EnforcementOrderAdminSummaryView> GetEnforcementOrderAdminSummaryViewListOfOne() =>
            new() {GetEnforcementOrderAdminSummaryView(GetEnforcementOrders.First().Id)};

        public static EnforcementOrder FillNavigationProperties(EnforcementOrder order)
        {
            order.LegalAuthority = GetLegalAuthorities.SingleOrDefault(e => e.Id == order.LegalAuthorityId);
            order.CommentContact = GetEpdContacts.SingleOrDefault(e => e.Id == order.CommentContactId);
            if (order.CommentContact != null)
                order.CommentContact.Address =
                    GetAddresses.SingleOrDefault(e => e.Id == order.CommentContact.AddressId);
            order.HearingContact = GetEpdContacts.SingleOrDefault(e => e.Id == order.HearingContactId);
            if (order.HearingContact != null)
                order.HearingContact.Address =
                    GetAddresses.SingleOrDefault(e => e.Id == order.HearingContact.AddressId);

            return order;
        }

        // Legal Authorities

        public static List<LegalAuthorityView> GetLegalAuthorityViewList() =>
            GetLegalAuthorities.Select(e => new LegalAuthorityView(e)).ToList();
    }
}