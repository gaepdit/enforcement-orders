using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Enfo.Domain.Entities;
using Enfo.Infrastructure.Contexts;
using Enfo.Repository.Utils;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Enfo.WebApp.Services
{
    public static class TempData
    {
        public static async Task SeedTempDataAsync([NotNull] this EnfoDbContext context,
            CancellationToken cancellationToken)
        {
            await context.Database.OpenConnectionAsync(cancellationToken);

            await context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT dbo.{nameof(context.Addresses)} ON", cancellationToken);
            if (!await context.Addresses.AnyAsync(cancellationToken))
                await context.Addresses.AddRangeAsync(GetAddresses, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            await context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT dbo.{nameof(context.Addresses)} OFF", cancellationToken);

            await context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT dbo.{nameof(context.LegalAuthorities)} ON", cancellationToken);
            if (!await context.LegalAuthorities.AnyAsync(cancellationToken))
                await context.LegalAuthorities.AddRangeAsync(GetLegalAuthorities, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            await context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT dbo.{nameof(context.LegalAuthorities)} OFF", cancellationToken);

            await context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT dbo.{nameof(context.EpdContacts)} ON", cancellationToken);
            if (!await context.EpdContacts.AnyAsync(cancellationToken))
                await context.EpdContacts.AddRangeAsync(GetEpdContacts, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            await context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT dbo.{nameof(context.EpdContacts)} OFF", cancellationToken);

            await context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT dbo.{nameof(context.EnforcementOrders)} ON", cancellationToken);
            if (!await context.EnforcementOrders.AnyAsync(cancellationToken))
                await context.EnforcementOrders.AddRangeAsync(GetEnforcementOrders, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            await context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT dbo.{nameof(context.EnforcementOrders)} OFF", cancellationToken);

            await context.Database.CloseConnectionAsync();
        }

        private static readonly IEnumerable<Address> GetAddresses = new List<Address>
        {
            new()
            {
                Id = 2000,
                Active = true,
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
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "000 Obsolete Address",
            },
            new()
            {
                Id = 2002,
                Active = true,
                City = "Atlanta",
                PostalCode = "30000",
                State = "GA",
                Street = "123 New Street",
            },
        };

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
                AddressId = 2000,
                ContactName = "A. Jones",
                Email = "example@example.com",
                Organization = "Environmental Protection Division",
                Telephone = "555-1212",
                Title = "Chief, Air Protection Branch",
            },
            new()
            {
                Id = 2001,
                Active = false,
                AddressId = 2001,
                ContactName = "B. Smith",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Land Protection Branch",
            },
            new()
            {
                Id = 2002,
                Active = true,
                AddressId = 2001,
                ContactName = "B. Smith",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Land Protection Branch",
            },
        };

        private static readonly IEnumerable<EnforcementOrder> GetEnforcementOrders = new List<EnforcementOrder>
        {
            new()
            {
                Id = 1,
                Cause = "abc-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Appling",
                Deleted = false,
                ExecutedDate = DateTime.Today.AddDays(-14),
                ExecutedOrderPostedDate = DateTime.Today.AddDays(-12),
                FacilityName = "abc-" + Guid.NewGuid(),
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
                Requirements = "abc-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
            new()
            {
                Id = 2,
                Cause = "bcd-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Butts",
                Deleted = false,
                ExecutedDate = DateTime.Today.AddDays(-14),
                ExecutedOrderPostedDate = DateUtils.MostRecentMonday(),
                FacilityName = "bcd-" + Guid.NewGuid(),
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
                Requirements = "bcd-" + Guid.NewGuid(),
                SettlementAmount = 1.5m
            },
            new()
            {
                Id = 3,
                Cause = "ūrbǣnitas-" + Guid.NewGuid(),
                CommentContactId = 2000,
                CommentPeriodClosesDate = null,
                County = "Bulloch",
                Deleted = false,
                ExecutedDate = DateUtils.MostRecentMonday().AddDays(1),
                ExecutedOrderPostedDate = DateUtils.MostRecentMonday().AddDays(7),
                FacilityName = "ūrbǣnitas-" + Guid.NewGuid(),
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
                Requirements = "ūrbǣnitas-" + Guid.NewGuid(),
                SettlementAmount = 5000
            },
            new()
            {
                Id = 4,
                Cause = "efg-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = true,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "efg-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 14, 30, 00),
                HearingLocation = "efg-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0004",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "efg-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 5,
                Cause = "fgh-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "fgh-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 12, 00, 00),
                HearingLocation = "fgh-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0005",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "fgh-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 6,
                Cause = "ghi-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = new DateTime(2012, 11, 15),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "ghi-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 18, 30, 00),
                HearingLocation = "ghi-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 1,
                OrderNumber = "EPD-SW-WQ-0006",
                ProposedOrderPostedDate = new DateTime(2012, 10, 16),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "ghi-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 7,
                Cause = "hij-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Worth",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "hij-" + Guid.NewGuid(),
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
                Requirements = "hij-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
            new()
            {
                Id = 11,
                Cause = "abc-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Appling",
                Deleted = false,
                ExecutedDate = DateTime.Today.AddDays(-14),
                ExecutedOrderPostedDate = DateTime.Today.AddDays(-12),
                FacilityName = "abc-" + Guid.NewGuid(),
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
                Requirements = "abc-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
            new()
            {
                Id = 12,
                Cause = "bcd-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Butts",
                Deleted = false,
                ExecutedDate = DateTime.Today.AddDays(-14),
                ExecutedOrderPostedDate = DateUtils.MostRecentMonday(),
                FacilityName = "bcd-" + Guid.NewGuid(),
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
                Requirements = "bcd-" + Guid.NewGuid(),
                SettlementAmount = 1.5m
            },
            new()
            {
                Id = 13,
                Cause = "ūrbǣnitas-" + Guid.NewGuid(),
                CommentContactId = 2000,
                CommentPeriodClosesDate = null,
                County = "Bulloch",
                Deleted = false,
                ExecutedDate = DateUtils.MostRecentMonday().AddDays(1),
                ExecutedOrderPostedDate = DateUtils.MostRecentMonday().AddDays(7),
                FacilityName = "ūrbǣnitas-" + Guid.NewGuid(),
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
                Requirements = "ūrbǣnitas-" + Guid.NewGuid(),
                SettlementAmount = 5000
            },
            new()
            {
                Id = 14,
                Cause = "efg-" + Guid.NewGuid(),
                CommentContactId = 2000,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = true,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "efg-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 18, 00, 00),
                HearingLocation = "efg-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0014",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "efg-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 15,
                Cause = "fgh-" + Guid.NewGuid(),
                CommentContactId = 2000,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "fgh-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = DateTime.Today.AddDays(3),
                HearingContactId = 2000,
                HearingDate = DateTime.Today.AddDays(3),
                HearingLocation = "fgh-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0015",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "fgh-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 16,
                Cause = "ghi-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = new DateTime(2012, 11, 15),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "ghi-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 8, 30, 00),
                HearingLocation = "ghi-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 1,
                OrderNumber = "EPD-SW-WQ-0016",
                ProposedOrderPostedDate = new DateTime(2012, 10, 16),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "ghi-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 17,
                Cause = "hij-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Worth",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "hij-" + Guid.NewGuid(),
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
                Requirements = "hij-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
            new()
            {
                Id = 21,
                Cause = "abc-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Appling",
                Deleted = false,
                ExecutedDate = DateTime.Today.AddDays(-14),
                ExecutedOrderPostedDate = DateTime.Today.AddDays(-12),
                FacilityName = "abc-" + Guid.NewGuid(),
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
                Requirements = "abc-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
            new()
            {
                Id = 22,
                Cause = "bcd-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Butts",
                Deleted = false,
                ExecutedDate = DateTime.Today.AddDays(-14),
                ExecutedOrderPostedDate = DateUtils.MostRecentMonday(),
                FacilityName = "bcd-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                IsProposedOrder = false,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-WP-0022",
                ProposedOrderPostedDate = null,
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "bcd-" + Guid.NewGuid(),
                SettlementAmount = 1.5m
            },
            new()
            {
                Id = 23,
                Cause = "ūrbǣnitas-" + Guid.NewGuid(),
                CommentContactId = 2000,
                CommentPeriodClosesDate = null,
                County = "Bulloch",
                Deleted = false,
                ExecutedDate = DateUtils.MostRecentMonday().AddDays(1),
                ExecutedOrderPostedDate = DateUtils.MostRecentMonday().AddDays(7),
                FacilityName = "ūrbǣnitas-" + Guid.NewGuid(),
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
                Requirements = "ūrbǣnitas-" + Guid.NewGuid(),
                SettlementAmount = 5000
            },
            new()
            {
                Id = 24,
                Cause = "efg-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = true,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "efg-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 9, 30, 00),
                HearingLocation = "efg-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0024",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "efg-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 25,
                Cause = "fgh-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "fgh-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 14, 30, 00),
                HearingLocation = "fgh-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-0025",
                ProposedOrderPostedDate = DateTime.Today.AddDays(-7),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "fgh-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 26,
                Cause = "ghi-" + Guid.NewGuid(),
                CommentContactId = 2001,
                CommentPeriodClosesDate = new DateTime(2012, 11, 15),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "ghi-" + Guid.NewGuid(),
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15, 14, 30, 00),
                HearingLocation = "ghi-" + Guid.NewGuid(),
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 1,
                OrderNumber = "EPD-SW-WQ-0026",
                ProposedOrderPostedDate = new DateTime(2012, 10, 16),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements = "ghi-" + Guid.NewGuid(),
                SettlementAmount = 25000
            },
            new()
            {
                Id = 27,
                Cause = "hij-" + Guid.NewGuid(),
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Worth",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "hij-" + Guid.NewGuid(),
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
                Requirements = "hij-" + Guid.NewGuid(),
                SettlementAmount = 1800
            },
        };
    }
}