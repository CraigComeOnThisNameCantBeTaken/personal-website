using System.Configuration;
using GitIntegration.Resolvers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GitIntegration.Tests.Resolvers
{
    public class OptionResolverTest
    {
        private readonly IOptionResolver sut;
        private readonly Mock<IOptionsSnapshot<GitIntegrationOption>> options;

        public OptionResolverTest()
        {
            options = new Mock<IOptionsSnapshot<GitIntegrationOption>>();
            sut = new OptionResolver(options.Object);
        }

        [Fact]
        public void Resolve_Should_ThrowWhenNoOptionAvailable()
        {
            Assert.Throws<ConfigurationErrorsException>(() => sut.Resolve("missing"));
        }

        [Fact]
        public void Resolve_Should_ReturnIntegrationOptionsForGivenProvider()
        {
            var expected = new GitIntegrationOption();

            options
                .Setup(o => o.Get(It.IsAny<string>()))
                .Returns(expected);

            var result = sut.Resolve("GitHub");
            Assert.Equal(expected, result);
        }
    }
}
