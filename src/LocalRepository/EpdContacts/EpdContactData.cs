﻿using Enfo.Domain.EpdContacts.Entities;

// ReSharper disable StringLiteralTypo

namespace Enfo.LocalRepository.EpdContacts;

internal static class EpdContactData
{
    public static readonly List<EpdContact> EpdContacts = new()
    {
        new EpdContact
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
        new EpdContact
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
        new EpdContact
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
}
