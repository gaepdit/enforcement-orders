using Enfo.Domain.Repositories;
using Enfo.Domain.Utils;

namespace Enfo.Domain.Resources.EnforcementOrder;

public class EnforcementOrderCreate
{
    // Common data elements

    [DisplayName("Facility")]
    [Required(ErrorMessage = "Facility Name is required.")]
    public string FacilityName { get; set; }

    [Required(ErrorMessage = "County Name is required.")]
    [StringLength(20)]
    public string County { get; set; }

    [DisplayName("Legal Authority")]
    [Required(ErrorMessage = "Legal Authority is required.")]
    public int? LegalAuthorityId { get; set; }

    [DisplayName("Cause of Order")]
    [Required(ErrorMessage = "Cause of Order is required.")]
    [DataType(DataType.MultilineText)]
    public string Cause { get; set; }

    [DisplayName("Requirements of Order")]
    [DataType(DataType.MultilineText)]
    public string Requirements { get; set; }

    [DisplayName("Settlement Amount")]
    [DataType(DataType.Currency)]
    public decimal? SettlementAmount { get; set; }

    public PublicationProgress Progress { get; set; } = PublicationProgress.Published;

    [DisplayName("Order Number")]
    [Required(ErrorMessage = "Order Number is required.")]
    [StringLength(50)]
    public string OrderNumber { get; set; }

    //  Determines the type of Enforcement Order created
    [DisplayName("Status")]
    public NewEnforcementOrderType CreateAs { get; set; } = NewEnforcementOrderType.Proposed;

    // Proposed orders

    [DisplayName("Date Comment Period Closes")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? CommentPeriodClosesDate { get; set; }

    [DisplayName("Send Comments To")]
    public int? CommentContactId { get; set; }

    [DisplayName("Publication Date For Proposed Order")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ProposedOrderPostedDate { get; set; } = DateUtils.NextMonday();

    // Executed orders

    [DisplayName("Date Executed")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ExecutedDate { get; set; }

    [DisplayName("Publication Date For Executed Order")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? ExecutedOrderPostedDate { get; set; } = DateUtils.NextMonday();

    // Hearing info

    [DisplayName("Public Hearing Scheduled")]
    public bool IsHearingScheduled { get; set; }

    [DisplayName("Hearing Date/Time")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDateTime, ApplyFormatInEditMode = true)]
    public DateTime? HearingDate { get; set; } = DateTime.Today.AddHours(12);

    [DisplayName("Hearing Location")]
    [DataType(DataType.MultilineText)]
    public string HearingLocation { get; set; }

    [DisplayName("Date Hearing Comment Period Closes")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DisplayFormats.EditDate, ApplyFormatInEditMode = true)]
    public DateTime? HearingCommentPeriodClosesDate { get; set; }

    [DisplayName("Hearing Information Contact")]
    public int? HearingContactId { get; set; }

    private void TrimAll()
    {
        County = County?.Trim();
        FacilityName = FacilityName?.Trim();
        Cause = Cause?.Trim();
        Requirements = Requirements?.Trim();
        OrderNumber = OrderNumber?.Trim();
        HearingLocation = HearingLocation?.Trim();
    }

    public Task<ResourceSaveResult> TrySaveNewAsync(IEnforcementOrderRepository repository)
    {
        if (repository == null) throw new ArgumentNullException(nameof(repository));

        if (!Enum.IsDefined(CreateAs))
            throw new InvalidEnumArgumentException(nameof(CreateAs), (int)CreateAs, typeof(NewEnforcementOrderType));

        return TrySaveNewInternalAsync(repository);
    }

    private async Task<ResourceSaveResult> TrySaveNewInternalAsync(
        [NotNull] IEnforcementOrderRepository repository)
    {
        TrimAll();

        var result = await ValidateNewEnforcementOrderAsync(repository);

        if (result.IsValid)
        {
            result.NewId = await repository.CreateAsync(this);
            result.Success = true;
        }

        return result;
    }

    private async Task<ResourceSaveResult> ValidateNewEnforcementOrderAsync(
        [NotNull] IEnforcementOrderRepository repository)
    {
        var result = new ResourceSaveResult();

        if (await repository.OrderNumberExistsAsync(OrderNumber).ConfigureAwait(false))
            result.AddValidationError(nameof(EnforcementOrderCreate.OrderNumber),
                $"An Order with the same number ({OrderNumber}) already exists.");

        if (Progress != PublicationProgress.Published) return result;

        if (SettlementAmount is < 0)
            result.AddValidationError(nameof(EnforcementOrderCreate.SettlementAmount),
                "Settlement Amount cannot be less than zero.");

        switch (CreateAs)
        {
            case NewEnforcementOrderType.Proposed:
                {
                    if (CommentContactId is null)
                        result.AddValidationError(nameof(EnforcementOrderCreate.CommentContactId),
                            "A contact for comments is required for proposed orders.");

                    if (!CommentPeriodClosesDate.HasValue)
                        result.AddValidationError(nameof(EnforcementOrderCreate.CommentPeriodClosesDate),
                            "A closing date for comments is required for proposed orders.");

                    if (!ProposedOrderPostedDate.HasValue)
                        result.AddValidationError(nameof(EnforcementOrderCreate.ProposedOrderPostedDate),
                            "A publication date is required for proposed orders.");

                    break;
                }
            case NewEnforcementOrderType.Executed:
                {
                    if (!ExecutedDate.HasValue)
                        result.AddValidationError(nameof(EnforcementOrderCreate.ExecutedDate),
                            "An execution date is required for executed orders.");

                    if (!ExecutedOrderPostedDate.HasValue)
                        result.AddValidationError(nameof(EnforcementOrderCreate.ExecutedOrderPostedDate),
                            "A publication date is required for executed orders.");

                    break;
                }

            default:
                break;
        }

        if (!IsHearingScheduled) return result;

        if (HearingDate is null)
            result.AddValidationError(nameof(EnforcementOrderCreate.HearingDate),
                "A hearing date is required if a hearing is scheduled.");

        if (string.IsNullOrEmpty(HearingLocation))
            result.AddValidationError(nameof(EnforcementOrderCreate.HearingLocation),
                "A hearing location is required if a hearing is scheduled.");

        if (HearingCommentPeriodClosesDate is null)
            result.AddValidationError(nameof(EnforcementOrderCreate.HearingCommentPeriodClosesDate),
                "A closing date for hearing comments is required if a hearing is scheduled.");

        if (HearingContactId is null)
            result.AddValidationError(nameof(EnforcementOrderCreate.HearingContactId),
                "A contact for hearings is required if a hearing is scheduled.");

        return result;
    }
}

public enum NewEnforcementOrderType
{
    Proposed,
    Executed
}
