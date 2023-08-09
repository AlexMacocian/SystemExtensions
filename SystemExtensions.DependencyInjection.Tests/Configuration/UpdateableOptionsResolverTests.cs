using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Configuration;

namespace SystemExtensions.DependencyInjection.Tests.Configuration;

[TestClass]
public class UpdateableOptionsResolverTests
{
    private readonly UpdateableOptionsResolver updateableOptionsResolver = new();
    private readonly Slim.IServiceProvider serviceProviderMock = Substitute.For<Slim.IServiceProvider>();
    private readonly IOptionsManager optionsManagerMock = Substitute.For<IOptionsManager>();

    [TestMethod]
    public void CanResolve_ILiveOptions_ReturnsTrue()
    {
        var type = typeof(IUpdateableOptions<string>);

        var canResolve = this.updateableOptionsResolver.CanResolve(type);

        canResolve.Should().BeTrue();
    }

    [TestMethod]
    public void CanResolve_AnythingElse_ReturnsFalse()
    {
        var types = new Type[] { typeof(object), typeof(string), typeof(UpdateableOptionsResolverTests), typeof(int) };

        foreach (var type in types)
        {
            var canResolve = this.updateableOptionsResolver.CanResolve(type);

            canResolve.Should().BeFalse();
        }
    }

    [TestMethod]
    public void Resolve_IUpdateableOptions_ReturnsIUpdateableOptions()
    {
        this.SetupServiceProvider();

        var liveOptions = this.updateableOptionsResolver.Resolve(this.serviceProviderMock, typeof(IUpdateableOptions<string>));

        liveOptions.Should().BeAssignableTo<IUpdateableOptions<string>>();
    }

    [TestMethod]
    public void Resolve_AnythingElse_Throws()
    {
        this.SetupServiceProvider();

        Action action = new(() =>
        {
            this.updateableOptionsResolver.Resolve(this.serviceProviderMock, typeof(string));
        });

        action.Should().Throw<Exception>();
    }

    private void SetupServiceProvider()
    {
        this.serviceProviderMock.GetService<IOptionsManager>().Returns(this.optionsManagerMock);
    }
}
