using Enfo.Domain.Querying;
using Enfo.Infrastructure.QueryingEvaluators;
using Enfo.Infrastructure.Tests.Helpers;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Enfo.Infrastructure.Tests
{
    public class SpecificationEvaluatorTests
    {
        [Theory]
        [InlineData("Abc", "A")]
        [InlineData("Abc", "a")]
        [InlineData("abc", "A")]
        public void EvaluatePositiveSpecification(string testString, string startsWith)
        {
            IQueryable<string> list = (new List<string> { testString }).AsQueryable();
            var spec = new StringStartsWithSpecification(startsWith);
            var result = list.Apply(spec);

            result.Should().BeEquivalentTo(list);
        }

        [Theory]
        [InlineData("ABC", "A", "C")]
        [InlineData("abc", "A", "C")]
        [InlineData("ABC", "a", "c")]
        public void EvaluatePositiveCompositeSpecification(
            string testString,
            string startsWith,
            string endsWith)
        {
            IQueryable<string> list = (new List<string> { testString }).AsQueryable();
            var spec = new StringStartsWithSpecification(startsWith)
                .And(new StringEndsWithSpecification(endsWith));
            var result = list.Apply(spec);

            result.Should().BeEquivalentTo(list);
        }

        [Theory]
        [InlineData("Abc", "X")]
        public void EvaluateNegativeSpecification(string testString, string startsWith)
        {
            IQueryable<string> list = (new List<string> { testString }).AsQueryable();
            var spec = new StringStartsWithSpecification(startsWith);
            var result = list.Apply(spec);

            result.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("ABC", "X", "C")]
        [InlineData("ABC", "A", "X")]
        [InlineData("ABC", "X", "X")]
        public void EvaluateNegativeCompositeSpecification(
            string testString,
            string startsWith,
            string endsWith)
        {
            IQueryable<string> list = (new List<string> { testString }).AsQueryable();
            var spec = new StringStartsWithSpecification(startsWith)
                .And(new StringEndsWithSpecification(endsWith));
            var result = list.Apply(spec);

            result.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("Abc")]
        public void EvaluateEmptySpecification(string testString)
        {
            IQueryable<string> list = (new List<string> { testString }).AsQueryable();
            var spec = new EmptySpecification();
            var result = list.Apply(spec);

            result.Should().BeEquivalentTo(list);
        }

        [Theory]
        [InlineData("Abc")]
        public void EvaluateTrueSpecification(string testString)
        {
            IQueryable<string> list = (new List<string> { testString }).AsQueryable();
            var spec = new TrueSpec<string>();
            var result = list.Apply(spec);

            result.Should().BeEquivalentTo(list);
        }

        [Theory]
        [InlineData("Abc", "A")]
        [InlineData("Abc", "a")]
        [InlineData("abc", "A")]
        public void EvaluateCompositeWithTrueSpecification(string testString, string startsWith)
        {
            IQueryable<string> list = (new List<string> { testString }).AsQueryable();
            var spec = new TrueSpec<string>()
                .And(new StringStartsWithSpecification(startsWith));
            var result = list.Apply(spec);

            result.Should().BeEquivalentTo(list);
        }
    }
}
