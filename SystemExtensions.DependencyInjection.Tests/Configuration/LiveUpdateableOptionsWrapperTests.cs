using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Configuration;

namespace SystemExtensions.DependencyInjection.Tests.Configuration;

[TestClass]
public class LiveUpdateableOptionsWrapperTests
{
    private LiveUpdateableOptionsWrapper<string> optionsWrapper;
    private readonly IOptionsManager optionsManagerMock = Substitute.For<IOptionsManager>();

    [TestInitialize]
    public void TestInitialize()
    {
        this.optionsWrapper = new LiveUpdateableOptionsWrapper<string>(this.optionsManagerMock);
    }

    [TestMethod]
    public void GetValue_ReturnsValue()
    {
        this.optionsManagerMock.GetOptions<string>().Returns("hello");

        var value = this.optionsWrapper.Value;

        value.Should().Be("hello");
    }

    [TestMethod]
    public void UpdateOption_CallsOptionsManager()
    {
        this.optionsWrapper.UpdateOption();

        this.optionsManagerMock.ReceivedWithAnyArgs().UpdateOptions(Arg.Any<string>());
    }
}
