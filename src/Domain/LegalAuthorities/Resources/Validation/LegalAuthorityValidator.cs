using Enfo.Domain.LegalAuthorities.Repositories;
using FluentValidation;

namespace Enfo.Domain.LegalAuthorities.Resources.Validation;

public class LegalAuthorityValidator : AbstractValidator<LegalAuthorityCommand>
{
    private readonly ILegalAuthorityRepository _repository;

    public LegalAuthorityValidator(ILegalAuthorityRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.AuthorityName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (authorityName, _) => await NotDuplicateName(authorityName).ConfigureAwait(false))
            .WithMessage(e => $"The authority name entered ({e.AuthorityName}) already exists.");
    }

    private async Task<bool> NotDuplicateName(string authorityName) =>
        !await _repository.NameExistsAsync(authorityName).ConfigureAwait(false);
}
