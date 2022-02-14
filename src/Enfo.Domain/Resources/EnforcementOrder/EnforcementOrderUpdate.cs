using Enfo.Domain.Repositories;

namespace Enfo.Domain.Resources.EnforcementOrder;

public class EnforcementOrderUpdate
{
    public EnforcementOrderUpdate() { }

    public EnforcementOrderUpdate(EnforcementOrderAdminView item)
    {
        Cause = item.Cause;
        County = item.County;
        Requirements = item.Requirements;
        ExecutedDate = item.ExecutedDate;
        FacilityName = item.FacilityName;
        HearingDate = item.HearingDate;
        HearingLocation = item.HearingLocation;
        OrderNumber = item.OrderNumber;
        Progress = item.PublicationStatus;
        SettlementAmount = item.SettlementAmount;
        CommentContactId = item.CommentContact?.Id;
        IsInactiveCommentContact = !item.CommentContact?.Active ?? false;
        HearingContactId = item.HearingContact?.Id;
        IsInactiveHearingContact = !item.HearingContact?.Active ?? false;
        IsProposedOrder = item.IsProposedOrder;
        IsExecutedOrder = item.IsExecutedOrder;
        IsHearingScheduled = item.IsHearingScheduled;
        LegalAuthorityId = item.LegalAuthority.Id;
        CommentPeriodClosesDate = item.CommentPeriodClosesDate;
        ExecutedOrderPostedDate = item.ExecutedOrderPostedDate;
        ProposedOrderPostedDate = item.ProposedOrderPostedDate;
        HearingCommentPeriodClosesDate = item.HearingCommentPeriodClosesDate;
    }

    // Common data elements

    [DisplayName("Facility")]
    [Required(ErrorMessage = "Facility Name is required.")]
    public string FacilityName { get; set; }

    [Required(ErrorMessage = "County Name is required.")]
    [StringLength(20)]
    public string County { get; set; }

    [DisplayName("Legal Authority")]
    [Required(ErrorMessage = "Legal Authority is required.")]
    public int LegalAuthorityId { get; init; }

    [DisplayName("Cause of Order")]
    [Required(ErrorMessage = "Cause of Order is required.")]
    [DataType(DataType.MultilineText)]
    public string Cause { get; set; }

    [DisplayName("Requirements of Order")]
    [DataType(DataType.MultilineText)]
    public string Requirements { get; set; }

    [DisplayName("Settlement Amount")]
    [DataType(DataType.Currency)]
    public decimal? SettlementAmount { get; init; }

    public PublicationProgress Progress { get; init; }

    [DisplayName("Order Number")]
    [Required(ErrorMessage = "Order Number is required.")]
    [StringLength(50)]
    public string OrderNumber { get; set; }

    // Proposed orders

    [DisplayName("Proposed Order Public Noticed")]
    public bool IsProposedOrder { get; init; }

