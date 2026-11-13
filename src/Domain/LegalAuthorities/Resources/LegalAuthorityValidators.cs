using FluentValidation;

namespace Enfo.Domain.LegalAuthorities.Resources;

public class LegalAuthorityCreateValidator : AbstractValidator<LegalAuthorityCreateDto>
{
    private readonly ILegalAuthorityRepository _repository;

    public LegalAuthorityCreateValidator(ILegalAuthorityRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.AuthorityName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(2, 100)
            .MustAsync(async (authorityName, token) =>
                await NotDuplicateName(authorityName, token).ConfigureAwait(false))
            .WithMessage("The authority name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string authorityName, CancellationToken token) =>
        !await _repository.NameExistsAsync(authorityName, token).ConfigureAwait(false);
}

public class LegalAuthorityUpdateValidator : AbstractValidator<LegalAuthorityUpdateDto>
{
    private readonly ILegalAuthorityRepository _repository;

    public LegalAuthorityUpdateValidator(ILegalAuthorityRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.AuthorityName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(2, 100)
            .MustAsync(async (_, name, context, token) =>
                await NotDuplicateName(name, context, token).ConfigureAwait(false))
            .WithMessage("The authority name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, ValidationContext<LegalAuthorityUpdateDto> context,
        CancellationToken token)
    {
        var itemId = await _repository.FindIdByNameAsync(name, token: token).ConfigureAwait(false);
        return itemId is null || itemId == (int)context.RootContextData["Id"];
    }
}
