using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Users;
using Enfo.Domain.Utils;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enfo.WebApp.Pages.Admin;

[Authorize]
public class Details(IEnforcementOrderRepository repository) : PageModel
{
    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    public IFormFile Attachment { get; set; }

    public EnforcementOrderAdminView Item { get; private set; }
    public DisplayMessage Message { get; private set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();
        Item = await repository.GetAdminViewAsync(id.Value);
        if (Item == null) return NotFound("ID not found.");

        // TempData might be null when called from unit test
        // ReSharper disable once ConstantConditionalAccessQualifier
        Message = TempData?.GetDisplayMessage();

        Id = id.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAddAttachmentAsync()
    {
        if (!User.IsInRole(AppRole.OrderAdministrator)) return Forbid();

        Item = await repository.GetAdminViewAsync(Id);
        if (Item == null) return NotFound("ID not found.");

        if (Item.Deleted)
        {
            TempData.SetDisplayMessage(Context.Warning, "This Enforcement Order is deleted and cannot be edited.");
            return Page();
        }

        if (!ModelState.IsValid || Attachment is null)
        {
            Message = new DisplayMessage(Context.Warning, "Please select a valid file.");
            return Page();
        }

        if (Attachment.Length == 0)
        {
            Message = new DisplayMessage(Context.Error, "File upload failed.");
            return Page();
        }

        var extension = Path.GetExtension(Attachment.FileName);

        if (!FileTypes.FileUploadAllowed(extension))
        {
            Message = new DisplayMessage(Context.Error, "Invalid file. Only PDF attachments are allowed.");
            return Page();
        }

        await repository.AddAttachmentAsync(Id, Attachment);
        return RedirectToPage("Details", null, new { Id }, "attachments");
    }

    public async Task<IActionResult> OnPostDeleteAttachmentAsync(Guid attachmentId)
    {
        if (!User.IsInRole(AppRole.OrderAdministrator)) return Forbid();

        Item = await repository.GetAdminViewAsync(Id);
        if (Item == null) return NotFound("ID not found.");

        if (Item.Deleted)
        {
            TempData.SetDisplayMessage(Context.Warning, "This Enforcement Order is deleted and cannot be edited.");
            return Page();
        }

        await repository.DeleteAttachmentAsync(Id, attachmentId);
        return RedirectToPage("Details", null, new { Id }, "attachments");
    }
}
