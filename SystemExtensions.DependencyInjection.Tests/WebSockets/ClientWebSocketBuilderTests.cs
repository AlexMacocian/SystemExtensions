using FluentAssertions;
using global::SystemExtensions.NetStandard.DependencyInjection.Tests.Http.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Extensions;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace SystemExtensions.NetStandard.DependencyInjection.Tests.WebSockets;
[TestClass]
public sealed class HttpClientBuilderTests
{
    private readonly ClientWebSocketBuilder<object> clientWebSocketBuilder;
    private readonly IServiceCollection serviceProducerMock = Substitute.For<IServiceCollection>();

    public HttpClientBuilderTests()
    {
        this.clientWebSocketBuilder = new ClientWebSocketBuilder<object>(this.serviceProducerMock);
    }

    [TestMethod]
    public void Constructor_NullServiceProducer_Throws()
    {
        var action = () => new ClientWebSocketBuilder<object>(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void WithInnerWebSocket_NullHandler_Throws()
    {
        var action = () => this.clientWebSocketBuilder.WithInnerWebSocket(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void WIthInnerWebSocketFactory_NullBaseAddress_Throws()
    {
        var action = () => this.clientWebSocketBuilder.WithInnerWebSocketFactory(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Build_ReturnsServiceProducer()
    {
        var producer = this.clientWebSocketBuilder.Build();

        producer.Should().BeEquivalentTo(this.serviceProducerMock);
    }

    [TestMethod]
    public async Task ClientWebSocketBuilder_RegistersClientCorrectly_IServiceProviderReturnsExpected()
    {
        var container = new ServiceCollection();
        var innerWebSocket = new ClientWebSocket();
        container.RegisterClientWebSocket<object>()
            .WithInnerWebSocket(innerWebSocket)
            .Build();

        var di = container.BuildServiceProvider();
        var client = di.GetService<IClientWebSocket<object>>();
        client.Should().NotBeNull();
    }
}