    [DisplayName("Date Comment Period Closes")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? CommentPeriodClosesDate { get; init; }

    [DisplayName("Send Comments To")]
    public int? CommentContactId { get; init; }

    public bool IsInactiveCommentContact { get; }

    [DisplayName("Publication Date For Proposed Order")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ProposedOrderPostedDate { get; init; }

    // Executed orders

    [DisplayName("Enforcement Order Executed")]
    public bool IsExecutedOrder { get; init; }

    [DisplayName("Date Executed")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ExecutedDate { get; set; }

    [DisplayName("Publication Date For Executed Order")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ExecutedOrderPostedDate { get; set; }

    // Hearing info

    [DisplayName("Public Hearing Scheduled")]
    public bool IsHearingScheduled { get; set; }

    [DisplayName("Hearing Date/Time")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDateTime, ApplyFormatInEditMode = true)]
    public DateTime? HearingDate { get; set; }

    [DisplayName("Hearing Location")]
    [DataType(DataType.MultilineText)]
    public string HearingLocation { get; set; }

    [DisplayName("Date Hearing Comment Period Closes")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? HearingCommentPeriodClosesDate { get; set; }

    [DisplayName("Hearing Information Contact")]
    public int? HearingContactId { get; set; }

    public bool IsInactiveHearingContact { get; }

    private void TrimAll()
    {
        Cause = Cause?.Trim();
        County = County?.Trim();
        Requirements = Requirements?.Trim();
        FacilityName = FacilityName?.Trim();
        HearingLocation = HearingLocation?.Trim();
        OrderNumber = OrderNumber?.Trim();
    }

    public Task<ResourceUpdateResult<EnforcementOrderAdminView>> TryUpdateAsync(
        IEnforcementOrderRepository repository, int id)
    {
        if (repository == null) throw new ArgumentNullException(nameof(repository));
        return TryUpdateInternalAsync(repository, id);
    }

    private async Task<ResourceUpdateResult<EnforcementOrderAdminView>> TryUpdateInternalAsync(
        [NotNull] IEnforcementOrderRepository repository, int id)
    {

        var result = new ResourceUpdateResult<EnforcementOrderAdminView>
        {
            OriginalItem = await repository.GetAdminViewAsync(id)
        };

        if (result.OriginalItem is null || result.OriginalItem.Deleted)
        {
            return result;
        }

        TrimAll();

        await ValidateEnforcementOrderUpdateAsync(result, repository);

        if (result.IsValid)
        {
            await repository.UpdateAsync(id, this);
            result.Success = true;
        }

        return result;
    }

    private async Task ValidateEnforcementOrderUpdateAsync(
        [NotNull] ResourceUpdateResult<EnforcementOrderAdminView> result,
        [NotNull] IEnforcementOrderRepository repository)
    {
        var order = result.OriginalItem;

        if (await repository.OrderNumberExistsAsync(OrderNumber, order.Id).ConfigureAwait(false))
        {
            result.AddValidationError(nameof(EnforcementOrderCreate.OrderNumber),
                $"An Order with the same number ({OrderNumber}) already exists.");
        }

        // Executed order info can only be removed if proposed order info exists.
        if (!IsExecutedOrder && order.IsExecutedOrder && !order.IsProposedOrder)
        {
            result.AddValidationError(nameof(IsExecutedOrder),
                "Executed Order details are required for this Enforcement Order.");
        }

        if (Progress != PublicationProgress.Published) return;

        if (SettlementAmount is < 0)
            result.AddValidationError(nameof(EnforcementOrderCreate.SettlementAmount),
                "Settlement Amount cannot be less than zero.");

        // Proposed order info cannot be removed from an existing order.
        if (order.IsProposedOrder)
        {
            if (CommentContactId is null)
                result.AddValidationError(nameof(CommentContactId),
                    "A contact for comments is required for proposed orders.");

            if (!CommentPeriodClosesDate.HasValue)
                result.AddValidationError(nameof(CommentPeriodClosesDate),
                    "A closing date for comments is required for proposed orders.");

            if (!ProposedOrderPostedDate.HasValue)
                result.AddValidationError(nameof(ProposedOrderPostedDate),
                    "A publication date is required for proposed orders.");
        }

        if (IsExecutedOrder)
        {
            if (!ExecutedDate.HasValue)
                result.AddValidationError(nameof(ExecutedDate),
                    "An execution date is required for executed orders.");

            if (!ExecutedOrderPostedDate.HasValue)
                result.AddValidationError(nameof(ExecutedOrderPostedDate),
                    "A publication date is required for executed orders.");
        }

        if (!IsHearingScheduled) return;

        if (!HearingDate.HasValue)
            result.AddValidationError(nameof(HearingDate),
                "A hearing date is required if a hearing is scheduled.");

        if (string.IsNullOrEmpty(HearingLocation))
            result.AddValidationError(nameof(HearingLocation),
                "A hearing location is required if a hearing is scheduled.");

        if (!HearingCommentPeriodClosesDate.HasValue)
            result.AddValidationError(nameof(HearingCommentPeriodClosesDate),
                "A closing date for hearing comments is required if a hearing is scheduled.");

        if (HearingContactId is null)
            result.AddValidationError(nameof(HearingContactId),
                "A contact for hearings is required if a hearing is scheduled.");
    }

}
