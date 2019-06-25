using Enfo.Domain.Entities;
using Enfo.Domain.Repositories;
using Enfo.Infrastructure.Repositories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static Enfo.Infrastructure.Tests.Helpers.RepositoryHelpers;

namespace Enfo.Infrastructure.Tests.RepositoryTests
{
    public class ReadableRepositoryTests
    {
        [Fact]
        public async Task GetAllReturnsListAsync()
        {
            IAsyncReadableRepository<County> repository = this.GetRepository<County>();

            IReadOnlyList<County> items = await repository.ListAsync().ConfigureAwait(false);

            items.Should().HaveCount(159);
            var expected = new County { Id = 1, CountyName = "Appling" };
            items[0].Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdReturnsItemAsync()
        {
            IAsyncReadableRepository<County> repository = this.GetRepository<County>();

            County item = await repository.GetByIdAsync(1).ConfigureAwait(false);

            var expected = new County { Id = 1, CountyName = "Appling" };
            item.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByMissingIdReturnsNullAsync()
        {
            IAsyncReadableRepository<County> repository = this.GetRepository<County>();

            County item = await repository.GetByIdAsync(-1).ConfigureAwait(false);

            item.Should().BeNull();
        }

        [Fact]
        public async Task CountWithSpecification()
        {
            IAsyncReadableRepository<County> repository = this.GetRepository<County>();

            var specification = new Specification<County>(e => e.CountyName.StartsWith("B", StringComparison.CurrentCultureIgnoreCase));
            int count = await repository.CountAsync(specification).ConfigureAwait(false);

            count.Should().Be(16);
        }

        [Fact]
        public async Task CountAll()
        {
            var repository = this.GetRepository<EpdContact>();

            int count = await repository.CountAsync().ConfigureAwait(false);

            count.Should().Be(3);
        }

        [Fact]
        public async Task GetByIdReturnsItemWithRelatedEntityAsync()
        {
            var repository = this.GetRepository<EpdContact>();

            var item = await repository.GetByIdAsync(2000).ConfigureAwait(false);

            // since this passes, are the GetByIdAsync overloads with includes even needed?

            var expectedAddress = new Address { Id = 2000, Active = true, City = "Atlanta", PostalCode = "30354", State = "GA", Street = "4244 International Parkway", Street2 = "Suite 120" };
            var expectedContact = new EpdContact { Id = 2000, Active = false, Address = expectedAddress, AddressId = 2000, ContactName = "Mr. Keith M. Bentley", Email = "null", Organization = "Environmental Protection Division", Title = "Chief, Air Protection Branch" };

            item.Should().BeEquivalentTo(expectedContact);
            item.Address.Should().BeEquivalentTo(expectedAddress);
        }

        [Fact]
        public async Task GetByIdWithIncludeReturnsItemWithRelatedEntityAsync()
        {
            var repository = this.GetRepository<EpdContact>();

            var item = await repository.GetByIdAsync(2000, e => e.Address).ConfigureAwait(false);

            var expectedAddress = new Address { Id = 2000, Active = true, City = "Atlanta", PostalCode = "30354", State = "GA", Street = "4244 International Parkway", Street2 = "Suite 120" };
            var expectedContact = new EpdContact { Id = 2000, Active = false, Address = expectedAddress, AddressId = 2000, ContactName = "Mr. Keith M. Bentley", Email = "null", Organization = "Environmental Protection Division", Title = "Chief, Air Protection Branch" };

            item.Should().BeEquivalentTo(expectedContact);
            item.Address.Should().BeEquivalentTo(expectedAddress);
        }

        [Fact]
        public async Task GetByIdWithIncludeStringsReturnsItemWithRelatedEntityAsync()
        {
            var repository = this.GetRepository<EpdContact>();

            var item = await repository.GetByIdAsync(2000, new List<string> { "Address" }).ConfigureAwait(false);

            var expectedAddress = new Address { Id = 2000, Active = true, City = "Atlanta", PostalCode = "30354", State = "GA", Street = "4244 International Parkway", Street2 = "Suite 120" };
            var expectedContact = new EpdContact { Id = 2000, Active = false, Address = expectedAddress, AddressId = 2000, ContactName = "Mr. Keith M. Bentley", Email = "null", Organization = "Environmental Protection Division", Title = "Chief, Air Protection Branch" };

            item.Should().BeEquivalentTo(expectedContact);
            item.Address.Should().BeEquivalentTo(expectedAddress);
        }
    }
}
