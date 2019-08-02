using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Domain.Specifications;
using Enfo.Infrastructure.Tests.Helpers;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Enfo.Infrastructure.Tests.RepositoryTests
{
    public class ReadableRepositoryTests
    {
        [Fact]
        public async Task GetAllReturnsListAsync()
        {
            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                IReadOnlyList<County> items = await repository.ListAsync().ConfigureAwait(false);

                items.Should().HaveCount(159);
                var expected = new County { Id = 1, CountyName = "Appling" };
                items[0].Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task GetByIdReturnsItemAsync()
        {
            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                County item = await repository.GetByIdAsync(1).ConfigureAwait(false);

                var expected = new County { Id = 1, CountyName = "Appling" };
                item.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async Task GetByMissingIdReturnsNullAsync()
        {
            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                County item = await repository.GetByIdAsync(-1).ConfigureAwait(false);

                item.Should().BeNull();
            }
        }

        private class CountyNameStartsWithLetterSpecification : Specification<County>
        {
            public CountyNameStartsWithLetterSpecification(char startsWith)
                : base(e => e.CountyName.StartsWith(startsWith)) { }
        }

        [Fact]
        public async Task CountWithSpecification()
        {
            using (IAsyncReadableRepository<County> repository = this.GetRepository<County>())
            {
                int count = await repository.CountAsync(new CountyNameStartsWithLetterSpecification('B')).ConfigureAwait(false);

                count.Should().Be(16);
            }
        }

        [Fact]
        public async Task CountAll()
        {
            using (var repository = this.GetRepository<EpdContact>())
            {
                int count = await repository.CountAsync().ConfigureAwait(false);

                count.Should().Be(3);
            }
        }

        [Fact]
        public async Task GetByIdReturnsItemWithRelatedEntityAsync()
        {
            using (var repository = this.GetRepository<EpdContact>())
            {
                var item = await repository.GetByIdAsync(2000).ConfigureAwait(false);

                var expectedAddress = new Address { Id = 2000, Active = true, City = "Atlanta", PostalCode = "30354", State = "GA", Street = "4244 International Parkway", Street2 = "Suite 120" };
                var expectedContact = new EpdContact { Id = 2000, Active = false, Address = expectedAddress, AddressId = 2000, ContactName = "Mr. Keith M. Bentley", Email = "null", Organization = "Environmental Protection Division", Title = "Chief, Air Protection Branch" };

                item.Should().BeEquivalentTo(expectedContact);
                item.Address.Should().BeEquivalentTo(expectedAddress);
            }
        }

        [Fact]
        public async Task GetAllWithSpecificationReturnsCorrectListAsync()
        {
            using (IAsyncReadableRepository<Address> repository = this.GetRepository<Address>())
            {
                IReadOnlyList<Address> items = await repository.ListAsync(new ExcludeInactiveItemsSpecification<Address>()).ConfigureAwait(false);

                items.Should().HaveCount(2);
                items.Any(e => !e.Active).Should().BeFalse();
            }
        }

        [Fact]
        public async Task GetByIdIncludedWithSpecificationReturnsItemAsync()
        {
            using (IAsyncReadableRepository<Address> repository = this.GetRepository<Address>())
            {
                Address item = await repository.GetByIdAsync(2000, new ExcludeInactiveItemsSpecification<Address>()).ConfigureAwait(false);

                item.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetByIdExcludedWithSpecificationReturnsNullAsync()
        {
            using (IAsyncReadableRepository<Address> repository = this.GetRepository<Address>())
            {
                Address item = await repository.GetByIdAsync(2001, new ExcludeInactiveItemsSpecification<Address>()).ConfigureAwait(false);

                item.Should().BeNull();
            }
        }
    }
}
