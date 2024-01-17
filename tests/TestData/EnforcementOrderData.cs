﻿using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Utils;

// ReSharper disable StringLiteralTypo

namespace EnfoTests.TestData;

internal static class EnforcementOrderData
{
    public static List<EnforcementOrder> GetEnforcementOrdersIncludeAttachments()
    {
        EnforcementOrders.ForEach(e =>
            e.Attachments = AttachmentData.Attachments
                .Where(a => a.EnforcementOrder.Id == e.Id && !a.Deleted).ToList());
        return EnforcementOrders;
    }

    public static EnforcementOrderAdminView? GetEnforcementOrderAdminView(int id)
    {
        var item = GetEnforcementOrdersIncludeAttachments().Find(e => e.Id == id);
        return item is null ? null : new EnforcementOrderAdminView(item);
    }

    public static EnforcementOrderDetailedView? GetEnforcementOrderDetailedView(int id)
    {
        var item = GetEnforcementOrdersIncludeAttachments().Find(e => e.Id == id);
        return item is null ? null : new EnforcementOrderDetailedView(item);
    }

    public static readonly List<EnforcementOrder> EnforcementOrders = new()
    {
        new EnforcementOrder
        {
            Id = 1,
            Cause = "abc1-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentContact = EpdContactData.GetEpdContact(2000),
            CommentPeriodClosesDate = new DateTime(2019, 03, 25, 0, 0, 0, DateTimeKind.Local),
            County = "Appling",
            Deleted = false,
            ExecutedDate = DateTime.Today.AddDays(-14),
            ExecutedOrderPostedDate = DateTime.Today.AddDays(-12),
            FacilityName = "abc1-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = true,
            IsHearingScheduled = false,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-WP-0001",
            ProposedOrderPostedDate = new DateTime(2019, 03, 18, 0, 0, 0, DateTimeKind.Local),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "abc1-" + Guid.NewGuid(),
            SettlementAmount = 1800,
        },
        new EnforcementOrder
        {
            Id = 2,
            Cause = "bcd2-" + Guid.NewGuid(),
            CommentContactId = null,
            CommentPeriodClosesDate = null,
            County = "Butts",
            Deleted = false,
            ExecutedDate = DateTime.Today.AddDays(-14),
            ExecutedOrderPostedDate = DateUtils.MostRecentMonday(),
            FacilityName = " bcd2-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = true,
            IsHearingScheduled = false,
            IsProposedOrder = false,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-WP-0002",
            ProposedOrderPostedDate = null,
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "bcd2-" + Guid.NewGuid(),
            SettlementAmount = 1.5m,
        },
        new EnforcementOrder
        {
            Id = 3,
            Cause = "ūrbǣnitas3-" + Guid.NewGuid(),
            CommentContactId = null,
            CommentPeriodClosesDate = null,
            County = "Bulloch",
            Deleted = false,
            ExecutedDate = DateUtils.MostRecentMonday().AddDays(1),
            ExecutedOrderPostedDate = DateUtils.MostRecentMonday().AddDays(7),
            FacilityName = "ūrbǣnitas3-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = true,
            IsHearingScheduled = false,
            IsProposedOrder = false,
            LegalAuthorityId = 1,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(1),
            OrderNumber = "EPD-AQ-0003",
            ProposedOrderPostedDate = null,
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "ūrbǣnitas3-" + Guid.NewGuid(),
            SettlementAmount = 5000,
        },
        new EnforcementOrder
        {
            Id = 4,
            Cause = "efg4-" + Guid.NewGuid(),
            CommentContactId = 2001,
            CommentContact = EpdContactData.GetEpdContact(2001),
            CommentPeriodClosesDate = DateTime.Today.AddDays(7),
            County = "Stephens",
            Deleted = true,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "efg4-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21, 0, 0, 0, DateTimeKind.Local),
            HearingContactId = 2000,
            HearingContact = EpdContactData.GetEpdContact(2000),
            HearingDate = new DateTime(2012, 11, 15, 14, 30, 00, DateTimeKind.Local),
            HearingLocation = "efg4-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-SW-WQ-0004",
            ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "efg4-" + Guid.NewGuid(),
            SettlementAmount = 25000,
        },
        new EnforcementOrder
        {
            Id = 5,
            Cause = "fgh5-" + Guid.NewGuid(),
            CommentContactId = 2001,
            CommentContact = EpdContactData.GetEpdContact(2001),
            CommentPeriodClosesDate = DateTime.Today.AddDays(7),
            County = "Stephens",
            Deleted = false,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "fgh5-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21, 0, 0, 0, DateTimeKind.Local),
            HearingContactId = 2000,
            HearingContact = EpdContactData.GetEpdContact(2000),
            HearingDate = new DateTime(2012, 11, 15, 12, 00, 00, DateTimeKind.Local),
            HearingLocation = "fgh5-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-SW-WQ-0005",
            ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "fgh5-" + Guid.NewGuid(),
            SettlementAmount = 25000,
        },
        new EnforcementOrder
        {
            Id = 6,
            Cause = "ghi6-" + Guid.NewGuid(),
            CommentContactId = 2001,
            CommentContact = EpdContactData.GetEpdContact(2001),
            CommentPeriodClosesDate = new DateTime(2012, 11, 15, 0, 0, 0, DateTimeKind.Local),
            County = "Stephens",
            Deleted = false,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "ghi6-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21, 0, 0, 0, DateTimeKind.Local),
            HearingContactId = 2000,
            HearingContact = EpdContactData.GetEpdContact(2000),
            HearingDate = new DateTime(2012, 11, 15, 18, 30, 00, DateTimeKind.Local),
            HearingLocation = "ghi6-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 1,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(1),
            OrderNumber = "EPD-SW-WQ-0006",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16, 0, 0, 0, DateTimeKind.Local),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "ghi6-" + Guid.NewGuid(),
            SettlementAmount = 25000,
        },
        new EnforcementOrder
        {
            Id = 7,
            Cause = "hij7-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentContact = EpdContactData.GetEpdContact(2000),
            CommentPeriodClosesDate = new DateTime(2019, 03, 25, 0, 0, 0, DateTimeKind.Local),
            County = "Worth",
            Deleted = false,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "hij7-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = false,
            IsHearingScheduled = false,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-WP-0007",
            ProposedOrderPostedDate = new DateTime(2019, 03, 18, 0, 0, 0, DateTimeKind.Local),
            PublicationStatus = EnforcementOrder.PublicationState.Draft,
            Requirements = "hij7-" + Guid.NewGuid(),
            SettlementAmount = 1800,
        },
        new EnforcementOrder
        {
            Id = 11,
            Cause = "abc11-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentContact = EpdContactData.GetEpdContact(2000),
            CommentPeriodClosesDate = new DateTime(2019, 03, 25, 0, 0, 0, DateTimeKind.Local),
            County = "Appling",
            Deleted = false,
            ExecutedDate = DateTime.Today.AddDays(-14),
            ExecutedOrderPostedDate = DateTime.Today.AddDays(-12),
            FacilityName = "abc11-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = true,
            IsHearingScheduled = false,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-WP-0011",
            ProposedOrderPostedDate = new DateTime(2019, 03, 18, 0, 0, 0, DateTimeKind.Local),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "abc11-" + Guid.NewGuid(),
            SettlementAmount = 1800,
        },
        new EnforcementOrder
        {
            Id = 12,
            Cause = "bcd12-" + Guid.NewGuid(),
            CommentContactId = null,
            CommentPeriodClosesDate = null,
            County = "Butts",
            Deleted = false,
            ExecutedDate = DateTime.Today.AddDays(-14),
            ExecutedOrderPostedDate = DateUtils.MostRecentMonday(),
            FacilityName = "bcd12-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = true,
            IsHearingScheduled = false,
            IsProposedOrder = false,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-WP-0012",
            ProposedOrderPostedDate = null,
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "bcd12-" + Guid.NewGuid(),
            SettlementAmount = 1.5m,
        },
        new EnforcementOrder
        {
            Id = 13,
            Cause = "ūrbǣnitas13-" + Guid.NewGuid(),
            CommentContactId = null,
            CommentPeriodClosesDate = null,
            County = "Bulloch",
            Deleted = false,
            ExecutedDate = DateUtils.MostRecentMonday().AddDays(1),
            ExecutedOrderPostedDate = DateUtils.MostRecentMonday().AddDays(7),
            FacilityName = "ūrbǣnitas13-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = true,
            IsHearingScheduled = false,
            IsProposedOrder = false,
            LegalAuthorityId = 1,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(1),
            OrderNumber = "EPD-AQ-0013",
            ProposedOrderPostedDate = null,
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "ūrbǣnitas13-" + Guid.NewGuid(),
            SettlementAmount = 5000,
        },
        new EnforcementOrder
        {
            Id = 14,
            Cause = "efg14-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentContact = EpdContactData.GetEpdContact(2000),
            CommentPeriodClosesDate = DateTime.Today.AddDays(7),
            County = "Stephens",
            Deleted = true,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "efg14-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21, 0, 0, 0, DateTimeKind.Local),
            HearingContactId = 2000,
            HearingContact = EpdContactData.GetEpdContact(2000),
            HearingDate = new DateTime(2012, 11, 15, 18, 00, 00, DateTimeKind.Local),
            HearingLocation = "efg14-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-SW-WQ-0014",
            ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "efg14-" + Guid.NewGuid(),
            SettlementAmount = 25000,
        },
        new EnforcementOrder
        {
            Id = 15,
            Cause = "fgh15-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentContact = EpdContactData.GetEpdContact(2000),
            CommentPeriodClosesDate = DateTime.Today.AddDays(7),
            County = "Stephens",
            Deleted = false,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "fgh15-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = DateTime.Today.AddDays(3),
            HearingContactId = 2000,
            HearingContact = EpdContactData.GetEpdContact(2000),
            HearingDate = DateTime.Today.AddDays(3),
            HearingLocation = "fgh15-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-SW-WQ-0015",
            ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "fgh15-" + Guid.NewGuid(),
            SettlementAmount = 25000,
        },
        new EnforcementOrder
        {
            Id = 16,
            Cause = "ghi16-" + Guid.NewGuid(),
            CommentContactId = 2001,
            CommentContact = EpdContactData.GetEpdContact(2001),
            CommentPeriodClosesDate = new DateTime(2012, 11, 15, 0, 0, 0, DateTimeKind.Local),
            County = "Stephens",
            Deleted = false,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "ghi16-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21, 0, 0, 0, DateTimeKind.Local),
            HearingContactId = 2000,
            HearingContact = EpdContactData.GetEpdContact(2000),
            HearingDate = new DateTime(2012, 11, 15, 8, 30, 00, DateTimeKind.Local),
            HearingLocation = "ghi16-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 1,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(1),
            OrderNumber = "EPD-SW-WQ-0016",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16, 0, 0, 0, DateTimeKind.Local),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "ghi16-" + Guid.NewGuid(),
            SettlementAmount = 25000,
        },
        new EnforcementOrder
        {
            Id = 17,
            Cause = "hij17-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentContact = EpdContactData.GetEpdContact(2000),
            CommentPeriodClosesDate = new DateTime(2019, 03, 25, 0, 0, 0, DateTimeKind.Local),
            County = "Worth",
            Deleted = false,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "hij17-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = false,
            IsHearingScheduled = false,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-WP-0017",
            ProposedOrderPostedDate = new DateTime(2019, 03, 18, 0, 0, 0, DateTimeKind.Local),
            PublicationStatus = EnforcementOrder.PublicationState.Draft,
            Requirements = "hij17-" + Guid.NewGuid(),
            SettlementAmount = 1800,
        },
        new EnforcementOrder
        {
            Id = 21,
            Cause = "abc21-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentContact = EpdContactData.GetEpdContact(2000),
            CommentPeriodClosesDate = new DateTime(2019, 03, 25, 0, 0, 0, DateTimeKind.Local),
            County = "Appling",
            Deleted = false,
            ExecutedDate = DateTime.Today.AddDays(-14),
            ExecutedOrderPostedDate = DateTime.Today.AddDays(-12),
            FacilityName = "abc21-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = true,
            IsHearingScheduled = false,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-WP-0021",
            ProposedOrderPostedDate = new DateTime(2019, 03, 18, 0, 0, 0, DateTimeKind.Local),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "abc21-" + Guid.NewGuid(),
            SettlementAmount = 1800,
        },
        new EnforcementOrder
        {
            Id = 22,
            Cause = "bcd22-" + Guid.NewGuid(),
            CommentContactId = null,
            CommentPeriodClosesDate = null,
            County = "Butts",
            Deleted = false,
            ExecutedDate = DateTime.Today.AddDays(-14),
            ExecutedOrderPostedDate = DateUtils.MostRecentMonday(),
            FacilityName = "bcd22-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21, 0, 0, 0, DateTimeKind.Local),
            HearingContactId = 2000,
            HearingContact = EpdContactData.GetEpdContact(2000),
            HearingDate = DateTime.Today.AddDays(14),
            HearingLocation = "bcd22-" + Guid.NewGuid(),
            IsExecutedOrder = true,
            IsHearingScheduled = true,
            IsProposedOrder = false,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-WP-0022",
            ProposedOrderPostedDate = null,
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "bcd22-" + Guid.NewGuid(),
            SettlementAmount = 1.5m,
        },
        new EnforcementOrder
        {
            Id = 23,
            Cause = "ūrbǣnitas23-" + Guid.NewGuid(),
            CommentContactId = null,
            CommentPeriodClosesDate = null,
            County = "Bulloch",
            Deleted = false,
            ExecutedDate = DateUtils.MostRecentMonday().AddDays(1),
            ExecutedOrderPostedDate = DateUtils.MostRecentMonday().AddDays(7),
            FacilityName = "ūrbǣnitas23-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = true,
            IsHearingScheduled = false,
            IsProposedOrder = false,
            LegalAuthorityId = 1,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(1),
            OrderNumber = "EPD-AQ-0023",
            ProposedOrderPostedDate = null,
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "ūrbǣnitas23-" + Guid.NewGuid(),
            SettlementAmount = 5000,
        },
        new EnforcementOrder
        {
            Id = 24,
            Cause = "efg24-" + Guid.NewGuid(),
            CommentContactId = 2001,
            CommentContact = EpdContactData.GetEpdContact(2001),
            CommentPeriodClosesDate = DateTime.Today.AddDays(7),
            County = "Stephens",
            Deleted = true,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "efg24-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21, 0, 0, 0, DateTimeKind.Local),
            HearingContactId = 2000,
            HearingContact = EpdContactData.GetEpdContact(2000),
            HearingDate = new DateTime(2012, 11, 15, 9, 30, 00, DateTimeKind.Local),
            HearingLocation = "efg24-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-SW-WQ-0024",
            ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "efg24-" + Guid.NewGuid(),
            SettlementAmount = 25000,
        },
        new EnforcementOrder
        {
            Id = 25,
            Cause = "fgh25-" + Guid.NewGuid(),
            CommentContactId = 2001,
            CommentContact = EpdContactData.GetEpdContact(2001),
            CommentPeriodClosesDate = DateTime.Today.AddDays(7),
            County = "Stephens",
            Deleted = false,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "fgh25-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21, 0, 0, 0, DateTimeKind.Local),
            HearingContactId = 2000,
            HearingContact = EpdContactData.GetEpdContact(2000),
            HearingDate = new DateTime(2012, 11, 15, 14, 30, 00, DateTimeKind.Local),
            HearingLocation = "fgh25-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-SW-WQ-0025",
            ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "fgh25-" + Guid.NewGuid(),
            SettlementAmount = 25000,
        },
        new EnforcementOrder
        {
            Id = 26,
            Cause = "ghi26-" + Guid.NewGuid(),
            CommentContactId = 2001,
            CommentContact = EpdContactData.GetEpdContact(2001),
            CommentPeriodClosesDate = new DateTime(2012, 11, 15, 0, 0, 0, DateTimeKind.Local),
            County = "Stephens",
            Deleted = false,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "ghi26-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21, 0, 0, 0, DateTimeKind.Local),
            HearingContactId = 2000,
            HearingContact = EpdContactData.GetEpdContact(2000),
            HearingDate = new DateTime(2012, 11, 15, 14, 30, 00, DateTimeKind.Local),
            HearingLocation = "ghi26-" + Guid.NewGuid(),
            IsExecutedOrder = false,
            IsHearingScheduled = true,
            IsProposedOrder = true,
            LegalAuthorityId = 1,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(1),
            OrderNumber = "EPD-SW-WQ-0026",
            ProposedOrderPostedDate = new DateTime(2012, 10, 16, 0, 0, 0, DateTimeKind.Local),
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            Requirements = "ghi26-" + Guid.NewGuid(),
            SettlementAmount = 25000,
        },
        new EnforcementOrder
        {
            Id = 27,
            Cause = "hij27-" + Guid.NewGuid(),
            CommentContactId = 2000,
            CommentContact = EpdContactData.GetEpdContact(2000),
            CommentPeriodClosesDate = new DateTime(2019, 03, 25, 0, 0, 0, DateTimeKind.Local),
            County = "Worth",
            Deleted = false,
            ExecutedDate = null,
            ExecutedOrderPostedDate = null,
            FacilityName = "hij27-" + Guid.NewGuid(),
            HearingCommentPeriodClosesDate = null,
            HearingDate = null,
            HearingLocation = "",
            IsExecutedOrder = false,
            IsHearingScheduled = false,
            IsProposedOrder = true,
            LegalAuthorityId = 2,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(2),
            OrderNumber = "EPD-WP-0027",
            ProposedOrderPostedDate = new DateTime(2019, 03, 18, 0, 0, 0, DateTimeKind.Local),
            PublicationStatus = EnforcementOrder.PublicationState.Draft,
            Requirements = "hij27-" + Guid.NewGuid(),
            SettlementAmount = 1800,
        },
        new EnforcementOrder
        {
            Id = 28,
            CommentContactId = 2000,
            CommentContact = EpdContactData.GetEpdContact(2000),
            CommentPeriodClosesDate = new DateTime(999, 03, 25, 0, 0, 0, DateTimeKind.Local),
            Deleted = false,
            FacilityName = "Date Range Test",
            County = "",
            LegalAuthorityId = 1,
            LegalAuthority = LegalAuthorityData.GetLegalAuthority(1),
            Cause = "",
            OrderNumber = "",
            PublicationStatus = EnforcementOrder.PublicationState.Published,
            IsProposedOrder = true,
            ProposedOrderPostedDate = new DateTime(999, 2, 1, 0, 0, 0, DateTimeKind.Local),
            IsExecutedOrder = true,
            ExecutedDate = new DateTime(999, 5, 1, 0, 0, 0, DateTimeKind.Local),
            ExecutedOrderPostedDate = new DateTime(999, 5, 1, 0, 0, 0, DateTimeKind.Local),
            Requirements = "",
        },
    };
}
