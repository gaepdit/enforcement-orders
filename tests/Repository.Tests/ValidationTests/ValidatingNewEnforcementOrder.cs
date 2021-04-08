using FluentAssertions;
using Xunit;
using static Enfo.Repository.Validation.EnforcementOrderValidation;
using static TestHelpers.ResourceHelper;

namespace Repository.Tests.ValidationTests
{
    public class ValidatingNewEnforcementOrder
    {
        [Fact]
        public void SucceedsGivenValidOrder()
        {
            var order = GetValidEnforcementOrderCreate();
            var result = ValidateNewEnforcementOrder(order);

            result.IsValid.Should().BeTrue();
            result.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        public void FailsIfMissingRequiredProperties()
        {
            var order = GetValidEnforcementOrderCreate();
            order.CommentContactId = null;
            order.CommentPeriodClosesDate = null;
            order.ProposedOrderPostedDate = null;

            var result = ValidateNewEnforcementOrder(order);

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