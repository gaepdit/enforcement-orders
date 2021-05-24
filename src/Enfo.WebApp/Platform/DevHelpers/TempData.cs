using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Domain.Utils;
using Enfo.Infrastructure.Contexts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

// ReSharper disable StringLiteralTypo

namespace Enfo.WebApp.Platform.DevHelpers
{
    public static class TempData
    {
        public static async Task SeedTempDataAsync([NotNull] this EnfoDbContext c, CancellationToken t)
        {
            await c.Database.OpenConnectionAsync(t);

            if (!await c.LegalAuthorities.AnyAsync(t))
            {
                await c.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{nameof(c.LegalAuthorities)} ON", t);
                await c.LegalAuthorities.AddRangeAsync(GetLegalAuthorities, t);
                await c.SaveChangesAsync(t);
                await c.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{nameof(c.LegalAuthorities)} OFF", t);
            }

            if (!await c.EpdContacts.AnyAsync(t))
            {
                await c.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{nameof(c.EpdContacts)} ON", t);
                await c.EpdContacts.AddRangeAsync(GetEpdContacts, t);
                await c.SaveChangesAsync(t);
                await c.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{nameof(c.EpdContacts)} OFF", t);
            }

            if (!await c.EnforcementOrders.AnyAsync(t))
            {
                await c.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{nameof(c.EnforcementOrders)} ON", t);
                await c.EnforcementOrders.AddRangeAsync(GetEnforcementOrders, t);
                await c.SaveChangesAsync(t);
                await c.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{nameof(c.EnforcementOrders)} OFF", t);
            }

            await c.Database.CloseConnectionAsync();
        }

        private static readonly IEnumerable<LegalAuthority> GetLegalAuthorities = new List<LegalAuthority>
        {
            new() {Id = 1, Active = true, AuthorityName = "Air Quality Act",},
            new() {Id = 2, Active = true, AuthorityName = "Asbestos Safety Act",},
            new() {Id = 3, Active = false, AuthorityName = "Obsolete Act",},
        };

        private static readonly IEnumerable<EpdContact> GetEpdContacts = new List<EpdContact>
        {
            new()
            {
                Id = 2000,
                Active = true,
                ContactName = "A. Jones",
                Email = "example@example.com",
                Organization = "Environmental Protection Division",
                Telephone = "555-1212",
                Title = "Chief, Air Protection Branch",
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "4244 International Parkway",
                Street2 = "Suite 120",
            },
            new()
            {
                Id = 2001,
                Active = false,
                ContactName = "B. Smith",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Land Protection Branch",
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "4244 International Parkway",
                Street2 = "Suite 120",
            },
            new()
            {
                Id = 2002,
                Active = true,
                ContactName = "B. Smith",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Land Protection Branch",
                City = "Atlanta",
                PostalCode = "30000",
                State = "GA",
                Street = "123 New Street",
            },
        };

