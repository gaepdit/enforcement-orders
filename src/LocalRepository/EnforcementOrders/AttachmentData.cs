using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.LocalRepository.Users;

namespace Enfo.LocalRepository.EnforcementOrders;

internal static class AttachmentData
{
    public static readonly List<Attachment> Attachments = new()
    {
        new Attachment
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
            FileName = "FileOne",
            FileExtension = ".pdf",
            Size = 1,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 1),
            UploadedBy = UsersData.Users[0],
            DateUploaded = DateTime.Today.AddDays(-2),
        },
        new Attachment
        {
            Id = new Guid("00000000-0000-0000-0000-000000000002"),
            FileName = "File-Two",
            FileExtension = ".pdf",
            Size = 10,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 1),
            UploadedBy = UsersData.Users[0],
            DateUploaded = DateTime.Today.AddDays(-1),
        },
        new Attachment
        {
            Id = new Guid("00000000-0000-0000-0000-000000000003"),
            FileName = "File Three",
            FileExtension = ".pdf",
            Size = 100,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 2),
            UploadedBy = UsersData.Users[0],
            DateUploaded = DateTime.Today.AddDays(-1),
        },
        new Attachment
        {
            Id = new Guid("00000000-0000-0000-0000-000000000004"),
            FileName = "FileFourDeleted",
            FileExtension = ".pdf",
            Size = 1000,
            EnforcementOrder = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == 3),
            UploadedBy = UsersData.Users[0],
            DateUploaded = DateTime.Today.AddDays(-2),
            Deleted = true,
            DeletedBy = UsersData.Users[0],
            DateDeleted = DateTime.Today.AddDays(-1),
        },
    };
}
