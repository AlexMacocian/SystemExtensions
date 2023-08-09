using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Configuration;
using System.Extensions.Configuration;

namespace SystemExtensions.DependencyInjection.Tests.Configuration;

[TestClass]
public class UpdateableOptionsWrapperTests
{
    private const string Value = "hello";

    private UpdateableOptionsWrapper<string> optionsWrapper;
    private readonly IOptionsManager optionsManagerMock = Substitute.For<IOptionsManager>();

    [TestInitialize]
    public void TestInitialize()
    {
        this.optionsWrapper = new UpdateableOptionsWrapper<string>(this.optionsManagerMock, Value);
    }

    [TestMethod]
    public void GetValue_ReturnsValue()
    {
        this.optionsManagerMock.GetOptions<string>().Throws<Exception>();

        var value = this.optionsWrapper.Value;

        value.Should().Be(Value);
    }

    [TestMethod]
    public void UpdateOption_CallsOptionsManager()
    {
        this.optionsWrapper.UpdateOption();

        this.optionsManagerMock.ReceivedWithAnyArgs().UpdateOptions(Arg.Any<string>());
    }
}
