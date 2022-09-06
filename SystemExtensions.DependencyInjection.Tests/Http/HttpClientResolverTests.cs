using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Http;
using System.Net.Http;

namespace SystemExtensions.DependencyInjection.Tests.Http;

[TestClass]
public class HttpClientResolverTests
{
    private readonly HttpClientResolver httpClientResolver = new();
    private readonly Mock<Slim.IServiceProvider> serviceProviderMock = new();

    [TestMethod]
    public void CanResolve_IHttpClient_ReturnsTrue()
    {
        var type = typeof(IHttpClient<>);

        var canResolve = this.httpClientResolver.CanResolve(type);

        canResolve.Should().BeTrue();
    }

    [TestMethod]
    public void CanResolve_AnythingElse_ReturnsFalse()
    {
        var types = new Type[] { typeof(HttpClient), typeof(object), typeof(string), typeof(HttpClientResolverTests), typeof(int) };

        foreach (var type in types)
        {
            var canResolve = this.httpClientResolver.CanResolve(type);

            canResolve.Should().BeFalse();
        }
    }

    [TestMethod]
    public void Resolve_TypedClient_ReturnsIHttpClient()
    {
        var client = this.httpClientResolver.Resolve(this.serviceProviderMock.Object, typeof(IHttpClient<string>));

        client.Should().BeAssignableTo<IHttpClient<string>>();
    }

    [TestMethod]
    public void Resolve_NonGenericType_Throws()
    {
        Action action = new(() =>
        {
            this.httpClientResolver.Resolve(this.serviceProviderMock.Object, typeof(IHttpClient<>));
        });

        action.Should().Throw<Exception>();
    }

    [TestMethod]
    public void Resolve_RandomType_Throws()
    {
        Action action = new(() =>
        {
            this.httpClientResolver.Resolve(this.serviceProviderMock.Object, typeof(string));
        });

        action.Should().Throw<Exception>();
    }
}
