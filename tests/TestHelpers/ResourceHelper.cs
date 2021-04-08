using System;
using System.Collections.Generic;
using System.Linq;
using Enfo.Domain.Entities;
using Enfo.Repository.Resources.Address;
using Enfo.Repository.Resources.EnforcementOrder;
using Enfo.Repository.Resources.EpdContact;
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

        public static EnforcementOrderCreate GetValidEnforcementOrderCreate() => new()
        {
            Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus.",
            CommentContactId = 2004,
            CommentPeriodClosesDate = new DateTime(2012, 11, 15),
            County = "Liberty",
            ExecutedDate = new DateTime(1998, 06, 29),
            ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
            FacilityName = "A diam maecenas",
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
            HearingContactId = 2004,
            HearingDate = new DateTime(2012, 11, 15),
            HearingLocation = "venenatis urna cursus viverra mauris in aliquam sem",
            IsHearingScheduled = true,
            LegalAuthorityId = 7,
            OrderNumber = "EPD-ACQ-7936",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            Progress = PublicationState.Published,
            Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum.",
            SettlementAmount = 2000,
        };

        // Maintenance

        public static List<AddressView> GetAddressViewList() =>
            GetAddresses.Select(e => new AddressView(e)).ToList();

        public static List<EpdContactView> GetEpdContactViewList() =>
            GetEpdContacts.Select(e => new EpdContactView(FillNavigationProperties(e))).ToList();

        private static EpdContact FillNavigationProperties(EpdContact contact)
        {
            contact.Address = GetAddresses.SingleOrDefault(e => e.Id == contact.AddressId);
            return contact;
        }

        public static List<LegalAuthorityView> GetLegalAuthorityViewList() =>
            GetLegalAuthorities.Select(e => new LegalAuthorityView(e)).ToList();
    }
}