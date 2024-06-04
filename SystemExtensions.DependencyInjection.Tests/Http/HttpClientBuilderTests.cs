using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SystemExtensions.NetStandard.DependencyInjection.Tests.Http.Models;

namespace SystemExtensions.NetStandard.DependencyInjection.Tests.Http;

[TestClass]
public sealed class HttpClientBuilderTests
{
    private const string SomeHeader = "SomeHeader";
    private const string SomeValue = "SomeValue";

    private readonly HttpClientBuilder<object> httpClientBuilder;
    private readonly IServiceCollection serviceProducerMock = Substitute.For<IServiceCollection>();
    private readonly Uri baseAddress = new("http://contoso.co");

    public HttpClientBuilderTests()
    {
        this.httpClientBuilder = new HttpClientBuilder<object>(this.serviceProducerMock);
    }

    [TestMethod]
    public void Constructor_NullServiceProducer_Throws()
    {
        var action = () => new HttpClientBuilder<object>(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void WithMessageHandler_NullHandler_Throws()
    {
        var action = () => this.httpClientBuilder.WithMessageHandler(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void WithBaseAddress_NullBaseAddress_Throws()
    {
        var action = () => this.httpClientBuilder.WithBaseAddress(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void WithDefaultRequestHeadersSetup_NullSetup_Throws()
    {
        var action = () => this.httpClientBuilder.WithDefaultRequestHeadersSetup(null);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Build_ReturnsServiceProducer()
    {
        var producer = this.httpClientBuilder.Build();

        producer.Should().BeEquivalentTo(this.serviceProducerMock);
    }

    [TestMethod]
    public async Task HttpClientBuilder_RegistersClientCorrectly_IServiceProviderReturnsExpected()
    {
        var container = new ServiceCollection();
        var messageHandler = new HttpMessageHandlerMock();
        new HttpClientBuilder<object>(container)
            .WithBaseAddress(this.baseAddress)
            .WithDefaultRequestHeadersSetup(header => header.TryAddWithoutValidation(SomeHeader, SomeValue))
            .WithMaxResponseBufferSize(5)
            .WithMessageHandler(sp => messageHandler)
            .WithTimeout(TimeSpan.FromSeconds(5))
            .Build();

        var di = container.BuildServiceProvider();
        var client = di.GetService<IHttpClient<object>>();
        await client.GetAsync("");

        client.Should().NotBeNull();
        client.BaseAddress.Should().Be(this.baseAddress);
        client.DefaultRequestHeaders.Should().HaveCount(1);
        client.DefaultRequestHeaders.First().Key.Should().Be(SomeHeader);
        client.DefaultRequestHeaders.First().Value.Should().HaveCount(1);
        client.DefaultRequestHeaders.First().Value.First().Should().Be(SomeValue);
        client.MaxResponseContentBufferSize.Should().Be(5);
        client.Timeout.Should().Be(TimeSpan.FromSeconds(5));
        messageHandler.Called.Should().BeTrue();
    }
}
