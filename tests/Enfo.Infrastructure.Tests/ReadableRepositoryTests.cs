using Enfo.Domain.Entities;
using Enfo.Domain.Querying;
using Enfo.Infrastructure.SeedData;
using Enfo.Infrastructure.Tests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.Infrastructure.Tests
{
    public class ReadableRepositoryTests
    {
        private readonly County[] _counties;
        private readonly Address[] _addresses;
        private readonly EpdContact[] _epdContacts;

        public ReadableRepositoryTests()
        {
            _counties = ProdSeedData.GetCounties();
            _addresses = ProdSeedData.GetAddresses();
            _epdContacts = ProdSeedData.GetEpdContacts();
        }

        [Fact]
        public async Task GetAllReturnsListAsync()
        {
            using var repository = this.GetRepository<County>();
            var items = await repository.ListAsync().ConfigureAwait(false);
            items.Should().HaveCount(_counties.Length);
            items[0].Should().BeEquivalentTo(_counties[0]);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        [InlineData("Ba")]
        [InlineData("bA")]
        public async Task GetWithSpecReturnsPartialListAsync(string startsWith)
        {
            using var repository = this.GetRepository<County>();
            var items = await repository
                .ListAsync(new CountyNameStartsWithSpecification(startsWith))
                .ConfigureAwait(false);
            items.Should().HaveCount(_counties
                .Where(e => e.CountyName.StartsWith(startsWith, StringComparison.InvariantCultureIgnoreCase))
                .Count());
            items[0].Should().BeEquivalentTo(_counties
                .First(e => e.CountyName.StartsWith(startsWith, StringComparison.InvariantCultureIgnoreCase)));
        }

        [Theory]
        [InlineData("B", "n")]
        [InlineData("B", "y")]
        public async Task GetWithCompositeAndSpecReturnsPartialListAsync(string startsWith, string endsWith)
        {
            using var repository = this.GetRepository<County>();

            var spec = new CountyNameStartsWithSpecification(startsWith)
                .And(new CountyNameEndsWithSpecification(endsWith));

            var items = await repository
                .ListAsync(spec)
                .ConfigureAwait(false);
            var expected = _counties
                .Where(e => e.CountyName.StartsWith(startsWith)
                    && e.CountyName.EndsWith(endsWith));

            items.Should().HaveCount(expected.Count());
            items[0].Should().BeEquivalentTo(expected.First());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetByIdReturnsItemAsync(int id)
        {
            using var repository = this.GetRepository<County>();
            var item = await repository.GetByIdAsync(id).ConfigureAwait(false);
            item.Should().BeEquivalentTo(_counties.Single(e => e.Id == id));
        }

        [Fact]
        public async Task GetByMissingIdReturnsNullAsync()
        {
            using var repository = this.GetRepository<County>();
            var item = await repository.GetByIdAsync(-1).ConfigureAwait(false);
            item.Should().BeNull();
        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        [InlineData("Ba")]
        [InlineData("bA")]
        public async Task CountWithSpecification(string startsWith)
        {
            using var repository = this.GetRepository<County>();
            var count = await repository
                .CountAsync(new CountyNameStartsWithSpecification(startsWith))
                .ConfigureAwait(false);
            count.Should().Be(_counties
                .Count(e => e.CountyName.StartsWith(
                    startsWith,
                    StringComparison.InvariantCultureIgnoreCase)));
        }

        [Theory]
        [InlineData("A")]
        [InlineData("a")]
        [InlineData("Ba")]
        [InlineData("bA")]
        public async Task CountWithSpecificationCaseSensitive(string startsWith)
        {
            using var repository = this.GetRepository<County>();
            var count = await repository
                .CountAsync(new CountyNameStartsWithSpecificationCaseSensitive(startsWith))
                .ConfigureAwait(false);
            count.Should().Be(_counties
                .Count(e => e.CountyName.StartsWith(
                    startsWith)));
        }

        [Fact]
        public async Task CountAll()
        {
            using var repository = this.GetRepository<EpdContact>();
            var count = await repository.CountAsync().ConfigureAwait(false);
            count.Should().Be(_epdContacts.Length);
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        public async Task GetByIdReturnsItemWithRelatedEntityAsync(int id)
        {
            using var repository = this.GetRepository<EpdContact>();

            var item = await repository.GetByIdAsync(
                id, inclusion: new EpdContactIncludingAddress())
                .ConfigureAwait(false);

            var expectedContact = _epdContacts.Single(e => e.Id == id);
            expectedContact.Address = _addresses.Single(e => e.Id == expectedContact.AddressId);

            item.Should().BeEquivalentTo(expectedContact);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task GetAllWithSpecificationReturnsCorrectListAsync(bool includeInactive)
        {
            using var repository = this.GetRepository<Address>();
            var items = await repository
                .ListAsync(specification: new FilterByActiveItems<Address>(includeInactive))
                .ConfigureAwait(false);

            items.Should().HaveCount(_addresses.Count(e => e.Active || includeInactive));
            items.Any(e => !e.Active).Should().Equals(includeInactive);
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        public async Task GetByIdIncludedWithSpecificationReturnsItemAsync(int id)
        {
            using var repository = this.GetRepository<Address>();
            var item = await repository.GetByIdAsync(id, new FilterByActiveItems<Address>()).ConfigureAwait(false);

            if (_addresses.Single(e => e.Id == id).Active)
            {
                item.Should().NotBeNull();
            }
            else
            {
                item.Should().BeNull();
            }
        }

        [Fact]
        public async Task GetPaginatedByPageSizeReturnsPartialListAsync()
        {
            int pageSize = 10;
            int pageNum = 2;
            int itemIndex = (pageNum - 1) * pageSize;

            using var repository = this.GetRepository<County>();

            var pagination = Pagination.FromPageSizeAndNumber(pageSize, pageNum);

            var items = await repository.ListAsync(pagination: pagination).ConfigureAwait(false);

            items.Should().HaveCount(pageSize);
            items[0].Should().BeEquivalentTo(_counties[itemIndex]);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        [InlineData("Ba")]
        public async Task GetPaginatedWithSpecification(string startsWith)
        {
            int pageSize = 10;
            int pageNum = 1;
            int itemIndex = (pageNum - 1) * pageSize;

            using var repository = this.GetRepository<County>();

            var spec = new CountyNameStartsWithSpecification(startsWith);
            var pagination = Pagination.FromPageSizeAndNumber(pageSize, pageNum);

            IReadOnlyList<County> items = await repository.ListAsync(spec, pagination).ConfigureAwait(false);

            items.Should().HaveCount(Math.Min(
                pageSize,
                _counties.Count(e => e.CountyName.StartsWith(
                    startsWith,
                    StringComparison.InvariantCultureIgnoreCase))));
            items[0].Should().BeEquivalentTo(
                _counties.First(e => e.CountyName.StartsWith(
                    startsWith,
                    StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
