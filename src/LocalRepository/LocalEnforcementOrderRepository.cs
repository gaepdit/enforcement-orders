using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.Domain.Services;
using Enfo.Domain.Utils;
using EnfoTests.TestData;
using Microsoft.AspNetCore.Http;

namespace Enfo.LocalRepository;

public sealed class LocalEnforcementOrderRepository : IEnforcementOrderRepository
{
    private readonly IFileService _fileService;

    public LocalEnforcementOrderRepository(IFileService fileService) =>
        _fileService = fileService;

    public Task<EnforcementOrderDetailedView> GetAsync(int id)
    {
        if (!EnforcementOrderData.EnforcementOrders.AsQueryable()
                .FilterForOnlyPublic().Any(e => e.Id == id))
            return Task.FromResult(null as EnforcementOrderDetailedView);

        var order = EnforcementOrderData.GetEnforcementOrderDetailedView(id);

        return Task.FromResult(order);
    }

    public Task<EnforcementOrderAdminView> GetAdminViewAsync(int id)
    {
        if (!EnforcementOrderData.EnforcementOrders.AsQueryable().Any(e => e.Id == id))
            return Task.FromResult(null as EnforcementOrderAdminView);

        var order = EnforcementOrderData.GetEnforcementOrderAdminView(id);

        return Task.FromResult(order);
    }

    public Task<AttachmentView> GetAttachmentAsync(Guid id) =>
        AttachmentData.Attachments.Exists(a => a.Id == id && !a.Deleted)
            ? Task.FromResult(new AttachmentView(
                AttachmentData.Attachments.Single(a => a.Id == id)!))
            : Task.FromResult(null as AttachmentView);

    public Task<PaginatedResult<EnforcementOrderSummaryView>> ListAsync(
        EnforcementOrderSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = EnforcementOrderData.GetEnforcementOrdersIncludeAttachments().AsQueryable()
            .ApplySpecFilter(spec);

        var items = filteredItems
            .ApplySorting(spec.Sort)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToList();

        var count = filteredItems.Count();
        var result = new PaginatedResult<EnforcementOrderSummaryView>(items, count, paging);
        return Task.FromResult(result);
    }

    public Task<PaginatedResult<EnforcementOrderDetailedView>> ListDetailedAsync(
        EnforcementOrderSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = EnforcementOrderData.GetEnforcementOrdersIncludeAttachments().AsQueryable()
            .ApplySpecFilter(spec);

        var items = filteredItems
            .ApplySorting(spec.Sort)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderDetailedView(e))
            .ToList();

