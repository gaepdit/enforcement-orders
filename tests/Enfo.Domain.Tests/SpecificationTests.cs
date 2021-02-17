using Enfo.Repository.Querying;
using Enfo.Domain.Tests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Enfo.Domain.Tests
{
    public class SpecificationTests
    {
        [Fact]
        public void SuccessfullyCreateSpecification()
        {
            var spec = new StringStartsWithSpecification("A");

            spec.Criteria.Should()
                .BeEquivalentTo(new List<Expression<Func<string, bool>>>
                {
                    e => e.ToLower().StartsWith("a")
                });
        }

        [Fact]
        public void SuccessfullyCreateCompositeSpecification()
        {
            var spec1 = new StringStartsWithSpecification("A");
            var spec2 = new StringEndsWithSpecification("B");
            var spec = spec1.And(spec2);

            spec.Criteria.Should().HaveCount(2);
            spec.Criteria.Should()
                .BeEquivalentTo(new List<Expression<Func<string, bool>>>
                {
                    e => e.ToLower().StartsWith("a"),
                    e => e.ToLower().EndsWith("b")
                });
        }

        [Fact]
        public void CreateEmptySpecification()
        {
            var spec = new EmptySpecification();
            spec.Criteria.Should().BeNull();
        }

        [Fact]
        public void NullSpecificationCriterionThrowsException()
        {
            Action act = () => new NullCriterionSpecification();
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("criterion");
        }

        [Fact]
        public void NullSpecificationCriteriaThrowsException()
        {
            Action act = () => new NullCriteriaSpecification();
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("criteria");
        }

        [Fact]
        public void EmptySpecificationCriteriaThrowsException()
        {
            Action act = () => new EmptyCriteriaSpecification();
            act.Should().Throw<ArgumentException>()
                .And.ParamName.Should().Be("criteria");
        }
    }
}
