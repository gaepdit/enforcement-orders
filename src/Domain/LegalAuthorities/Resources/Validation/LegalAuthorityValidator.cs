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
            .NotNull()
            .MustAsync(async (authorityName, _) => await NotDuplicateName(authorityName))
            .WithMessage(e => $"The authority name entered ({e.AuthorityName}) already exists.");
    }

    private async Task<bool> NotDuplicateName(string authorityName, int? ignoreId = null) =>
        !await _repository.NameExistsAsync(authorityName, ignoreId);
}
