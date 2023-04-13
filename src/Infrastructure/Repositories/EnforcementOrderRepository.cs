using Enfo.Domain.EnforcementOrders.Entities;
using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.EnforcementOrders.Specs;
using Enfo.Domain.Pagination;
using Enfo.Domain.Services;
using Enfo.Domain.Utils;
using Enfo.Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Enfo.Infrastructure.Repositories;

public sealed class EnforcementOrderRepository : IEnforcementOrderRepository
{
    private readonly EnfoDbContext _context;
    private readonly IFileService _fileService;
    private readonly IErrorLogger _errorLogger;

    public EnforcementOrderRepository(EnfoDbContext context, IFileService fileService, IErrorLogger errorLogger)
    {
        _context = context;
        _fileService = fileService;
        _errorLogger = errorLogger;
    }

    public async Task<EnforcementOrderDetailedView> GetAsync(int id)
    {
        var item = await _context.EnforcementOrders.AsNoTracking()
            .FilterForOnlyPublic()
            .Include(e => e.CommentContact)
            .Include(e => e.HearingContact)
            .Include(e => e.LegalAuthority)
            .Include(e => e.Attachments)
            .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

        return item == null ? null : new EnforcementOrderDetailedView(item);
    }

    public async Task<EnforcementOrderAdminView> GetAdminViewAsync(int id)
    {
        var item = (await _context.EnforcementOrders.AsNoTracking()
            .Include(e => e.CommentContact)
            .Include(e => e.HearingContact)
            .Include(e => e.LegalAuthority)
            .Include(e => e.Attachments)
            .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false));

