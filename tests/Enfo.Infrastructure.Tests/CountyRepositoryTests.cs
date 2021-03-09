using System.Linq;
using Enfo.Infrastructure.Repositories;
using Enfo.Repository.Resources.County;
using FluentAssertions;
using Xunit;
using static Enfo.Domain.Data.DomainData;

namespace Enfo.Infrastructure.Tests
{
    public class CountyRepositoryTests
    {
        [Fact]
        public void ReturnsList()
        {
            var items = CountyRepository.List();
            items.Should().HaveCount(Counties.Count());
            items[0].Should().BeEquivalentTo(new CountyView(Counties.First()));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void ReturnsItemGivenId(int id)
        {
            var item = CountyRepository.Get(id);
            item.Should().BeEquivalentTo(new CountyView(Counties.Single(e => e.Id == id)));
        }

        [Fact]
        public void ReturnsNullGivenUnassignedId()
        {
            var item = CountyRepository.Get(-1);
            item.Should().BeNull();
        }
    }
}