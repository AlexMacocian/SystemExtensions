using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Configuration;

namespace SystemExtensions.DependencyInjection.Tests.Configuration;

[TestClass]
public class LiveOptionsResolverTests
{
    private readonly LiveOptionsResolver liveOptionsResolver = new();
    private readonly Slim.IServiceProvider serviceProviderMock = Substitute.For<Slim.IServiceProvider>();
    private readonly IOptionsManager optionsManagerMock = Substitute.For<IOptionsManager>();

    [TestMethod]
    public void CanResolve_ILiveOptions_ReturnsTrue()
    {
        var type = typeof(ILiveOptions<string>);

        var canResolve = this.liveOptionsResolver.CanResolve(type);

        canResolve.Should().BeTrue();
    }

    [TestMethod]
    public void CanResolve_AnythingElse_ReturnsFalse()
    {
        var types = new Type[] { typeof(object), typeof(string), typeof(LiveOptionsResolverTests), typeof(int) };

        foreach (var type in types)
        {
            var canResolve = this.liveOptionsResolver.CanResolve(type);

            canResolve.Should().BeFalse();
        }
    }

    [TestMethod]
    public void Resolve_ILiveOptions_ReturnsILiveOptions()
    {
        this.SetupServiceProvider();

        var liveOptions = this.liveOptionsResolver.Resolve(this.serviceProviderMock, typeof(ILiveOptions<string>));

        liveOptions.Should().BeAssignableTo<ILiveOptions<string>>();
    }

    [TestMethod]
    public void Resolve_AnythingElse_Throws()
    {
        this.SetupServiceProvider();

        Action action = new(() =>
        {
            this.liveOptionsResolver.Resolve(this.serviceProviderMock, typeof(string));
        });

        action.Should().Throw<Exception>();
    }

    private void SetupServiceProvider()
    {
        this.serviceProviderMock.GetService<IOptionsManager>().Returns(this.optionsManagerMock);
    }
}
