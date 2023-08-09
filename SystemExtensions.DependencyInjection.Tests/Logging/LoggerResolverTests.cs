using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Logging;

namespace SystemExtensions.DependencyInjection.Tests.Logging;

[TestClass]
public class LoggerResolverTests
{
    private readonly Slim.IServiceProvider serviceProviderMock = Substitute.For<Slim.IServiceProvider>();
    private readonly ILoggerFactory loggerFactoryMock = Substitute.For<ILoggerFactory>();
    private readonly ILogger loggerMock = Substitute.For<ILogger>();
    private readonly LoggerResolver loggerResolver = new();

    [TestMethod]
    public void CanResolve_ILogger_ReturnsTrue()
    {
        var type = typeof(ILogger);

        var canResolve = this.loggerResolver.CanResolve(type);

        canResolve.Should().BeTrue();
    }

    [TestMethod]
    public void CanResolve_GenericILogger_ReturnsTrue()
    {
        var type = typeof(ILogger<string>);

        var canResolve = this.loggerResolver.CanResolve(type);

        canResolve.Should().BeTrue();
    }

    [TestMethod]
    public void CanResolve_AnythingElse_ReturnsFalse()
    {
        var types = new Type[] { typeof(CVLogger), typeof(object), typeof(string), typeof(LoggerResolverTests), typeof(int) };

        foreach(var type in types)
        {
            var canResolve = this.loggerResolver.CanResolve(type);

            canResolve.Should().BeFalse();
        }   
    }

    [TestMethod]
    public void Resolve_ILogger_ReturnsILogger()
    {
        this.SetupIServiceProvider();

        var logger = this.loggerResolver.Resolve(this.serviceProviderMock, typeof(ILogger));

        logger.Should().BeAssignableTo<ILogger>();
    }

    [TestMethod]
    public void Resolve_TypedILogger_ReturnsTypedILogger()
    {
        this.SetupIServiceProvider();

        var logger = this.loggerResolver.Resolve(this.serviceProviderMock, typeof(ILogger<string>));

        logger.Should().BeAssignableTo<ILogger<string>>();
    }

    [TestMethod]
    public void Resolve_RandomType_Throws()
    {
        this.SetupIServiceProvider();

        Action action = new(() =>
        {
            this.loggerResolver.Resolve(this.serviceProviderMock, typeof(string));
        });

        action.Should().Throw<Exception>();
    }

    [TestMethod]
    public void Resolve_NonTypedGeneric_Throws()
    {
        this.SetupIServiceProvider();

        Action action = new(() =>
        {
            this.loggerResolver.Resolve(this.serviceProviderMock, typeof(ILogger<>));
        });

        action.Should().Throw<Exception>();
    }

    private void SetupIServiceProvider()
    {
        this.serviceProviderMock.GetService<ILoggerFactory>().Returns(this.loggerFactoryMock);

        this.loggerFactoryMock.CreateLogger(Arg.Any<string>()).Returns(this.loggerMock);
    }
}
