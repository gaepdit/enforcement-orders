using Enfo.Domain.Repositories;
using Enfo.Domain.Resources.LegalAuthority;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.AssertExtensions;
using static EnfoTests.Helpers.ResourceHelper;

namespace EnfoTests.Domain.LegalAuthorityTests
{
    public class LegalAuthorityCommandTests
    {
        [Fact]
        public async Task TrySave_GivenUniqueName_ReturnsSuccess()
        {
            var item = new LegalAuthorityCommand { AuthorityName = "test" };
            var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
            repo.Setup(l => l.NameExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);
            repo.Setup(l => l.CreateAsync(item))
                .ReturnsAsync(1);

            var result = await item.TrySaveNewAsync(repo.Object);

            result.IsValid.ShouldBeTrue();
            result.Success.ShouldBeTrue();
            result.NewId.ShouldEqual(1);
            result.ValidationErrors.Count.ShouldEqual(0);
        }

        [Fact]
        public async Task TrySave_GivenExistingName_ReturnsFail()
        {
            var item = new LegalAuthorityCommand { AuthorityName = "test" };
            var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
            repo.Setup(l => l.NameExistsAsync(It.IsAny<string>(), null))
                .ReturnsAsync(true);

            var result = await item.TrySaveNewAsync(repo.Object);

            result.IsValid.ShouldBeFalse();
            result.Success.ShouldBeFalse();
            result.NewId.ShouldBeNull();
            result.ValidationErrors
                .ShouldContain(new KeyValuePair<string, string>(nameof(item.AuthorityName),
                $"The authority name entered ({item.AuthorityName}) already exists."));
        }

        [Fact]
        public async Task TryUpdate_GivenUniqueName_ReturnsSuccess()
        {
            var originalItem = GetLegalAuthorityViewList()[0];
            var item = new LegalAuthorityCommand(originalItem);
            var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(originalItem);
            repo.Setup(l => l.NameExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(false);


            var result = await item.TryUpdateAsync(repo.Object, default);

            result.IsValid.Should().BeTrue();
            result.Success.Should().BeTrue();
            result.ValidationErrors.Count.Should().Be(0);
            result.OriginalItem.ShouldEqual(originalItem);
        }

        [Fact]
        public async Task TryUpdate_GivenExistingName_ReturnsFail()
        {
            var originalItem = GetLegalAuthorityViewList()[0];
            var item = new LegalAuthorityCommand { AuthorityName = "test" };
            var repo = new Mock<ILegalAuthorityRepository> { DefaultValue = DefaultValue.Mock };
            repo.Setup(l => l.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(originalItem);
            repo.Setup(l => l.NameExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(true);

            var result = await item.TryUpdateAsync(repo.Object, default);

            result.IsValid.Should().BeFalse();
            result.Success.Should().BeFalse();
            result.ValidationErrors
                .ShouldContain(new KeyValuePair<string, string>(nameof(item.AuthorityName),
                $"The authority name entered ({item.AuthorityName}) already exists."));
            result.OriginalItem.ShouldEqual(originalItem);
        }
    }
}
