﻿using System;
using System.Collections.Generic;
using Enfo.Domain.Entities;

namespace Enfo.Infrastructure.Tests
{
    public static class RepositoryHelperData
    {
        public static IEnumerable<Address> GetAddresses = new List<Address>
        {
            new Address
            {
                Id = 2000,
                Active = true,
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "4244 International Parkway",
                Street2 = "Suite 120",
            },
            new Address
            {
                Id = 2001,
                Active = false,
                City = "Atlanta",
                PostalCode = "30354",
                State = "GA",
                Street = "4244 International Parkway",
                Street2 = "Suite 104",
            },
        };

        public static IEnumerable<LegalAuthority> GetLegalAuthorities = new List<LegalAuthority>
        {
            new LegalAuthority {Id = 1, Active = true, AuthorityName = "Air Quality Act",},
            new LegalAuthority {Id = 2, Active = true, AuthorityName = "Asbestos Safety Act",},
        };

        public static IEnumerable<EpdContact> GetEpdContacts = new List<EpdContact>
        {
            new EpdContact
            {
                Id = 2000,
                Active = true,
                AddressId = 2000,
                ContactName = "A. Jones",
                Email = null,
                Organization = "Environmental Protection Division",
                Telephone = null,
                Title = "Chief, Air Protection Branch",
            },
            new EpdContact
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
        };

        public static List<EnforcementOrder> GetEnforcementOrders = new List<EnforcementOrder>
        {
            new EnforcementOrder
            {
                Id = 27,
                Cause = "Nulla pellentesque dignissim enim sit.",
                CommentContactId = 2000,
                CommentPeriodClosesDate = null,
                County = "Bulloch",
                Deleted = false,
                ExecutedDate = new DateTime(1998, 06, 10),
                ExecutedOrderPostedDate = new DateTime(1998, 06, 15),
                FacilityName = "Diam donec adipiscing",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                IsProposedOrder = false,
                LegalAuthorityId = 1,
                OrderNumber = "EPD-AQ-17310",
                ProposedOrderPostedDate = null,
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements =
                    "Ið velit meƿÞitum qui. Ei nām quaƿdo fāċilisi? Homerō nomiƿati id sit, sea tibique lūpÞǣtum āð. Ei porro nulla soleat mei, vīx " +
                    "oȝlīque ūrbǣnitas id, pro te commune scrīptorem? Fæcilis quǣēstiō has eǽ, id tinċiduƿÞ perseqūeris prō! Nam āccumsǣn forensibus cotidīēqūe cu.",
                SettlementAmount = 5000
            },
            new EnforcementOrder
            {
                Id = 58310,
                Cause = "Mollis nunc sed id semper.",
                CommentContactId = 2001,
                CommentPeriodClosesDate = new DateTime(2012, 11, 15),
                County = "Stephens",
                Deleted = true,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "Tempor orci dapibus",
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15),
                HearingLocation = "venenatis urna cursus eget nunc scelerisque viverra mauris in aliquam sem",
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-SW-WQ-111",
                ProposedOrderPostedDate = new DateTime(2012, 10, 16),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements =
                    "Suspendisse potenti nullam ac tortor vitae purus faucibus ornare suspendisse. Magna fermentum iaculis eu non diam phasellus " +
                    "vestibulum. Imperdiet proin fermentum leo vel. Et ligula ullamcorper malesuada proin libero nunc consequat interdum varius. Neque gravida " +
                    "in fermentum et sollicitudin ac orci phasellus. Nunc sed blandit libero volutpat. Nisl rhoncus mattis rhoncus urna. Varius sit amet mattis " +
                    "vulputate enim nulla aliquet porttitor. Consectetur adipiscing elit pellentesque habitant morbi tristique. Ante metus dictum at tempor commodo " +
                    "ullamcorper a. Morbi tristique senectus et netus et malesuada fames ac turpis. Velit ut tortor pretium viverra suspendisse potenti. In arcu " +
                    "cursus euismod quis. Nulla malesuada pellentesque elit eget gravida cum sociis natoque penatibus. Mattis rhoncus urna neque viverra justo nec " +
                    "ultrices dui.",
                SettlementAmount = 25000
            },
            new EnforcementOrder
            {
                Id = 58312,
                Cause = "Mollis nunc sed id semper.",
                CommentContactId = 2001,
                CommentPeriodClosesDate = new DateTime(2012, 11, 15),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "Tempor orci dapibus",
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2000,
                HearingDate = new DateTime(2012, 11, 15),
                HearingLocation = "venenatis urna cursus eget nunc scelerisque viverra mauris in aliquam sem",
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 1,
                OrderNumber = "EPD-SW-WQ-112",
                ProposedOrderPostedDate = new DateTime(2012, 10, 16),
                PublicationStatus = EnforcementOrder.PublicationState.Published,
                Requirements =
                    "Suspendisse potenti nullam ac tortor vitae purus faucibus ornare suspendisse. Magna fermentum iaculis eu non diam phasellus " +
                    "vestibulum. Imperdiet proin fermentum leo vel. Et ligula ullamcorper malesuada proin libero nunc consequat interdum varius. Neque gravida " +
                    "in fermentum et sollicitudin ac orci phasellus. Nunc sed blandit libero volutpat. Nisl rhoncus mattis rhoncus urna. Varius sit amet mattis " +
                    "vulputate enim nulla aliquet porttitor. Consectetur adipiscing elit pellentesque habitant morbi tristique. Ante metus dictum at tempor " +
                    "commodo ullamcorper a. Morbi tristique senectus et netus et malesuada fames ac turpis. Velit ut tortor pretium viverra suspendisse potenti. " +
                    "In arcu cursus euismod quis. Nulla malesuada pellentesque elit eget gravida cum sociis natoque penatibus. Mattis rhoncus urna neque viverra " +
                    "justo nec ultrices dui.",
                SettlementAmount = 25000
            },
            new EnforcementOrder
            {
                Id = 71625,
                Cause = "Arcu non odio euismod lacinia at quis risus.",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Fannin",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "Odio tempor orci",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = false,
                IsHearingScheduled = false,
                IsProposedOrder = true,
                LegalAuthorityId = 2,
                OrderNumber = "EPD-WP-3854",
                ProposedOrderPostedDate = new DateTime(2019, 03, 18),
                PublicationStatus = EnforcementOrder.PublicationState.Draft,
                Requirements =
                    "Vitae sapien pellentesque habitant morbi tristique. Felis bibendum ut tristique et egestas quis. In aliquam sem fringilla " +
                    "ut morbi. Amet purus gravida quis blandit turpis. Et magnis dis parturient montes nascetur ridiculus. Diam vel quam elementum pulvinar " +
                    "etiam non quam. Varius sit amet mattis vulputate. Semper quis lectus nulla at volutpat diam ut. Aliquam purus sit amet luctus venenatis. " +
                    "Semper viverra nam libero justo laoreet sit amet. Eget mauris pharetra et ultrices. Quisque non tellus orci ac auctor augue mauris augue. " +
                    "Massa enim nec dui nunc. Pulvinar etiam non quam lacus suspendisse faucibus interdum posuere lorem. Neque sodales ut etiam sit amet nisl purus.",
                SettlementAmount = 1800
            },
        };
    }
}