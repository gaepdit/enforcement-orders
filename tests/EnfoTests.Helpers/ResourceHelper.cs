using System;
using System.Collections.Generic;
using System.Linq;
using Enfo.Domain.Entities;
using Enfo.Domain.Resources.EnforcementOrder;
using Enfo.Domain.Resources.EpdContact;
using Enfo.Domain.Resources.LegalAuthority;
using static EnfoTests.Helpers.DataHelper;

namespace EnfoTests.Helpers
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

        public static List<EnforcementOrderAdminView> GetEnforcementOrderAdminViewList() =>
            GetEnforcementOrders.Select(e => GetEnforcementOrderAdminView(e.Id)).ToList();

        public static List<EnforcementOrderAdminSummaryView> GetEnforcementOrderAdminSummaryViewListOfOne() =>
            new() {GetEnforcementOrderAdminSummaryView(GetEnforcementOrders.First().Id)};

        public static EnforcementOrder FillNavigationProperties(EnforcementOrder order)
        {
            order.LegalAuthority = GetLegalAuthorities.SingleOrDefault(e => e.Id == order.LegalAuthorityId);
            order.CommentContact = GetEpdContacts.SingleOrDefault(e => e.Id == order.CommentContactId);
            order.HearingContact = GetEpdContacts.SingleOrDefault(e => e.Id == order.HearingContactId);

            return order;
        }

        public static EnforcementOrderCreate GetValidEnforcementOrderCreate() => new()
        {
            Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus.",
            CommentContactId = 2000,
            CommentPeriodClosesDate = new DateTime(2012, 11, 15),
            County = "Liberty",
            ExecutedDate = new DateTime(1998, 06, 29),
            ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
            FacilityName = "A diam maecenas",
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
            HearingContactId = 2000,
            HearingDate = new DateTime(2012, 11, 15),
            HearingLocation = "venenatis urna cursus viverra mauris in aliquam sem",
            IsHearingScheduled = true,
            LegalAuthorityId = 1,
            OrderNumber = "EPD-ACQ-7936",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            Progress = PublicationProgress.Published,
            Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum.",
            SettlementAmount = 2000,
        };

        public static EnforcementOrderUpdate GetValidEnforcementOrderUpdate() => new()
        {
            Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
            CommentContactId = 2000,
            CommentPeriodClosesDate = new DateTime(2012, 11, 15),
            County = "Liberty",
            ExecutedDate = new DateTime(1998, 06, 29),
            ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
            FacilityName = "A diam maecenas",
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
            HearingContactId = 2000,
            HearingDate = new DateTime(2012, 11, 15),
            HearingLocation = "venenatis urna cursus eget nunc scelerisque viverra mauris in aliquam sem",
            IsExecutedOrder = true,
            IsHearingScheduled = true,
            LegalAuthorityId = 1,
            OrderNumber = "EPD-ACQ-7936",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16),
            Progress = PublicationProgress.Published,
            Requirements =
                "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
            SettlementAmount = 2000,
        };

        // Maintenance

        public static List<EpdContactView> GetEpdContactViewList() =>
            GetEpdContacts.Select(e => new EpdContactView(e)).ToList();

        public static List<LegalAuthorityView> GetLegalAuthorityViewList() =>
            GetLegalAuthorities.Select(e => new LegalAuthorityView(e)).ToList();
    }
}