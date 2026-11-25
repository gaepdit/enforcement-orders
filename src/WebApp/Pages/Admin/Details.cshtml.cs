using Enfo.Domain.EnforcementOrders.Repositories;
using Enfo.Domain.EnforcementOrders.Resources;
using Enfo.Domain.Identity;
using Enfo.Domain.Utils;
using Enfo.WebApp.Models;
using Enfo.WebApp.Platform.PageModelHelpers;

namespace Enfo.WebApp.Pages.Admin;

[Authorize]
public class DetailsModel(IEnforcementOrderRepository repository) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public IFormFile? Attachment { get; set; }

    public EnforcementOrderAdminView Item { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return NotFound();
        Item = await repository.GetAdminViewAsync(Id);
        if (Item == null) return NotFound("ID not found.");
        return Page();
    }

    public async Task<IActionResult> OnPostAddAttachmentAsync()
    {
        if (!User.IsInRole(AppRole.OrderAdministrator)) return Forbid();

        Item = await repository.GetAdminViewAsync(Id);
        if (Item == null) return NotFound("ID not found.");

        if (Item.Deleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "This Enforcement Order is deleted and cannot be edited.");
            return Page();
        }

        if (!ModelState.IsValid || Attachment is null)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning, "Please select a valid file.");
            return Page();
        }

        if (Attachment.Length == 0)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Danger, "File upload failed.");
            return Page();
        }

        var extension = Path.GetExtension(Attachment.FileName);

        if (!FileTypes.FileUploadAllowed(extension))
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Danger,
                "Invalid file. Only PDF attachments are allowed.");
            return Page();
        }

        await repository.AddAttachmentAsync(Id, Attachment);
        return RedirectToPage("Details", null, new { Id }, "attachments");
    }

    public async Task<IActionResult> OnPostDeleteAttachmentAsync(Guid attachmentId)
    {
        if (!User.IsInRole(AppRole.OrderAdministrator)) return Forbid();

        Item = await repository.GetAdminViewAsync(Id);
        if (Item == null) return BadRequest("ID not found.");

        if (Item.Deleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "This Enforcement Order is deleted and cannot be edited.");
            return Page();
        }

        await repository.DeleteAttachmentAsync(Id, attachmentId);
        return RedirectToPage("Details", null, new { Id }, "attachments");
    }
}
