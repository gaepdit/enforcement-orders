using Enfo.Domain.Repositories;
using FluentValidation;

namespace Enfo.Domain.Validation;

public abstract class BaseEnforcementOrderValidator<T> : AbstractValidator<T>
{
    private readonly IEnforcementOrderRepository _repository;

    protected BaseEnforcementOrderValidator(IEnforcementOrderRepository repository) =>
        _repository = repository;

    internal async Task<bool> NotDuplicateOrderNumber(string orderNumber, int? ignoreId = null) =>
        !await _repository.OrderNumberExistsAsync(orderNumber?.Trim(), ignoreId).ConfigureAwait(false);
}
