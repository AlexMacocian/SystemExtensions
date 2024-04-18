using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Tests;

[TestClass]
public class CircularBufferTests
{
    [TestMethod]
    public void Constructor_WithCapacity_RespectsCapacity()
    {
        var buffer = new CircularBuffer<int>(10);
        
        buffer.Count.Should().Be(0);

        for (var i = 0; i < 11; i++)
        {
            buffer.Add(i);
        }

        buffer.Count.Should().Be(10);
    }

    [TestMethod]
    public void Constructor_WithBuffer_ReturnsExpectedElements()
    {
        var array = new int[10];
        for(var i = 0; i < 10; i++)
        {
            array[i] = i;
        }

        var buffer = new CircularBuffer<int>(array, 0, 9);

        buffer.Count.Should().Be(10);
        buffer.Should().BeEquivalentTo(array);
    }

    [TestMethod]
    public void Constructor_WithBuffer_RespectsOrder()
    {
        var expectedArray = new int[] { 3, 4, 5, 6, 7, 8, 9, 0, 1, 2 };
        var array = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        var buffer = new CircularBuffer<int>(array, 3, 2);

        buffer.Count.Should().Be(10);
        buffer.Should().BeEquivalentTo(expectedArray);
    }

    [TestMethod]
    public void Constructor_WithBuffer_RespectsInternalBuffer()
    {
        var buffer = new CircularBuffer<int>(new int[] { 0, 1, 2, 3, 4, 5 });

        buffer.Count.Should().Be(6);
        buffer.Should().BeEquivalentTo(new int[] { 0, 1, 2, 3, 4, 5 });
    }

    [TestMethod]
    public void AddItem_AddsItem()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            1
        };

        buffer.Should().HaveCount(1);
        buffer.First().Should().Be(1);
    }

    [TestMethod]
    public void AddItem_AddsItem_RespectsCapacity()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10
        };

        buffer.Add(0);

        buffer.Should().HaveCount(10);
        buffer.Last().Should().Be(0);
        buffer.First().Should().Be(2);
    }

    [TestMethod]
    public void RemoveItem_RemovesDesiredItem()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10
        };

        var result = buffer.Remove(5);

        result.Should().BeTrue();
        buffer.Count.Should().Be(9);
        buffer.Should().BeEquivalentTo(new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10 });
    }

    [TestMethod]
    public void RemoveItem_UnknownItem_DoesNotRemoveItem()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10
        };

        var result = buffer.Remove(15);

        result.Should().BeFalse();
        buffer.Count.Should().Be(10);
        buffer.Should().BeEquivalentTo(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
    }

    [TestMethod]
    public void Contains_ItemExists_ReturnsTrue()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10
        };

        var result = buffer.Contains(5);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void Contains_ItemDoesNotExists_ReturnsFalse()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10
        };

        var result = buffer.Contains(15);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void Clear_ClearsBuffer()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10
        };

        buffer.Clear();

        buffer.Count.Should().Be(0);
        buffer.Should().BeEquivalentTo(Array.Empty<int>());
    }

    [TestMethod]
    public void DeepClear_ClearsBuffer()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10
        };

        buffer.DeepClear();

        buffer.Count.Should().Be(0);
        buffer.Should().BeEquivalentTo(Array.Empty<int>());
    }

    [TestMethod]
    public void Insert_FullBuffer_ThrowsException()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10
        };

        var action = () =>
        {
            buffer.Insert(0, 1);
        };

        action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Insert_ShouldInsertItemAtPosition()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
        };

        buffer.Insert(0, 0);

        buffer.Should().BeEquivalentTo(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
    }

    [TestMethod]
    public void RemoveAt_RemovesExpectedItem()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
        };

        buffer.RemoveAt(0);

        buffer.Should().BeEquivalentTo(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
    }

    [TestMethod]
    public void Indexer_ReturnsExpectedItem()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
        };

        buffer[5].Should().Be(5);
    }

    [TestMethod]
    public void Indexer_SetsExpectedItem()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
        };

        buffer[6] = 5;

        buffer.Should().BeEquivalentTo(new int[] { 0, 1, 2, 3, 4, 5, 5, 7, 8, 9 });
    }

    [TestMethod]
    public void CircularBuffer_AddAndRemoveOperations_PerformAsExpected()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
        };

        buffer.Add(10);
        buffer.Remove(9);
        buffer.RemoveAt(5);
        buffer.Add(11);

        buffer.Should().BeEquivalentTo(new int[] { 1, 2, 3, 4, 5, 7, 8, 10, 11 });
    }

    [TestMethod]
    public void CircularBuffer_AddItems_RotatesBuffer()
    {
        var buffer = new CircularBuffer<int>(10)
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
        };

        for(var i = 0; i < 10; i++)
        {
            buffer.Add(i + 10);
        }

        buffer.Should().BeEquivalentTo(new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
    }
}