        var count = filteredItems.Count();
        var result = new PaginatedResult<EnforcementOrderDetailedView>(items, count, paging);
        return Task.FromResult(result);
    }

    public Task<PaginatedResult<EnforcementOrderAdminSummaryView>> ListAdminAsync(
        EnforcementOrderAdminSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = EnforcementOrderData.GetEnforcementOrdersIncludeAttachments().AsQueryable()
            .ApplyAdminSpecFilter(spec);

        var items = filteredItems
            .ApplySorting(spec.Sort)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToList();

        var count = filteredItems.Count();
        var result = new PaginatedResult<EnforcementOrderAdminSummaryView>(items, count, paging);
        return Task.FromResult(result);
    }

    public Task<bool> ExistsAsync(int id, bool onlyPublic = true) =>
        Task.FromResult(
            EnforcementOrderData.EnforcementOrders
                .Exists(e => e.Id == id && (e.GetIsPublic || !onlyPublic)));

    public Task<bool> OrderNumberExistsAsync(string orderNumber, int? ignoreId = null) =>
        Task.FromResult(
            EnforcementOrderData.EnforcementOrders
                .Exists(e => string.Equals(e.OrderNumber, orderNumber, StringComparison.CurrentCultureIgnoreCase) && !e.Deleted && e.Id != ignoreId));

    public Task<IReadOnlyList<EnforcementOrderDetailedView>> ListCurrentProposedEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderDetailedView>)
            EnforcementOrderData.GetEnforcementOrdersIncludeAttachments().AsQueryable()
                .FilterForCurrentProposed()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderDetailedView(e))
                .ToList());

    public Task<IReadOnlyList<EnforcementOrderDetailedView>> ListRecentlyExecutedEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderDetailedView>)
            EnforcementOrderData.GetEnforcementOrdersIncludeAttachments().AsQueryable()
                .FilterForRecentlyExecuted()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderDetailedView(e))
                .ToList());

    public Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListDraftEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderAdminSummaryView>)
            EnforcementOrderData.GetEnforcementOrdersIncludeAttachments().AsQueryable()
                .FilterForDrafts()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderAdminSummaryView(e))
                .ToList());

    public Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListPendingEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderAdminSummaryView>)
            EnforcementOrderData.GetEnforcementOrdersIncludeAttachments().AsQueryable()
                .FilterForPending()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderAdminSummaryView(e))
                .ToList());

    public async Task<int> CreateAsync(EnforcementOrderCreate resource)
    {
        resource.TrimAll();
        var id = EnforcementOrderData.EnforcementOrders.Max(e => e.Id) + 1;

        var item = new EnforcementOrder(resource) { Id = id };
        item.LegalAuthority = LegalAuthorityData.GetLegalAuthority(item.LegalAuthorityId);
        item.CommentContact = item.CommentContactId is null
            ? null
            : EpdContactData.GetEpdContact(item.CommentContactId.Value);
        item.HearingContact = item.HearingContactId is null
            ? null
            : EpdContactData.GetEpdContact(item.HearingContactId.Value);

        EnforcementOrderData.EnforcementOrders.Add(item);
        if (resource.Attachment is not null) await AddAttachmentInternalAsync(resource.Attachment, item);
        return id;
    }

    public Task UpdateAsync(EnforcementOrderUpdate resource)
    {
        Guard.NotNull(resource, nameof(resource));

        var item = EnforcementOrderData.EnforcementOrders.SingleOrDefault(e => e.Id == resource.Id)
            ?? throw new ArgumentException($"ID ({resource.Id}) not found.", nameof(resource));

        if (item.Deleted)
            throw new ArgumentException("A deleted Enforcement Order cannot be modified.", nameof(resource));

        item.ApplyUpdate(resource);
        item.LegalAuthority = LegalAuthorityData.GetLegalAuthority(item.LegalAuthorityId);
        item.CommentContact = item.CommentContactId is null
            ? null
            : EpdContactData.GetEpdContact(item.CommentContactId.Value);
        item.HearingContact = item.HearingContactId is null
            ? null
            : EpdContactData.GetEpdContact(item.HearingContactId.Value);

        return Task.CompletedTask;
    }

    public Task AddAttachmentAsync(int orderId, IFormFile file)
    {
        if (!EnforcementOrderData.EnforcementOrders.AsQueryable().Any(e => e.Id == orderId))
            throw new ArgumentException($"Order ID {orderId} does not exist.", nameof(orderId));

        var order = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == orderId);

        if (order.Deleted)
            throw new ArgumentException($"Order ID {orderId} has been deleted and cannot be edited.", nameof(orderId));

        return AddAttachmentInternalAsync(file, order);
    }

    private async Task AddAttachmentInternalAsync(IFormFile file, EnforcementOrder order)
    {
        var extension = Path.GetExtension(file.FileName);
        if (file.Length == 0 || !FileTypes.FileUploadAllowed(extension)) return;

        var attachmentId = Guid.NewGuid();
        await _fileService.SaveFileAsync(file, attachmentId);

        var attachment = new Attachment
        {
            Id = attachmentId,
            Size = file.Length,
            FileExtension = extension,
            FileName = file.FileName,
            DateUploaded = DateTime.Now,
            EnforcementOrder = order,
        };

        AttachmentData.Attachments.Add(attachment);
    }

    public Task DeleteAttachmentAsync(int orderId, Guid attachmentId)
    {
        if (!EnforcementOrderData.EnforcementOrders.AsQueryable().Any(e => e.Id == orderId))
            throw new ArgumentException($"Order ID {orderId} does not exist.", nameof(orderId));

        var order = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == orderId);

        if (order.Deleted)
            throw new ArgumentException($"Order ID {orderId} has been deleted and cannot be edited.", nameof(orderId));

        if (!AttachmentData.Attachments.AsQueryable().Any(a => a.Id == attachmentId))
            throw new ArgumentException($"Attachment ID {attachmentId} does not exist.", nameof(attachmentId));

        var attachment = AttachmentData.Attachments.Single(a => a.Id == attachmentId);

        if (attachment.EnforcementOrder.Id != orderId)
            throw new ArgumentException($"Order ID {orderId} does not include Attachment ID {attachmentId}.",
                nameof(attachmentId));

        attachment.Deleted = true;
        attachment.DateDeleted = DateTime.Today;

        _fileService.TryDeleteFile(attachment.AttachmentFileName);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id) => SetWhetherDeleted(id, true);

    public Task RestoreAsync(int id) => SetWhetherDeleted(id, false);

    private static Task SetWhetherDeleted(int id, bool deleted)
    {
        var item = EnforcementOrderData.EnforcementOrders.SingleOrDefault(e => e.Id == id)
            ?? throw new ArgumentException($"ID ({id}) not found.", nameof(id));

        item.Deleted = deleted;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
