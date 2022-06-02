using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace System.Tests;

[TestClass]
public class AsyncLazyTests
{
    private const string Value = "Value";

    [TestMethod]
    public async Task ValueFactoryConstructor_AwaitsAndReturnsValue()
    {
        var asyncLazy = new AsyncLazy<string>(() => Value);

        var value = await asyncLazy;

        value.Should().Be(Value);
    }

    [TestMethod]
    public async Task TaskFactoryConstructor_AwaitsAndReturnsValue()
    {
        var asyncLazy = new AsyncLazy<string>(GetValueTask);

        var value = await asyncLazy;

        value.Should().Be(Value);
    }

    [TestMethod]
    public async Task Value_ReturnsValue()
    {
        var asyncLazy = new AsyncLazy<string>(GetValueTask);

        var value = await asyncLazy.Value;

        value.Should().Be(Value);
    }

    private static async Task<string> GetValueTask()
    {
        await Task.Delay(10);
        return Value;
    }
}
