using System.Extensions;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SystemExtensions.NetStandard.Tests.Logging.Models;

namespace System.Logging.Tests;

[TestClass]
public class ScopedLoggerTests
{
    private const string Flow = "Flow";
    private const string Message = "Some message";
    private readonly CachingLogger<ScopedLoggerTests> cachingLogger = new();

    [TestMethod]
    public void CreateLogger_CreatesNewLogger()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.CreateLogger_CreatesNewLogger), Flow);
        scopedLogger.Should().BeOfType<ScopedLogger<ScopedLoggerTests>>();
    }

    [TestMethod]
    public void CreateLogger_NullScope_ThrowsArgumentNullException()
    {
        var action = new Action(() =>
        {
            var scopedLogger = this.cachingLogger.CreateScopedLogger(null, Flow);
        });


        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateLogger_NullFlow_ReturnsScopedLogger()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.CreateLogger_NullFlow_ReturnsScopedLogger), null);
    }

    [TestMethod]
    public void LogInformation_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogInformation_LogsExpected), Flow);
        var expectedMessage = $"[{Flow}] {nameof(this.LogInformation_LogsExpected)}: {Message}";

        scopedLogger.LogInformation(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogInformation_EmptyFlow_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogInformation_LogsExpected), string.Empty);
        var expectedMessage = $"{nameof(this.LogInformation_LogsExpected)}: {Message}";

        scopedLogger.LogInformation(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogInformation_WhitespaceFlow_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogInformation_LogsExpected), "  ");
        var expectedMessage = $"{nameof(this.LogInformation_LogsExpected)}: {Message}";

        scopedLogger.LogInformation(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogDebug_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogDebug_LogsExpected), Flow);
        var expectedMessage = $"[{Flow}] {nameof(this.LogDebug_LogsExpected)}: {Message}";

        scopedLogger.LogDebug(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogDebug_EmptyFlow_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogDebug_EmptyFlow_LogsExpected), string.Empty);
        var expectedMessage = $"{nameof(this.LogDebug_EmptyFlow_LogsExpected)}: {Message}";

        scopedLogger.LogDebug(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogDebug_WhitespaceFlow_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogDebug_WhitespaceFlow_LogsExpected), "  ");
        var expectedMessage = $"{nameof(this.LogDebug_WhitespaceFlow_LogsExpected)}: {Message}";

        scopedLogger.LogDebug(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogWarning_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogWarning_LogsExpected), Flow);
        var expectedMessage = $"[{Flow}] {nameof(this.LogWarning_LogsExpected)}: {Message}";

        scopedLogger.LogWarning(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogWarning_EmptyFlow_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogWarning_EmptyFlow_LogsExpected), string.Empty);
        var expectedMessage = $"{nameof(this.LogWarning_EmptyFlow_LogsExpected)}: {Message}";

        scopedLogger.LogWarning(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogWarning_WhitespaceFlow_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogWarning_WhitespaceFlow_LogsExpected), "  ");
        var expectedMessage = $"{nameof(this.LogWarning_WhitespaceFlow_LogsExpected)}: {Message}";

        scopedLogger.LogWarning(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogError_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogError_LogsExpected), Flow);
        var expectedMessage = $"[{Flow}] {nameof(this.LogError_LogsExpected)}: {Message}";

        scopedLogger.LogError(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogError_EmptyFlow_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogError_EmptyFlow_LogsExpected), string.Empty);
        var expectedMessage = $"{nameof(this.LogError_EmptyFlow_LogsExpected)}: {Message}";

        scopedLogger.LogError(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogError_WhitespaceFlow_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogError_WhitespaceFlow_LogsExpected), "  ");
        var expectedMessage = $"{nameof(this.LogError_WhitespaceFlow_LogsExpected)}: {Message}";

        scopedLogger.LogError(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogCritical_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogCritical_LogsExpected), Flow);
        var expectedMessage = $"[{Flow}] {nameof(this.LogCritical_LogsExpected)}: {Message}";

        scopedLogger.LogCritical(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogCritical_EmptyFlow_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogCritical_EmptyFlow_LogsExpected), string.Empty);
        var expectedMessage = $"{nameof(this.LogCritical_EmptyFlow_LogsExpected)}: {Message}";

        scopedLogger.LogCritical(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogCritical_WhitespaceFlow_LogsExpected()
    {
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogCritical_WhitespaceFlow_LogsExpected), "  ");
        var expectedMessage = $"{nameof(this.LogCritical_WhitespaceFlow_LogsExpected)}: {Message}";

        scopedLogger.LogCritical(Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogWarning_WithException_LogsExpected()
    {
        var exception = new Exception();
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogWarning_WithException_LogsExpected), Flow);
        var expectedMessage = $"[{Flow}] {nameof(this.LogWarning_WithException_LogsExpected)}: {Message}";

        scopedLogger.LogWarning(exception, Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogError_WithException_LogsExpected()
    {
        var exception = new Exception();
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogError_WithException_LogsExpected), Flow);
        var expectedMessage = $"[{Flow}] {nameof(this.LogError_WithException_LogsExpected)}: {Message}";

        scopedLogger.LogError(exception, Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }

    [TestMethod]
    public void LogCritical_WithException_LogsExpected()
    {
        var exception = new Exception();
        var scopedLogger = this.cachingLogger.CreateScopedLogger(nameof(this.LogCritical_WithException_LogsExpected), Flow);
        var expectedMessage = $"[{Flow}] {nameof(this.LogCritical_WithException_LogsExpected)}: {Message}";

        scopedLogger.LogCritical(exception, Message);

        this.cachingLogger.LogCache.Should().HaveCount(1);
        this.cachingLogger.LogCache.First().Should().Be(expectedMessage);
    }
}