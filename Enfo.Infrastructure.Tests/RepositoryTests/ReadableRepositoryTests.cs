using Enfo.Domain.Entities;
using Enfo.Domain.Pagination;
using Enfo.Domain.Repositories;
using Enfo.Domain.Specifications;
using Enfo.Infrastructure.SeedData;
using Enfo.Infrastructure.Tests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.Infrastructure.Tests.RepositoryTests
{
    public class ReadableRepositoryTests
    {
        private readonly County[] counties;
        private readonly Address[] addresses;
        private readonly EpdContact[] epdContacts;

        public ReadableRepositoryTests()
        {
            counties = ProdSeedData.GetCounties();
            addresses = ProdSeedData.GetAddresses();
            epdContacts = ProdSeedData.GetEpdContacts();
        }

        // helpers
        private class CountyNameStartsWithSpecification : BaseSpecification<County>
        {
            public CountyNameStartsWithSpecification(string startsWith)
                : base(e => e.CountyName.ToLower().StartsWith(startsWith.ToLower())) { }
        }

        private class CountyNameStartsWithSpecificationCaseSensitive : BaseSpecification<County>
        {
            public CountyNameStartsWithSpecificationCaseSensitive(string startsWith)
                : base(e => e.CountyName.StartsWith(startsWith)) { }
        }

        // Tests

        [Fact]
        public async Task GetAllReturnsListAsync()
        {
            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                var items = await repository.ListAsync().ConfigureAwait(false);
                items.Should().HaveCount(counties.Length);
                items[0].Should().BeEquivalentTo(counties[0]);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetByIdReturnsItemAsync(int id)
        {
            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                var item = await repository.GetByIdAsync(id).ConfigureAwait(false);
                item.Should().BeEquivalentTo(counties.Single(e => e.Id == id));
            }
        }

        [Fact]
        public async Task GetByMissingIdReturnsNullAsync()
        {
            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                var item = await repository.GetByIdAsync(-1).ConfigureAwait(false);
                item.Should().BeNull();
            }
        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        [InlineData("Ba")]
        [InlineData("bA")]
        public async Task CountWithSpecification(string startsWith)
        {
            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                var count = await repository
                    .CountAsync(new CountyNameStartsWithSpecification(startsWith))
                    .ConfigureAwait(false);
                count.Should().Be(counties
                    .Count(e => e.CountyName.StartsWith(
                        startsWith,
                        StringComparison.InvariantCultureIgnoreCase)));
            }
        }

        [Theory]
        [InlineData("A")]
        [InlineData("a")]
        [InlineData("Ba")]
        [InlineData("bA")]
        public async Task CountWithSpecificationCaseSensitive(string startsWith)
        {
            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                var count = await repository
                    .CountAsync(new CountyNameStartsWithSpecificationCaseSensitive(startsWith))
                    .ConfigureAwait(false);
                count.Should().Be(counties
                    .Count(e => e.CountyName.StartsWith(
                        startsWith)));
            }
        }

        [Fact]
        public async Task CountAll()
        {
            using (var repository = this.GetRepository<EpdContact>())
            {
                var count = await repository.CountAsync().ConfigureAwait(false);
                count.Should().Be(epdContacts.Length);
            }
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        public async Task GetByIdReturnsItemWithRelatedEntityAsync(int id)
        {
            using (var repository = this.GetRepository<EpdContact>())
            {
                var item = await repository.GetByIdAsync(
                    id,
                    new EpdContactIncludeAddressSpec(includeInactive: true))
                    .ConfigureAwait(false);

                var expectedContact = epdContacts.Single(e => e.Id == id);
                expectedContact.Address = addresses.Single(e => e.Id == expectedContact.AddressId);

                item.Should().BeEquivalentTo(expectedContact);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task GetAllWithSpecificationReturnsCorrectListAsync(bool includeInactive)
        {
            using (IAsyncReadableRepository<Address> repository = this.GetRepository<Address>())
            {
                var items = await repository.ListAsync(new ExcludeInactiveItemsSpec<Address>(includeInactive)).ConfigureAwait(false);

                items.Should().HaveCount(addresses.Count(e => e.Active || includeInactive));
                items.Any(e => !e.Active).Should().Equals(includeInactive);
            }
        }

        [Theory]
        [InlineData(2000)]
        [InlineData(2001)]
        public async Task GetByIdIncludedWithSpecificationReturnsItemAsync(int id)
        {
            using (IAsyncReadableRepository<Address> repository = this.GetRepository<Address>())
            {
                var item = await repository.GetByIdAsync(id, new ExcludeInactiveItemsSpec<Address>()).ConfigureAwait(false);

                if (addresses.Single(e => e.Id == id).Active)
                {
                    item.Should().NotBeNull();
                }
                else
                {
                    item.Should().BeNull();
                }
            }
        }

        [Fact]
        public async Task GetPaginatedReturnsPartialListAsync()
        {
            int skip = 10;
            int take = 2;
            int firstItemIndex = skip;

            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                var pagination = Pagination.FromPageSkipAndTake(skip, take);

                var items = await repository.ListAsync(pagination).ConfigureAwait(false);

                items.Should().HaveCount(take);
                items[0].Should().BeEquivalentTo(counties[firstItemIndex]);
            }
        }

        [Fact]
        public async Task GetPaginatedByPageSizeReturnsPartialListAsync()
        {
            int pageSize = 10;
            int pageNum = 2;
            int itemIndex = (pageNum - 1) * pageSize;

            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                var pagination = Pagination.FromPageSizeAndNumber(pageSize, pageNum);

                var items = await repository.ListAsync(pagination).ConfigureAwait(false);

                items.Should().HaveCount(pageSize);
                items[0].Should().BeEquivalentTo(counties[itemIndex]);
            }
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

            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                var spec = new CountyNameStartsWithSpecification(startsWith);
                var pagination = Pagination.FromPageSizeAndNumber(pageSize, pageNum);

                IReadOnlyList<County> items = await repository.ListAsync(spec, pagination).ConfigureAwait(false);

                items.Should().HaveCount(Math.Min(
                    pageSize,
                    counties.Count(e => e.CountyName.StartsWith(
                        startsWith,
                        StringComparison.InvariantCultureIgnoreCase))));
                items[0].Should().BeEquivalentTo(
                    counties.First(e => e.CountyName.StartsWith(
                        startsWith,
                        StringComparison.InvariantCultureIgnoreCase)));
            }
        }
    }
}
