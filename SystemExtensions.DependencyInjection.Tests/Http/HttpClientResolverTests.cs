using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Net.Http;

namespace SystemExtensions.DependencyInjection.Tests.Http;

[TestClass]
public class HttpClientResolverTests
{
    private readonly HttpClientResolver httpClientResolver = new();
    private readonly Slim.IServiceProvider serviceProviderMock = Substitute.For<Slim.IServiceProvider>();

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
    public void Resolve_NonGenericType_Throws()
    {
        Action action = new(() =>
        {
            this.httpClientResolver.Resolve(this.serviceProviderMock, typeof(IHttpClient<>));
        });

        action.Should().Throw<Exception>();
    }

    [TestMethod]
    public void Resolve_RandomType_Throws()
    {
        Action action = new(() =>
        {
            this.httpClientResolver.Resolve(this.serviceProviderMock, typeof(string));
        });

        action.Should().Throw<Exception>();
    }
}
