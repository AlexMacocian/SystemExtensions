using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Configuration;

namespace SystemExtensions.DependencyInjection.Tests.Configuration;

[TestClass]
public class LiveUpdateableOptionsResolverTests
{
    private readonly LiveUpdateableOptionsResolver liveUpdateableOptionsResolver = new();
    private readonly Slim.IServiceProvider serviceProviderMock = Substitute.For<Slim.IServiceProvider>();
    private readonly IOptionsManager optionsManagerMock = Substitute.For<IOptionsManager>();

    [TestMethod]
    public void CanResolve_ILiveUpdateableOptions_ReturnsTrue()
    {
        var type = typeof(ILiveUpdateableOptions<string>);

        var canResolve = this.liveUpdateableOptionsResolver.CanResolve(type);

        canResolve.Should().BeTrue();
    }

    [TestMethod]
    public void CanResolve_AnythingElse_ReturnsFalse()
    {
        var types = new Type[] { typeof(object), typeof(string), typeof(LiveOptionsResolverTests), typeof(int) };

        foreach (var type in types)
        {
            var canResolve = this.liveUpdateableOptionsResolver.CanResolve(type);

            canResolve.Should().BeFalse();
        }
    }

    [TestMethod]
    public void Resolve_ILiveUpdateableOptions_ReturnsILiveUpdateableOptions()
    {
        this.SetupServiceProvider();

        var liveOptions = this.liveUpdateableOptionsResolver.Resolve(this.serviceProviderMock, typeof(ILiveUpdateableOptions<string>));

        liveOptions.Should().BeAssignableTo<ILiveUpdateableOptions<string>>();
    }

    [TestMethod]
    public void Resolve_AnythingElse_Throws()
    {
        this.SetupServiceProvider();

        Action action = new(() =>
        {
            this.liveUpdateableOptionsResolver.Resolve(this.serviceProviderMock, typeof(string));
        });

        action.Should().Throw<Exception>();
    }

    private void SetupServiceProvider()
    {
        this.serviceProviderMock.GetService<IOptionsManager>().Returns(this.optionsManagerMock);
    }
}
