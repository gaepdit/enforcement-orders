using FluentAssertions;
using Xunit;
using static EnfoTests.Helpers.ResourceHelper;
using static Enfo.Domain.Validation.EnforcementOrderValidation;

namespace EnfoTests.Domain.ValidationTests
{
    public class ValidatingNewEnforcementOrder
    {
        [Fact]
        public void SucceedsGivenValidOrder()
        {
            var orderCreate = GetValidEnforcementOrderCreate();
            var result = ValidateNewEnforcementOrder(orderCreate);

            result.IsValid.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void FailsIfMissingRequiredProperties()
        {
            var orderCreate = GetValidEnforcementOrderCreate();
            orderCreate.CommentContactId = null;
            orderCreate.CommentPeriodClosesDate = null;
            orderCreate.ProposedOrderPostedDate = null;

            var result = ValidateNewEnforcementOrder(orderCreate);

            result.IsValid.Should().BeFalse();
            result.ErrorMessages.Should().NotBeEmpty()
                .And.HaveCount(3)
                .And.ContainKeys(
                    "CommentContactId",
                    "CommentPeriodClosesDate",
                    "ProposedOrderPostedDate");
        }
    }
}