        private static readonly IEnumerable<EnforcementOrder> GetEnforcementOrders = new List<EnforcementOrder>
        {
            new()
            {
                Id = 1,
                Cause = "abc1-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
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
                OrderNumber = "EPD-WP-0001",
                ProposedOrderPostedDate = new DateTime(2019, 03, 18),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "abc1-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
            new()
            {
                Id = 2,
                Cause = "bcd2-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Butts",
                Deleted = false,
                ExecutedDate = DateTime.Today.AddDays(-14),
                ExecutedOrderPostedDate = DateUtils.MostRecentMonday(),
                FacilityName = "bcd2-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                IsProposedOrder = false,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-WP-0002",
                ProposedOrderPostedDate = null,
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "bcd2-" + Guid.NewGuid(),
                SettlementAmount = 1.5m
            },
            new()
            {
                Id = 3,
                Cause = "ūrbǣnitas3-" + Guid.NewGuid(),
                CommentContactId = 2000,
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
                OrderNumber = "EPD-AQ-0003",
                ProposedOrderPostedDate = null,
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "ūrbǣnitas3-" + Guid.NewGuid(),
                SettlementAmount = 5000
            },
            new()
            {
                Id = 4,
                Cause = "efg4-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = true,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "efg4-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 14, 30, 00),
                HearingLocation = "efg4-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0004",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "efg4-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 5,
                Cause = "fgh5-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "fgh5-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 12, 00, 00),
                HearingLocation = "fgh5-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0005",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "fgh5-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 6,
                Cause = "ghi6-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = new DateTime(2012, 11, 15),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "ghi6-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 18, 30, 00),
                HearingLocation = "ghi6-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 1,
                OrderNumber = "EPD-SW-WQ-0006",
                ProposedOrderPostedDate = new DateTime(2012, 10, 16),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "ghi6-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 7,
                Cause = "hij7-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
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
                OrderNumber = "EPD-WP-0007",
                ProposedOrderPostedDate = new DateTime(2019, 03, 18),
                PublicationStatus = EnforcementOrder.PublicationState.Draft,
                Requirements = "hij7-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
            new()
            {
                Id = 11,
                Cause = "abc11-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
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
                OrderNumber = "EPD-WP-0011",
                ProposedOrderPostedDate = new DateTime(2019, 03, 18),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "abc11-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
            new()
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
                OrderNumber = "EPD-WP-0012",
                ProposedOrderPostedDate = null,
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "bcd12-" + Guid.NewGuid(),
                SettlementAmount = 1.5m
            },
            new()
            {
                Id = 13,
                Cause = "ūrbǣnitas13-" + Guid.NewGuid(),
                CommentContactId = 2000,
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
                OrderNumber = "EPD-AQ-0013",
                ProposedOrderPostedDate = null,
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "ūrbǣnitas13-" + Guid.NewGuid(),
                SettlementAmount = 5000
            },
            new()
            {
                Id = 14,
                Cause = "efg14-" + Guid.NewGuid(),
                CommentContactId = 2000,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = true,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "efg14-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 18, 00, 00),
                HearingLocation = "efg14-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0014",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "efg14-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 15,
                Cause = "fgh15-" + Guid.NewGuid(),
                CommentContactId = 2000,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "fgh15-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = DateTime.Today.AddDays(3),
                HearingContactId = 2000,
                HearingDate = DateTime.Today.AddDays(3),
                HearingLocation = "fgh15-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0015",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "fgh15-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 16,
                Cause = "ghi16-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = new DateTime(2012, 11, 15),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "ghi16-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 8, 30, 00),
                HearingLocation = "ghi16-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 1,
                OrderNumber = "EPD-SW-WQ-0016",
                ProposedOrderPostedDate = new DateTime(2012, 10, 16),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "ghi16-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 17,
                Cause = "hij17-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
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
                OrderNumber = "EPD-WP-0017",
                ProposedOrderPostedDate = new DateTime(2019, 03, 18),
                PublicationStatus = EnforcementOrder.PublicationState.Draft,
                Requirements = "hij17-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
            new()
            {
                Id = 21,
                Cause = "abc21-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
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
                OrderNumber = "EPD-WP-0021",
                ProposedOrderPostedDate = new DateTime(2019, 03, 18),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "abc21-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
            new()
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
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = DateTime.Today.AddDays(14),
                HearingLocation = "bcd22-" + Guid.NewGuid(),
                IsExecutedOrder = true,
                IsHearingScheduled = true,
                IsProposedOrder = false,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-WP-0022",
                ProposedOrderPostedDate = null,
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "bcd22-" + Guid.NewGuid(),
                SettlementAmount = 1.5m
            },
            new()
            {
                Id = 23,
                Cause = "ūrbǣnitas23-" + Guid.NewGuid(),
                CommentContactId = 2000,
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
                OrderNumber = "EPD-AQ-0023",
                ProposedOrderPostedDate = null,
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "ūrbǣnitas23-" + Guid.NewGuid(),
                SettlementAmount = 5000
            },
            new()
            {
                Id = 24,
                Cause = "efg24-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = true,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "efg24-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 9, 30, 00),
                HearingLocation = "efg24-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0024",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "efg24-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 25,
                Cause = "fgh25-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "fgh25-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 14, 30, 00),
                HearingLocation = "fgh25-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0025",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "fgh25-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 26,
                Cause = "ghi26-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = new DateTime(2012, 11, 15),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "ghi26-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 14, 30, 00),
                HearingLocation = "ghi26-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 1,
                OrderNumber = "EPD-SW-WQ-0026",
                ProposedOrderPostedDate = new DateTime(2012, 10, 16),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "ghi26-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 27,
                Cause = "hij27-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
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
                OrderNumber = "EPD-WP-0027",
                ProposedOrderPostedDate = new DateTime(2019, 03, 18),
                PublicationStatus = EnforcementOrder.PublicationState.Draft,
                Requirements = "hij27-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
        };
    }
}