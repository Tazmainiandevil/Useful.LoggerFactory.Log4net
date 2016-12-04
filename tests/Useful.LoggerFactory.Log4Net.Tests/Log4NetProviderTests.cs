using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using log4net;
using Useful.LoggerFactory.Log4net;
using Xunit;

namespace Useful.LoggerFactory.Log4Net.Tests
{
    public class Log4NetProviderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void create_logger_with_null_or_empty_name_throws_exception(string name)
        {
            // Arrange
            var provider = new Log4NetProvider();

            // Act
            Action act = () => provider.CreateLogger(null);

            // Assert
            act.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: name");
        }

        [Fact]
        public void create_logger_returns_the_expected_type()
        {
            // Arrange
            var provider = new Log4NetProvider();

            // Act
            var logger = provider.CreateLogger("TestLogger");

            // Assert
            logger.Should().BeOfType<Log4NetLogger>();
        }

        [Fact]
        public void create_logger_returns_the_expected_logger_with_expected_name()
        {
            // Arrange
            var provider = new Log4NetProvider();

            // Act
            var logger = provider.CreateLogger("TestLogger");

            // Assert
            (logger as Log4NetLogger).LoggerName.Should().Be("TestLogger");
        }

        [Fact]
        public void create_logger_with_existing_loggers_returns_the_expected_logger()
        {
            // Arrange
            var provider = new Log4NetProvider();
            provider.CreateLogger("TestLogger1");
            provider.CreateLogger("TestLogger2");

            // Act
            var logger = provider.CreateLogger("TestLogger3");

            // Assert
            (logger as Log4NetLogger).LoggerName.Should().Be("TestLogger3");
        }


        [Fact]
        public void create_logger_adding_two_loggers_at_the_same_time_adds_both_loggers()
        {
            // Arrange
            var provider = new Log4NetProvider();

            // Act
            var task1 = Task.Factory.StartNew(() =>
            {
                provider.CreateLogger("TestLogger1");
            });

            var task2 = Task.Factory.StartNew(() =>
            {
                provider.CreateLogger("TestLogger2");
            });

            Task.WaitAll(task1, task2);

            // Assert
            provider.Loggers.Count.Should().Be(2);
        }

        [Fact]
        public void create_logger_adding_two_loggers_with_the_same_name_at_the_same_time_only_has_one_logger()
        {
            // Arrange
            var provider = new Log4NetProvider();

            // Act
            var task1 = Task.Factory.StartNew(() =>
            {
                provider.CreateLogger("TestLogger");
            });

            var task2 = Task.Factory.StartNew(() =>
            {
                provider.CreateLogger("TestLogger");
            });

            Task.WaitAll(task1, task2);

            // Assert
            provider.Loggers.Count.Should().Be(1);
        }

        [Fact]
        public void provider_clears_loggers_when_disposed()
        {
            // Arrange
            var provider = new Log4NetProvider();
            provider.CreateLogger("TestLogger");
            provider.CreateLogger("TestLogger1");

            // Act
            provider.Dispose();

            // Assert
            provider.Loggers.Count.Should().Be(0);
        }
    }
}