        return item == null ? null : new EnforcementOrderAdminView(item);
    }

    public async Task<AttachmentView> GetAttachmentAsync(Guid id)
    {
        var item = await _context.Attachments.AsNoTracking()
            .Include(a => a.EnforcementOrder)
            .SingleOrDefaultAsync(a => a.Id == id && !a.Deleted).ConfigureAwait(false);

        return item is null ? null : new AttachmentView(item);
    }

    public async Task<PaginatedResult<EnforcementOrderSummaryView>> ListAsync(
        EnforcementOrderSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = _context.EnforcementOrders.AsNoTracking()
            .ApplySpecFilter(spec);

        var items = await filteredItems
            .ApplySorting(spec.Sort)
            .Include(e => e.LegalAuthority)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderSummaryView(e))
            .ToListAsync().ConfigureAwait(false);

        var count = await filteredItems.CountAsync().ConfigureAwait(false);

        return new PaginatedResult<EnforcementOrderSummaryView>(items, count, paging);
    }

    public async Task<PaginatedResult<EnforcementOrderDetailedView>> ListDetailedAsync(
        EnforcementOrderSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = _context.EnforcementOrders.AsNoTracking()
            .ApplySpecFilter(spec);

        var items = await filteredItems
            .ApplySorting(spec.Sort)
            .Include(e => e.CommentContact)
            .Include(e => e.HearingContact)
            .Include(e => e.LegalAuthority)
            .Include(e => e.Attachments)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderDetailedView(e))
            .ToListAsync().ConfigureAwait(false);

        var count = await filteredItems.CountAsync().ConfigureAwait(false);

        return new PaginatedResult<EnforcementOrderDetailedView>(items, count, paging);
    }

    public async Task<PaginatedResult<EnforcementOrderAdminSummaryView>> ListAdminAsync(
        EnforcementOrderAdminSpec spec, PaginationSpec paging)
    {
        Guard.NotNull(spec, nameof(spec));
        Guard.NotNull(paging, nameof(paging));

        var filteredItems = _context.EnforcementOrders.AsNoTracking()
            .ApplyAdminSpecFilter(spec);

        var items = await filteredItems
            .ApplySorting(spec.Sort)
            .Include(e => e.LegalAuthority)
            .ApplyPagination(paging)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToListAsync().ConfigureAwait(false);

        var count = await filteredItems.CountAsync().ConfigureAwait(false);

        return new PaginatedResult<EnforcementOrderAdminSummaryView>(items, count, paging);
    }

    public async Task<bool> ExistsAsync(int id, bool onlyPublic = true)
    {
        var item = await _context.EnforcementOrders.AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

        return item != null && (!onlyPublic || item.GetIsPublic);
    }

    public Task<bool> OrderNumberExistsAsync(string orderNumber, int? ignoreId = null) =>
        _context.EnforcementOrders.AsNoTracking()
            .AnyAsync(e => e.OrderNumber == orderNumber && !e.Deleted && e.Id != ignoreId);

    // Current Proposed are public proposed orders (publication date in the past)
    // with comment close date in the future
    public async Task<IReadOnlyList<EnforcementOrderDetailedView>> ListCurrentProposedEnforcementOrdersAsync() =>
        await _context.EnforcementOrders.AsNoTracking()
            .FilterForCurrentProposed()
            .ApplySorting(OrderSorting.DateAsc)
            .Include(e => e.CommentContact)
            .Include(e => e.HearingContact)
            .Include(e => e.LegalAuthority)
            .Include(e => e.Attachments)
            .Select(e => new EnforcementOrderDetailedView(e))
            .ToListAsync().ConfigureAwait(false);

    // Recently Executed are public executed orders with 
    // publication date within current week
    public async Task<IReadOnlyList<EnforcementOrderDetailedView>> ListRecentlyExecutedEnforcementOrdersAsync() =>
        await _context.EnforcementOrders.AsNoTracking()
            .FilterForRecentlyExecuted()
            .ApplySorting(OrderSorting.DateAsc)
            .Include(e => e.CommentContact)
            .Include(e => e.HearingContact)
            .Include(e => e.LegalAuthority)
            .Include(e => e.Attachments)
            .Select(e => new EnforcementOrderDetailedView(e))
            .ToListAsync().ConfigureAwait(false);

    // Draft are orders with publication status set to Draft
    public async Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListDraftEnforcementOrdersAsync() =>
        await _context.EnforcementOrders.AsNoTracking()
            .FilterForDrafts()
            .ApplySorting(OrderSorting.DateAsc)
            .Include(e => e.LegalAuthority)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToListAsync().ConfigureAwait(false);

    // Pending are public proposed or executed orders with 
    // publication date after the current week
    public async Task<IReadOnlyList<EnforcementOrderAdminSummaryView>> ListPendingEnforcementOrdersAsync() =>
        await _context.EnforcementOrders.AsNoTracking()
            .FilterForPending()
            .ApplySorting(OrderSorting.DateAsc)
            .Include(e => e.LegalAuthority)
            .Select(e => new EnforcementOrderAdminSummaryView(e))
            .ToListAsync().ConfigureAwait(false);

    public async Task<int> CreateAsync(EnforcementOrderCreate resource)
    {
        resource.TrimAll();
        var item = new EnforcementOrder(resource);
        await _context.EnforcementOrders.AddAsync(item).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        if (resource.Attachment is not null) await AddAttachmentAsync(item.Id, resource.Attachment);

        return item.Id;
    }

    public async Task UpdateAsync(EnforcementOrderUpdate resource)
    {
        Guard.NotNull(resource, nameof(resource));

        var item = await _context.EnforcementOrders.FindAsync(resource.Id).ConfigureAwait(false)
            ?? throw new ArgumentException($"ID ({resource.Id}) not found.", nameof(resource));

        if (item.Deleted)
            throw new ArgumentException("A deleted Enforcement Order cannot be modified.", nameof(resource));

        item.ApplyUpdate(resource);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task AddAttachmentAsync(int orderId, IFormFile file)
    {
        var order = await _context.EnforcementOrders
            .Include(e => e.Attachments)
            .SingleOrDefaultAsync(e => e.Id == orderId).ConfigureAwait(false)
            ?? throw new ArgumentException($"Order ID {orderId} does not exist.", nameof(orderId));

        if (order.Deleted)
            throw new ArgumentException($"Order ID {orderId} has been deleted and cannot be edited.", nameof(orderId));

        await SaveAttachmentAsync(file, order);
    }

    private async Task SaveAttachmentAsync(IFormFile file, EnforcementOrder order)
    {
        var extension = Path.GetExtension(file.FileName);
        if (file.Length == 0 || !FileTypes.FileUploadAllowed(extension)) return;

        var attachmentId = Guid.NewGuid();
        var attachment = new Attachment
        {
            Id = attachmentId,
            Size = file.Length,
            FileExtension = extension,
            FileName = file.FileName,
            DateUploaded = DateTime.Now,
            EnforcementOrder = order,
        };

        await _fileService.SaveFileAsync(file, attachmentId);
        await _context.Attachments.AddAsync(attachment).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAttachmentAsync(int orderId, Guid attachmentId)
    {
        var order = await _context.EnforcementOrders.AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == orderId).ConfigureAwait(false)
            ?? throw new ArgumentException($"Order ID {orderId} does not exist.", nameof(orderId));

        if (order.Deleted)
            throw new ArgumentException($"Order ID {orderId} has been deleted and cannot be edited.", nameof(orderId));

        var attachment = await _context.Attachments
            .Include(a => a.EnforcementOrder)
            .SingleOrDefaultAsync(a => a.Id == attachmentId).ConfigureAwait(false)
            ?? throw new ArgumentException($"Attachment ID {attachmentId} does not exist.", nameof(attachmentId));

        if (attachment.EnforcementOrder.Id != orderId)
            throw new ArgumentException($"Order ID {orderId} does not include Attachment ID {attachmentId}.",
                nameof(attachmentId));

        attachment.Deleted = true;
        attachment.DateDeleted = DateTime.Today;
        await _context.SaveChangesAsync().ConfigureAwait(false);

        try
        {
            _fileService.TryDeleteFile(attachment.AttachmentFileName);
        }
        catch (Exception e)
        {
            // Log error but take no other action here
            var customData = new Dictionary<string, object>
            {
                { "Action", "Deleting File" },
                { "File Name", attachment.FileName },
                { "Attachment ID", attachment.Id },
            };
            await _errorLogger.LogErrorAsync(e, customData);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.EnforcementOrders.FindAsync(id).ConfigureAwait(false)
            ?? throw new ArgumentException($"ID ({id}) not found.", nameof(id));

        item.Deleted = true;
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task RestoreAsync(int id)
    {
        var item = await _context.EnforcementOrders.FindAsync(id).ConfigureAwait(false)
            ?? throw new ArgumentException($"ID ({id}) not found.", nameof(id));

        item.Deleted = false;
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public void Dispose() => _context.Dispose();
}
