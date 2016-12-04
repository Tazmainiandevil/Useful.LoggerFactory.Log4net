using FluentAssertions;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using Useful.LoggerFactory.Log4net;
using Xunit;

namespace Useful.LoggerFactory.Log4Net.Tests
{
    public class Log4NetLoggerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void construct_log4net_logger_with_null_or_empty_logger_name_throws_exception(string loggerName)
        {
            // Arrange
            // Act
            Action act = () => new Log4NetLogger(loggerName);

            // Assert
            act.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: loggerName");
        }

        public static IEnumerable<object[]> LogLevelTestData()
        {
            yield return new object[] { Level.Debug, LogLevel.Trace, true };
            yield return new object[] { Level.Debug, LogLevel.Debug, true };
            yield return new object[] { Level.Info, LogLevel.Trace, false };
            yield return new object[] { Level.Info, LogLevel.Debug, false };

            yield return new object[] { Level.Info, LogLevel.Information, true };
            yield return new object[] { Level.Debug, LogLevel.Information, true };
            yield return new object[] { Level.Warn, LogLevel.Information, false };

            yield return new object[] { Level.Warn, LogLevel.Warning, true };
            yield return new object[] { Level.Info, LogLevel.Warning, true };
            yield return new object[] { Level.Debug, LogLevel.Warning, true };
            yield return new object[] { Level.Error, LogLevel.Warning, false };

            yield return new object[] { Level.Error, LogLevel.Error, true };
            yield return new object[] { Level.Warn, LogLevel.Error, true };
            yield return new object[] { Level.Info, LogLevel.Error, true };
            yield return new object[] { Level.Debug, LogLevel.Error, true };
            yield return new object[] { Level.Fatal, LogLevel.Error, false };

            yield return new object[] { Level.Fatal, LogLevel.Critical, true };
            yield return new object[] { Level.Error, LogLevel.Critical, true };
            yield return new object[] { Level.Warn, LogLevel.Critical, true };
            yield return new object[] { Level.Info, LogLevel.Critical, true };
            yield return new object[] { Level.Debug, LogLevel.Critical, true };
            yield return new object[] { Level.Debug, 99, false };
        }

        [Theory]
        [MemberData("LogLevelTestData")]
        public void is_enabled_for_the_log_levels_returns_expected_value(Level configLevel, LogLevel toTest, bool expected)
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger");
            var repo = LogManager.GetRepository().As<Hierarchy>();
            repo.Root.Level = configLevel;
            repo.Configured = true;

            // Act
            var result = logger.IsEnabled(toTest);

            // Assert
            result.Should().Be(expected);
        }

        private ILog GetFakeLogger()
        {
            var log = Substitute.For<ILog>();
            log.IsDebugEnabled.Returns(true);
            log.IsInfoEnabled.Returns(true);
            log.IsWarnEnabled.Returns(true);
            log.IsErrorEnabled.Returns(true);
            log.IsFatalEnabled.Returns(true);

            return log;
        }

        [Fact]
        public void log_with_calls_trace_calls_the_correct_loglevel_method()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log(LogLevel.Trace, new EventId(), "Trace log", null, null);

            // Assert
            logger.Logger.Received(1).Debug("Trace log", null);
        }

        [Fact]
        public void log_with_calls_debug_calls_the_correct_loglevel_method()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log(LogLevel.Debug, new EventId(), "Debug log", null, null);

            // Assert
            logger.Logger.Received(1).Debug("Debug log", null);
        }

        [Fact]
        public void log_with_calls_info_calls_the_correct_loglevel_method()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log(LogLevel.Information, new EventId(), "Info log", null, null);

            // Assert
            logger.Logger.Received(1).Info("Info log", null);
        }

        [Fact]
        public void log_with_calls_warn_calls_the_correct_loglevel_method()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log(LogLevel.Warning, new EventId(), "Warn log", null, null);

            // Assert
            logger.Logger.Received(1).Warn("Warn log", null);
        }

        [Fact]
        public void log_with_calls_error_calls_the_correct_loglevel_method()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log(LogLevel.Error, new EventId(), "Error log", null, null);

            // Assert
            logger.Logger.Received(1).Error("Error log", null);
        }

        [Fact]
        public void log_with_calls_fatal_calls_the_correct_loglevel_method()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log(LogLevel.Critical, new EventId(), "Fatal log", null, null);

            // Assert
            logger.Logger.Received(1).Fatal("Fatal log", null);
        }

        [Fact]
        public void log_with_calls_invalid_level_does_not_call_debug()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log((LogLevel)99, new EventId(), "Invalid log", null, null);

            // Assert
            logger.Logger.DidNotReceive().Debug("Invalid log", null);
        }

        [Fact]
        public void log_with_calls_invalid_level_does_not_call_info()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log((LogLevel)99, new EventId(), "Invalid log", null, null);

            // Assert
            logger.Logger.DidNotReceive().Info("Invalid log", null);
        }

        [Fact]
        public void log_with_calls_invalid_level_does_not_call_warn()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log((LogLevel)99, new EventId(), "Invalid log", null, null);

            // Assert
            logger.Logger.DidNotReceive().Warn("Invalid log", null);
        }

        [Fact]
        public void log_with_calls_invalid_level_does_not_call_error()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log((LogLevel)99, new EventId(), "Invalid log", null, null);

            // Assert
            logger.Logger.DidNotReceive().Error("Invalid log", null);
        }

        [Fact]
        public void log_with_calls_invalid_level_does_not_call_fatal()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger") { Logger = GetFakeLogger() };

            // Act
            logger.Log((LogLevel)99, new EventId(), "Invalid log", null, null);

            // Assert
            logger.Logger.DidNotReceive().Fatal("Invalid log", null);
        }

        [Fact]
        public void begin_scope_is_not_required_and_should_return_null()
        {
            // Arrange
            var logger = new Log4NetLogger("TestLogger");

            // Act
            var result = logger.BeginScope("Some data");

            // Assert
            result.Should().BeNull();
        }
    }
}