using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Logging;

namespace SystemExtensions.DependencyInjection.Tests.Logging;

[TestClass]
public class CVLoggerTests
{
    private readonly ICVLoggerProvider cvLoggerProviderMock = Substitute.For<ICVLoggerProvider>();
    private CVLogger cVLogger;

    [TestInitialize]
    public void TestInitialize()
    {
        this.cVLogger = new CVLogger(string.Empty, this.cvLoggerProviderMock);
    }

    [TestMethod]
    public void BeginScope_ReturnsNull()
    {
        var scope = this.cVLogger.BeginScope(string.Empty);

        scope.Should().BeNull();
    }

    [TestMethod]
    [DataRow(LogLevel.Debug)]
    [DataRow(LogLevel.Trace)]
    [DataRow(LogLevel.Information)]
    [DataRow(LogLevel.Warning)]
    [DataRow(LogLevel.Error)]
    [DataRow(LogLevel.Critical)]
    public void IsEnabled_OnAllLogLevels_ReturnsTrue(LogLevel logLevel)
    {
        var enabled = this.cVLogger.IsEnabled(logLevel);

        enabled.Should().BeTrue();
    }

    [TestMethod]
    public void Log_CallsFormatter()
    {
        var called = false;
        Func<string, Exception, string> messageFormatter = new((state, exception) =>
        {
            called = true;
            return string.Empty;
        });

        this.cVLogger.Log(LogLevel.Debug, new EventId(), "Some message", new Exception(), messageFormatter);

        called.Should().BeTrue();
    }

    [TestMethod]
    public void Log_CallsLogsProvider()
    {
        this.cVLogger.Log(LogLevel.Debug, new EventId(), "Some message", new Exception(), new Func<string, Exception, string>((s, e) => string.Empty));

        this.cvLoggerProviderMock.ReceivedWithAnyArgs().LogEntry(Arg.Any<Log>());
    }
}
