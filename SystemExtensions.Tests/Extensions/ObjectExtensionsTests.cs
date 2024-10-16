using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Extensions.Tests;

[TestClass]
public class ObjectExtensionsTests
{
    [TestMethod]
    public void ThrowIfNull_NetCore_ThrowsWithCorrectName()
    {
        object? obj = null;
        try
        {
            System.Core.Extensions.ObjectExtensions.ThrowIfNull(obj);
        }
        catch (ArgumentNullException ex)
        {
            ex.ParamName.Should().Be("obj");
            return;
        }

        Assert.Fail("Null object should throw");
    }

    [TestMethod]
    public void ThrowIfNull_NetStandard_ThrowsWithCorrectName()
    {
        object? obj = null;
        try
        {
            ObjectExtensions.ThrowIfNull(obj, nameof(obj));
        }
        catch (ArgumentNullException ex)
        {
            ex.ParamName.Should().Be("obj");
            return;
        }

        Assert.Fail("Null object should throw");
    }
}
