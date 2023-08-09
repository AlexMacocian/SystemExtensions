using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Slim;
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
    private readonly IServiceProducer serviceProducerMock = Substitute.For<IServiceProducer>();
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

        producer.Should().Be(this.serviceProducerMock);
    }

    [TestMethod]
    public void Build_RegistersWithServiceProducer()
    {
        this.httpClientBuilder.Build();

        this.serviceProducerMock.Received().RegisterScoped(Arg.Any<Func<Slim.IServiceProvider, IHttpClient<object>>>(), false);
    }

    [TestMethod]
    public void Build_CreatesExpectedClient()
    {
        this.serviceProducerMock.When(u => u.RegisterScoped(Arg.Any<Func<Slim.IServiceProvider, IHttpClient<object>>>(), false))
            .Do(callInfo =>
            {
                var factory = callInfo.ArgAt<Func<Slim.IServiceProvider, IHttpClient<object>>>(0);
                var client = factory(Substitute.For<Slim.IServiceProvider>());
                client.BaseAddress.Should().BeNull();
                client.DefaultRequestHeaders.Should().BeEmpty();
                client.MaxResponseContentBufferSize.Should().Be(2147483647L);
                client.Timeout.Should().Be(TimeSpan.FromSeconds(100));
            });

        this.httpClientBuilder.Build();

    }

    [TestMethod]
    public void Build_WithBaseAddress_ReturnsClientWithBaseAddress()
    {
        this.serviceProducerMock.When(u => u.RegisterScoped(Arg.Any<Func<Slim.IServiceProvider, IHttpClient<object>>>(), false))
            .Do(callInfo =>
            {
                var factory = callInfo.ArgAt<Func<Slim.IServiceProvider, IHttpClient<object>>>(0);
                var client = factory(Substitute.For<Slim.IServiceProvider>());
                client.BaseAddress.Should().Be(this.baseAddress);
            });

        this.httpClientBuilder.WithBaseAddress(this.baseAddress)
            .Build();
    }

    [TestMethod]
    public void Build_WithDefaultRequestHeaders_CallsFactory()
    {
        this.serviceProducerMock.When(u => u.RegisterScoped(Arg.Any<Func<Slim.IServiceProvider, IHttpClient<object>>>(), false))
            .Do(callInfo =>
            {
                var factory = callInfo.ArgAt<Func<Slim.IServiceProvider, IHttpClient<object>>>(0);
                var client = factory(Substitute.For<Slim.IServiceProvider>());
                client.DefaultRequestHeaders.TryGetValues(SomeHeader, out var values);
                values.Should().HaveCount(1);
                values.FirstOrDefault().Should().Be(SomeValue);
            });

        this.httpClientBuilder.WithDefaultRequestHeadersSetup((headers) =>
        {
            headers.TryAddWithoutValidation(SomeHeader, SomeValue);
        }).Build();
    }

    [TestMethod]
    public void Build_WithMaxResponseBufferSize_ReturnsClientWithMaxResponseBufferSize()
    {
        this.serviceProducerMock.When(u => u.RegisterScoped(Arg.Any<Func<Slim.IServiceProvider, IHttpClient<object>>>(), false))
            .Do(callInfo =>
            {
                var factory = callInfo.ArgAt<Func<Slim.IServiceProvider, IHttpClient<object>>>(0);
                var client = factory(Substitute.For<Slim.IServiceProvider>());
                client.MaxResponseContentBufferSize.Should().Be(100);
            });

        this.httpClientBuilder.WithMaxResponseBufferSize(100)
            .Build();
    }

    [TestMethod]
    public void Build_WithTimeout_ReturnsClientWithTimeout()
    {
        this.serviceProducerMock.When(u => u.RegisterScoped(Arg.Any<Func<Slim.IServiceProvider, IHttpClient<object>>>(), false))
            .Do(callInfo =>
            {
                var factory = callInfo.ArgAt<Func<Slim.IServiceProvider, IHttpClient<object>>>(0);
                var client = factory(Substitute.For<Slim.IServiceProvider>());
                client.Timeout.Should().Be(TimeSpan.FromSeconds(5));
            });

        this.httpClientBuilder.WithTimeout(TimeSpan.FromSeconds(5))
            .Build();
    }

    [TestMethod]
    public void Build_WithMessageHandler_CallsMessageHandler()
    {
        var handlerMock = new HttpMessageHandlerMock();
        this.serviceProducerMock.When(u => u.RegisterScoped(Arg.Any<Func<Slim.IServiceProvider, IHttpClient<object>>>(), false))
            .Do(async callInfo =>
            {
                var factory = callInfo.ArgAt<Func<Slim.IServiceProvider, IHttpClient<object>>>(0);
                var client = factory(Substitute.For<Slim.IServiceProvider>());
                await client.GetAsync(this.baseAddress);
                handlerMock.Called.Should().BeTrue();
            });

        this.httpClientBuilder.WithMessageHandler(sp => handlerMock)
            .Build();
    }

    [TestMethod]
    public async Task HttpClientBuilder_RegistersClientCorrectly_IServiceProviderReturnsExpected()
    {
        var container = new ServiceManager();
        var messageHandler = new HttpMessageHandlerMock();
        new HttpClientBuilder<object>(container)
            .WithBaseAddress(this.baseAddress)
            .WithDefaultRequestHeadersSetup(header => header.TryAddWithoutValidation(SomeHeader, SomeValue))
            .WithMaxResponseBufferSize(5)
            .WithMessageHandler(sp => messageHandler)
            .WithTimeout(TimeSpan.FromSeconds(5))
            .Build();

        var client = container.GetService<IHttpClient<object>>();
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
