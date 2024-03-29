﻿using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.Services;
using Enfo.Domain.Utils;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages;

public class Attachment : PageModel
{
    private readonly IEnforcementOrderRepository _repository;
    private readonly IAttachmentStore _attachmentStore;

    public Attachment(IEnforcementOrderRepository repository, IAttachmentStore attachmentStore)
    {
        _repository = repository;
        _attachmentStore = attachmentStore;
    }

    public async Task<IActionResult> OnGetAsync(Guid? id, [CanBeNull] string fileName)
    {
        if (id == null) return NotFound();

        var item = await _repository.GetAttachmentAsync(id.Value);
        if (item == null || string.IsNullOrWhiteSpace(item.FileName))
            return NotFound($"Attachment ID not found: {id.Value.ToString()}");

        var order = await _repository.GetAdminViewAsync(item.EnforcementOrderId);

        if ((User.Identity is null || !User.Identity.IsAuthenticated) && (order.Deleted || !order.IsPublic))
            return NotFound($"Attachment ID not found: {id.Value.ToString()}");

        if (fileName != item.FileName)
            return RedirectToPage("Attachment", new { id, item.FileName });

        var fileBytes = await _attachmentStore.GetFileAttachmentAsync(item.AttachmentFileName);

        return fileBytes.Length == 0
            ? NotFound($"File not available: {item.FileName}")
            : File(fileBytes, FileTypes.GetContentType(item.FileExtension));
    }
}
