using System.Collections.Generic;
using System.Linq;
using Enfo.Domain.Entities;
using Enfo.Repository.Resources.EnforcementOrder;
using static TestHelpers.DataHelper;

namespace TestHelpers
{
    public static class ResourceHelper
    {
        public static EnforcementOrderDetailedView GetEnforcementOrderDetailedView(int id) =>
            new(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));

        public static List<EnforcementOrderDetailedView> GetEnforcementOrderDetailedViewList() =>
            GetEnforcementOrders.Select(e => new EnforcementOrderDetailedView(e)).ToList();

        public static EnforcementOrderSummaryView GetEnforcementOrderSummaryView(int id) =>
            new(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));

        public static EnforcementOrderAdminView GetEnforcementOrderAdminView(int id) =>
            new(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));

        public static EnforcementOrderAdminSummaryView GetEnforcementOrderAdminSummaryView(int id) =>
            new(FillNavigationProperties(GetEnforcementOrders.Single(e => e.Id == id)));

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
    }
}