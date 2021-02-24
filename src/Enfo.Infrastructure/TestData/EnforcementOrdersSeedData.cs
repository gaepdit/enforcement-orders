﻿using System;
using System.Collections.Generic;
using Enfo.Domain.Entities;
using static Enfo.Domain.Entities.EnforcementOrder;

namespace Enfo.Infrastructure.TestData
{
    public static partial class TestData
    {
        public static IEnumerable<EnforcementOrder> GetEnforcementOrders() => new List<EnforcementOrder>
        {
            new EnforcementOrder
            {
                Id = 140,
                Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Liberty",
                Deleted = false,
                ExecutedDate = new DateTime(1998, 06, 29),
                ExecutedOrderPostedDate = new DateTime(1998, 07, 06),
                FacilityName = "A diam maecenas",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                IsProposedOrder = false,
                LegalAuthorityId = 7,
                OrderNumber = "EPD-ACQ-7936",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis " +
                    "parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus " +
                    "sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed " +
                    "viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet " +
                    "porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est " +
                    "ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. " +
                    "Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi." +
                    Environment.NewLine +
                    "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis " +
                    "parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus " +
                    "sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed " +
                    "viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet " +
                    "porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est " +
                    "ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. " +
                    "Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
                SettlementAmount = 2000
            },
            new EnforcementOrder
            {
                Id = 141,
                Cause = "Integer feugiat scelerisque varius morbi enim nunc faucibus a.",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Liberty",
                Deleted = false,
                ExecutedDate = new DateTime(1999, 06, 29),
                ExecutedOrderPostedDate = new DateTime(1999, 07, 06),
                FacilityName = "A diam maecenas",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                IsProposedOrder = false,
                LegalAuthorityId = 7,
                OrderNumber = "EPD-ACQ-7937",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis " +
                    "parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus " +
                    "sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed " +
                    "viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet " +
                    "porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est " +
                    "ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. " +
                    "Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi." +
                    Environment.NewLine +
                    "Duis ut diam quam nulla porttitor massa id neque. A lacus vestibulum sed arcu non. Amet massa vitae tortor condimentum. Magnis dis " +
                    "parturient montes nascetur ridiculus mus mauris. Arcu risus quis varius quam quisque id diam. Pellentesque massa placerat duis ultricies lacus " +
                    "sed. Tellus in hac habitasse platea dictumst vestibulum. Justo nec ultrices dui sapien eget. Ac odio tempor orci dapibus ultrices in. Lacus sed " +
                    "viverra tellus in hac habitasse platea dictumst vestibulum. Donec et odio pellentesque diam volutpat. Nunc faucibus a pellentesque sit amet " +
                    "porttitor eget dolor morbi. Neque ornare aenean euismod elementum nisi quis eleifend quam. Praesent elementum facilisis leo vel fringilla est " +
                    "ullamcorper eget. Et netus et malesuada fames. Urna et pharetra pharetra massa massa ultricies mi quis. Sit amet consectetur adipiscing elit. " +
                    "Felis donec et odio pellentesque diam volutpat commodo sed egestas. Adipiscing elit pellentesque habitant morbi.",
                SettlementAmount = 2000
            },
            new EnforcementOrder
            {
                Id = 27,
                Cause = "Nulla pellentesque dignissim enim sit.",
                CommentContactId = null,
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
                PublicationStatus = PublicationState.Published,
                Requirements = "Ið velit meƿÞitum qui. Ei nām quaƿdo fāċilisi? Homerō nomiƿati id sit, sea tibique lūpÞǣtum āð. Ei porro nulla soleat mei, vīx " +
                    "oȝlīque ūrbǣnitas id, pro te commune scrīptorem? Fæcilis quǣēstiō has eǽ, id tinċiduƿÞ perseqūeris prō! Nam āccumsǣn forensibus cotidīēqūe cu.",
                SettlementAmount = 5000
            },
            new EnforcementOrder
            {
                Id = 58310,
                Cause = "Mollis nunc sed id semper.",
                CommentContactId = 2004,
                CommentPeriodClosesDate = new DateTime(2012, 11, 15),
                County = "Stephens",
                Deleted = true,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "Tempor orci dapibus",
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2004,
                HearingDate = new DateTime(2012, 11, 15),
                HearingLocation = "venenatis urna cursus eget nunc scelerisque viverra mauris in aliquam sem",
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 8,
                OrderNumber = "EPD-SW-WQ-111",
                ProposedOrderPostedDate = new DateTime(2012, 10, 16),
                PublicationStatus = PublicationState.Published,
                Requirements = "Suspendisse potenti nullam ac tortor vitae purus faucibus ornare suspendisse. Magna fermentum iaculis eu non diam phasellus " +
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
                CommentContactId = 2004,
                CommentPeriodClosesDate = new DateTime(2012, 11, 15),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "Tempor orci dapibus",
                HearingCommentPeriodClosesDate = new DateTime(2012, 11, 21),
                HearingContactId = 2004,
                HearingDate = new DateTime(2012, 11, 15),
                HearingLocation = "venenatis urna cursus eget nunc scelerisque viverra mauris in aliquam sem",
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 8,
                OrderNumber = "EPD-SW-WQ-112",
                ProposedOrderPostedDate = new DateTime(2012, 10, 16),
                PublicationStatus = PublicationState.Published,
                Requirements = "Suspendisse potenti nullam ac tortor vitae purus faucibus ornare suspendisse. Magna fermentum iaculis eu non diam phasellus " +
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
                Id = 58313,
                Cause = "Mollis nunc sed id semper.",
                CommentContactId = 2004,
                CommentPeriodClosesDate = new DateTime(2013, 11, 15),
                County = "Stephens",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "Tempor orci dapibus",
                HearingCommentPeriodClosesDate = new DateTime(2013, 11, 21),
                HearingContactId = 2004,
                HearingDate = new DateTime(2013, 11, 15),
                HearingLocation = "venenatis urna cursus eget nunc scelerisque viverra mauris in aliquam sem",
                IsExecutedOrder = false,
                IsHearingScheduled = true,
                IsProposedOrder = true,
                LegalAuthorityId = 8,
                OrderNumber = "EPD-SW-WQ-113",
                ProposedOrderPostedDate = new DateTime(2013, 10, 16),
                PublicationStatus = PublicationState.Published,
                Requirements = "Suspendisse potenti nullam ac tortor vitae purus faucibus ornare suspendisse. Magna fermentum iaculis eu non diam phasellus " +
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
                Id = 70789,
                Cause = "Vitae turpis massa sed elementum tempus egestas sed sed.",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Cherokee",
                Deleted = true,
                ExecutedDate = new DateTime(2017, 06, 20),
                ExecutedOrderPostedDate = new DateTime(2017, 06, 26),
                FacilityName = "Congue mauris rhoncus",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                IsProposedOrder = false,
                LegalAuthorityId = 8,
                OrderNumber = "EPD-WP-4858",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Tortor ac id dolor 👾🔙🎄📩🐱💖🐸🌷🌅👺 mi auctor nunc. Cursus 👬📰🔖👐📁🐭🌻 gravida eget est 👻🍕🕖🕟📵💬🍩 imperdiet 👕🍎👢🌇🎤👡👝🌞🕗👒🌺 " +
                    "ut urna magnis aliquam vitae 💗🔙🐊💤🎶🌖🍦🐄👧🍼🏧🐬🍹🍁 mi quam at 🕗📗👝🐕💙💜🕧🎈👌🔨💯🍴🔢🕟👽 tortor, sed 🔡🐸📩🌚🏯💊🔫🔬.",
                SettlementAmount = 13375
            },
            new EnforcementOrder
            {
                Id = 70940,
                Cause = "Morbi leo urna molestie at elementum.",
                CommentContactId = 2010,
                CommentPeriodClosesDate = new DateTime(2018, 01, 17),
                County = "Cherokee",
                Deleted = true,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "Velit egestas dui",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = false,
                IsHearingScheduled = false,
                IsProposedOrder = true,
                LegalAuthorityId = 8,
                OrderNumber = "EPD-WP-15472",
                ProposedOrderPostedDate = new DateTime(2017, 12, 18),
                PublicationStatus = PublicationState.Published,
                Requirements = "Mauris in aliquam sem fringilla ut morbi tincidunt augue interdum. Cursus risus at ultrices mi tempus imperdiet." + 
                    Environment.NewLine +
                    "Imperdiet sed euismod nisi porta lorem mollis aliquam. Arcu ac tortor dignissim convallis aenean. Ultrices vitae auctor eu augue ut. " +
                    "Velit dignissim sodales ut eu sem integer vitae justo eget. Curabitur gravida arcu ac tortor dignissim convallis aenean et tortor. " +
                    "Blandit massa enim nec dui nunc mattis. Laoreet suspendisse interdum consectetur libero id faucibus. Mattis vulputate enim nulla aliquet " +
                    "porttitor lacus luctus. In cursus turpis massa tincidunt. Massa enim nec dui nunc mattis enim. Iaculis eu non diam phasellus vestibulum. " +
                    "Leo duis ut diam quam nulla. Facilisis gravida neque convallis a cras semper auctor neque vitae. Dignissim sodales ut eu sem. Erat " +
                    "pellentesque adipiscing commodo elit at.",
                SettlementAmount = 7500
            },
            new EnforcementOrder
            {
                Id = 71580,
                Cause = "Amet cursus sit amet dictum sit amet justo donec enim.",
                CommentContactId = 2013,
                CommentPeriodClosesDate = new DateTime(2019, 04, 03),
                County = "Catoosa",
                Deleted = false,
                ExecutedDate = new DateTime(2019, 04, 11),
                ExecutedOrderPostedDate = new DateTime(2019, 04, 15),
                FacilityName = "Ut sem nulla",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                IsProposedOrder = true,
                LegalAuthorityId = 8,
                OrderNumber = "EPD-WP-17456",
                ProposedOrderPostedDate = new DateTime(2019, 03, 04),
                PublicationStatus = PublicationState.Published,
                Requirements = "Tincidunt arcu non sodales neque sodales ut. Ultricies mi quis hendrerit dolor magna eget est lorem. Ac placerat vestibulum " +
                    "lectus mauris ultrices. Egestas quis ipsum suspendisse ultrices. Morbi tristique senectus et netus et malesuada fames. Facilisis volutpat " +
                    "est velit egestas dui id ornare. Pellentesque adipiscing commodo elit at imperdiet dui. Ut ornare lectus sit amet est placerat in. Eu " +
                    "facilisis sed odio morbi quis commodo odio. Sapien nec sagittis aliquam malesuada bibendum arcu vitae elementum curabitur. Ac tortor " +
                    "dignissim convallis aenean et. Tristique risus nec feugiat in fermentum posuere urna. Est ullamcorper eget nulla facilisi etiam dignissim " +
                    "diam quis enim.",
                SettlementAmount = 2375
            },
            new EnforcementOrder
            {
                Id = 71625,
                Cause = "Arcu non odio euismod lacinia at quis risus.",
                CommentContactId = 2013,
                CommentPeriodClosesDate = new DateTime(2019, 04, 17),
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
                LegalAuthorityId = 8,
                OrderNumber = "EPD-WP-3854",
                ProposedOrderPostedDate = new DateTime(2019, 03, 18),
                PublicationStatus = PublicationState.Published,
                Requirements = "Vitae sapien pellentesque habitant morbi tristique. Felis bibendum ut tristique et egestas quis. In aliquam sem fringilla " +
                    "ut morbi. Amet purus gravida quis blandit turpis. Et magnis dis parturient montes nascetur ridiculus. Diam vel quam elementum pulvinar " +
                    "etiam non quam. Varius sit amet mattis vulputate. Semper quis lectus nulla at volutpat diam ut. Aliquam purus sit amet luctus venenatis. " +
                    "Semper viverra nam libero justo laoreet sit amet. Eget mauris pharetra et ultrices. Quisque non tellus orci ac auctor augue mauris augue. " +
                    "Massa enim nec dui nunc. Pulvinar etiam non quam lacus suspendisse faucibus interdum posuere lorem. Neque sodales ut etiam sit amet nisl purus.",
                SettlementAmount = 1800
            },
            new EnforcementOrder
            {
                Id = 71689,
                Cause = "Netus et malesuada fames ac.",
                CommentContactId = 2013,
                CommentPeriodClosesDate = DateTime.Today.AddDays(7),
                County = "Walker",
                Deleted = false,
                ExecutedDate = null,
                ExecutedOrderPostedDate = null,
                FacilityName = "Netus et malesuada",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = false,
                IsHearingScheduled = false,
                IsProposedOrder = true,
                LegalAuthorityId = 8,
                OrderNumber = "EPD-WP-19409",
                ProposedOrderPostedDate = new DateTime(2019, 04, 15),
                PublicationStatus = PublicationState.Published,
                Requirements = "In cursus turpis massa tincidunt dui ut ornare. Augue neque gravida in fermentum et sollicitudin ac orci phasellus. Scelerisque " +
                    "mauris pellentesque pulvinar pellentesque habitant morbi tristique senectus. Nunc scelerisque viverra mauris in aliquam sem. Et ligula " +
                    "ullamcorper malesuada proin libero. Nec ullamcorper sit amet risus nullam. Sed risus ultricies tristique nulla aliquet enim tortor at. At " +
                    "risus viverra adipiscing at in tellus. Vivamus at augue eget arcu dictum varius duis. Diam vulputate ut pharetra sit amet aliquam id diam. " +
                    "Tempor orci dapibus ultrices in iaculis nunc sed.",
                SettlementAmount = 2025
            },
            new EnforcementOrder
            {
                Id = 71714,
                Cause = "Sit amet est placerat in egestas erat.y monitoring for pressurized piping; failure to conduct annual test of the operation of the " +
                    "automatic line leak detector; retraining for Class A and B Operators",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Dougherty",
                Deleted = false,
                ExecutedDate = new DateTime(2019, 04, 15),
                ExecutedOrderPostedDate = new DateTime(2019, 04, 22),
                FacilityName = "Duis convallis convallis",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                IsProposedOrder = false,
                LegalAuthorityId = 6,
                OrderNumber = "EPD-UST-19-13424",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Volutpat est velit egestas dui id ornare arcu. Dignissim suspendisse in est ante in nibh mauris cursus mattis. Varius vel " +
                    "pharetra vel turpis nunc eget. Nisi quis eleifend quam adipiscing vitae proin sagittis nisl. Elit ut aliquam purus sit amet luctus " +
                    "venenatis lectus. Risus nec feugiat in fermentum. Lacus viverra vitae congue eu consequat. In nulla posuere sollicitudin aliquam ultrices " +
                    "sagittis orci a. Convallis posuere morbi leo urna molestie at elementum eu facilisis. Quis viverra nibh cras pulvinar mattis nunc sed. " +
                    "Sed faucibus turpis in eu mi bibendum neque egestas congue. Et malesuada fames ac turpis.",
                SettlementAmount = 4500
            },
            new EnforcementOrder
            {
                Id = 71715,
                Cause = "Tortor id aliquet lectus proin nibh nisl condimentum.r immediately prior to a forecasted rain event resulting in an odorous, brown " +
                    "discharge into waters of the State",
                CommentContactId = null,
                CommentPeriodClosesDate = null,
                County = "Oglethorpe",
                Deleted = false,
                ExecutedDate = DateTime.Today.AddDays(-4),
                ExecutedOrderPostedDate = DateTime.Today.AddDays(-3),
                FacilityName = "Feugiat in ante",
                HearingCommentPeriodClosesDate = null,
                HearingDate = null,
                HearingLocation = "",
                IsExecutedOrder = true,
                IsHearingScheduled = false,
                IsProposedOrder = false,
                LegalAuthorityId = 8,
                OrderNumber = "EPD-WP-17136",
                ProposedOrderPostedDate = null,
                PublicationStatus = PublicationState.Published,
                Requirements = "Nulla aliquet porttitor lacus luctus accumsan tortor posuere ac. Sed blandit libero volutpat sed. Nibh praesent tristique magna " +
                    "sit amet purus. Pretium nibh ipsum consequat nisl. Neque aliquam vestibulum morbi blandit. Dui nunc mattis enim ut. Sed egestas egestas " +
                    "fringilla phasellus faucibus scelerisque. Sagittis purus sit amet volutpat consequat mauris nunc. Cursus turpis massa tincidunt dui. " +
                    "Facilisis leo vel fringilla est ullamcorper. Consectetur adipiscing elit ut aliquam purus sit amet luctus venenatis. Velit dignissim sodales " +
                    "ut eu sem. Risus in hendrerit gravida rutrum quisque non. Consequat mauris nunc congue nisi vitae suscipit tellus. Nulla malesuada " +
                    "pellentesque elit eget gravida cum sociis. Morbi tincidunt ornare massa eget.",
                SettlementAmount = 5000
            }
        };
    }
}
