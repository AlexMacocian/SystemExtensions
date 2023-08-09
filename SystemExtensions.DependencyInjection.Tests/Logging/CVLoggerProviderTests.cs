using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Logging;

namespace SystemExtensions.DependencyInjection.Tests.Logging;

[TestClass]
public class CVLoggerProviderTests
{
    private readonly ILogsWriter logsWriterMock = Substitute.For<ILogsWriter>();
    private CVLoggerProvider cVLoggerProvider;

    [TestInitialize]
    public void TestInitialize()
    {
        this.cVLoggerProvider = new CVLoggerProvider(this.logsWriterMock);
    }

    [TestMethod]
    public void CreateLogger_CreatesNewLogger()
    {
        var logger = this.cVLoggerProvider.CreateLogger(string.Empty);

        logger.Should().NotBeNull();
    }

    [TestMethod]
    public void LogEntry_CallsLogWriter()
    {
        this.cVLoggerProvider.LogEntry(new Log());

        this.logsWriterMock.ReceivedWithAnyArgs().WriteLog(Arg.Any<Log>());
    }
}
