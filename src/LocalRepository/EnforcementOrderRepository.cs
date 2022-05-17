using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.Domain.Services;
using Enfo.Domain.Utils;
using Microsoft.AspNetCore.Http;
using TestData;

namespace Enfo.LocalRepository;

public sealed class EnforcementOrderRepository : IEnforcementOrderRepository
{
    private readonly IFileService _fileService;

    public EnforcementOrderRepository(IFileService fileService) =>
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
        AttachmentData.Attachments.Any(e => e.Id == id && !e.Deleted)
            ? Task.FromResult(new AttachmentView(
                AttachmentData.Attachments.Single(e => e.Id == id)!))
            : Task.FromResult(null as AttachmentView);

    public Task<PaginatedResult<EnforcementOrderSummaryView>> ListAsync(
        EnforcementOrderSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = EnforcementOrderData.GetEnforcementOrders().AsQueryable()
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

        var filteredItems = EnforcementOrderData.GetEnforcementOrders().AsQueryable()
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

        var filteredItems = EnforcementOrderData.GetEnforcementOrders().AsQueryable()
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
                .Any(e => e.Id == id && (e.GetIsPublic || !onlyPublic)));

    public Task<bool> OrderNumberExistsAsync(string orderNumber, int? ignoreId = null) =>
        Task.FromResult(
            EnforcementOrderData.EnforcementOrders
                .Any(e => e.OrderNumber == orderNumber && !e.Deleted && e.Id != ignoreId));

    public Task<IReadOnlyList<EnforcementOrderDetailedView>> ListCurrentProposedEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderDetailedView>)
            EnforcementOrderData.GetEnforcementOrders().AsQueryable()
                .FilterForCurrentProposed()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderDetailedView(e))
                .ToList());

    public Task<IReadOnlyList<EnforcementOrderDetailedView>> ListRecentlyExecutedEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderDetailedView>)
            EnforcementOrderData.GetEnforcementOrders().AsQueryable()
                .FilterForRecentlyExecuted()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderDetailedView(e))
                .ToList());

    public Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListDraftEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderAdminSummaryView>)
            EnforcementOrderData.GetEnforcementOrders().AsQueryable()
                .FilterForDrafts()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderAdminSummaryView(e))
                .ToList());

    public Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListPendingEnforcementOrdersAsync() =>
        Task.FromResult(
            (IReadOnlyList<EnforcementOrderAdminSummaryView>)
            EnforcementOrderData.GetEnforcementOrders().AsQueryable()
                .FilterForPending()
                .ApplySorting(OrderSorting.DateAsc)
                .Select(e => new EnforcementOrderAdminSummaryView(e))
                .ToList());

    public Task<int> CreateAsync(EnforcementOrderCreate resource)
    {
        resource.TrimAll();
        var id = EnforcementOrderData.EnforcementOrders.Max(e => e.Id) + 1;
        var item = new EnforcementOrder(resource) { Id = id };
        EnforcementOrderData.EnforcementOrders.Add(item);

        return Task.FromResult(id);
    }

    public Task UpdateAsync(EnforcementOrderUpdate resource)
    {
        Guard.NotNull(resource, nameof(resource));

        var item = EnforcementOrderData.EnforcementOrders.SingleOrDefault(e => e.Id == resource.Id)
            ?? throw new ArgumentException($"ID ({resource.Id}) not found.", nameof(resource));

        if (item.Deleted)
            throw new ArgumentException("A deleted Enforcement Order cannot be modified.", nameof(resource));

        item.ApplyUpdate(resource);
        return Task.CompletedTask;
    }

    public Task AddAttachmentsAsync(int orderId, List<IFormFile> files)
    {
        if (files.Count == 0) throw new ArgumentException("Files list must not be empty.", nameof(files));

        if (!EnforcementOrderData.EnforcementOrders.AsQueryable().Any(e => e.Id == orderId))
            throw new ArgumentException($"Order ID {orderId} does not exist.", nameof(orderId));

        var order = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == orderId);

        if (order.Deleted)
            throw new ArgumentException($"Order ID {orderId} has been deleted and cannot be edited.", nameof(orderId));

        return AddAttachmentsInternalAsync(files, order);
    }

    private async Task AddAttachmentsInternalAsync(List<IFormFile> files, EnforcementOrder order)
    {
        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName);
            if (file.Length == 0 || !FileTypes.FileUploadAllowed(extension)) continue;

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
    }

    public Task DeleteAttachmentAsync(int orderId, Guid attachmentId)
    {
        if (!EnforcementOrderData.EnforcementOrders.AsQueryable().Any(e => e.Id == orderId))
            throw new ArgumentException($"Order ID {orderId} does not exist.", nameof(orderId));

        var order = EnforcementOrderData.EnforcementOrders.Single(e => e.Id == orderId);

        if (order.Deleted)
            throw new ArgumentException($"Order ID {orderId} has been deleted and cannot be edited.", nameof(orderId));

        if (!AttachmentData.Attachments.AsQueryable().Any(e => e.Id == attachmentId))
            throw new ArgumentException($"Attachment ID {attachmentId} does not exist.", nameof(attachmentId));

        var attachment = AttachmentData.Attachments.Single(a => a.Id == attachmentId);
        AttachmentData.Attachments.Remove(attachment);
        _fileService.TryDeleteFile(string.Concat(attachment.Id.ToString(), attachment.FileExtension));

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
