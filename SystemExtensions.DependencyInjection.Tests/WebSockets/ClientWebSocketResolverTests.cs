using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.WebSockets;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace SystemExtensions.NetStandard.DependencyInjection.Tests.WebSockets;

[TestClass]
public sealed class ClientWebSocketResolverTests
{
    private readonly ClientWebSocketResolver webSocketResolver = new();
    private readonly Slim.IServiceProvider serviceProviderMock = Substitute.For<Slim.IServiceProvider>();

    [TestMethod]
    public void CanResolve_IHttpClient_ReturnsTrue()
    {
        var type = typeof(IClientWebSocket<>);

        var canResolve = this.webSocketResolver.CanResolve(type);

        canResolve.Should().BeTrue();
    }

    [TestMethod]
    public void CanResolve_AnythingElse_ReturnsFalse()
    {
        var types = new Type[] { typeof(ClientWebSocket), typeof(object), typeof(string), typeof(ClientWebSocketResolverTests), typeof(int) };

        foreach (var type in types)
        {
            var canResolve = this.webSocketResolver.CanResolve(type);

            canResolve.Should().BeFalse();
        }
    }

    [TestMethod]
    public void Resolve_TypedClient_ReturnsIClientWebSocketResolver()
    {
        var loggerMock = Substitute.For<ILogger<string>>();
        this.serviceProviderMock.GetService(typeof(ILogger<string>)).Returns(loggerMock);

        var client = this.webSocketResolver.Resolve(this.serviceProviderMock, typeof(IClientWebSocket<string>));

        client.Should().BeAssignableTo<IClientWebSocket<string>>();
    }

    [TestMethod]
    public void Resolve_NonGenericType_Throws()
    {
        Action action = new(() =>
        {
            this.webSocketResolver.Resolve(this.serviceProviderMock, typeof(IClientWebSocket<>));
        });

        action.Should().Throw<Exception>();
    }

    [TestMethod]
    public void Resolve_RandomType_Throws()
    {
        Action action = new(() =>
        {
            this.webSocketResolver.Resolve(this.serviceProviderMock, typeof(string));
        });

        action.Should().Throw<Exception>();
    }
}
