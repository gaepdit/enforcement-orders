using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.Services;
using Enfo.Domain.Utils;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages;

public class Attachment : PageModel
{
    private readonly IEnforcementOrderRepository _repository;
    private readonly IFileService _fileService;

    public Attachment(IEnforcementOrderRepository repository, IFileService fileService) =>
        (_repository, _fileService) = (repository, fileService);

    public async Task<IActionResult> OnGetAsync(Guid? id, [CanBeNull] string fileName)
    {
        if (id == null) return NotFound();

        var item = await _repository.GetAttachmentAsync(id.Value);
        if (item == null || string.IsNullOrWhiteSpace(item.FileName))
            return NotFound($"File ID not found: {id.Value}");

        if (fileName != item.FileName)
            return RedirectToPage("Attachment", new { id, item.FileName });

        var fileBytes = await _fileService.GetFileAsync(string.Concat(item.Id, item.FileExtension));

        return fileBytes.Length == 0
            ? NotFound($"File not available: {item.FileName}")
            : File(fileBytes, FileTypes.GetContentType(item.FileExtension));
    }
}
