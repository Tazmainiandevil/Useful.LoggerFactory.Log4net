using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Useful.LoggerFactory.Log4net;
using Xunit;

namespace Useful.LoggerFactory.Log4Net.Tests
{
    public class Log4NetFactoryExtensionsTests
    {
        [Fact]
        public void add_log4net_with_null_factory_throws_exception()
        {
            // Arrange
            // Act
            Action act = () => Log4NetFactoryExtensions.AddLog4Net(null, "somefile");

            // Assert
            act.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: factory");
        }

        [Fact]
        public void add_log4net_with_factory_adds_provider_to_the_factory()
        {
            // Arrange
            ILoggerProvider provider = null;
            var factory = Substitute.For<ILoggerFactory>();
            factory.When(x => x.AddProvider(Arg.Any<ILoggerProvider>())).Do(y => provider = y.Arg<ILoggerProvider>());

            // Act
            Log4NetFactoryExtensions.AddLog4Net(factory, "somefile");

            // Assert
            provider.Should().BeOfType<Log4NetProvider>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void add_log4net_with_factory_and_null_or_empty_configuration_file_throws_exception(string configuration)
        {
            // Arrange
            var factory = Substitute.For<ILoggerFactory>();
            // Act
            Action act = () => Log4NetFactoryExtensions.AddLog4Net(factory, configuration);

            // Assert
            act.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: log4netConfiguration");
        }
    }
}
