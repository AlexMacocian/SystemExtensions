using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Structures.BitStructures.Tests
{
    [TestClass]
    public class Int64BitStructTests
    {
        [DataTestMethod]
        [DataRow(-1L)]
        [DataRow(long.MaxValue)]
        public void TestSetValueInt(long value)
        {
            Int64BitStruct int64 = value;
            Assert.IsTrue(int64 == value);
        }

        [DataTestMethod]
        [DataRow(1UL)]
        [DataRow(uint.MaxValue)]
        public void TestSetValueUint(ulong value)
        {
            Int64BitStruct int64 = value;
            Assert.IsTrue(int64 == value);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(long.MaxValue)]
        [DataRow(long.MinValue)]
        public void TestGetBit(long value)
        {
            Int64BitStruct int64 = value;
            Assert.IsTrue(value >= 0L ? int64.Bit63 == 0 : int64.Bit63 == 1);
        }

        [DataTestMethod]
        [DataRow(0U)]
        [DataRow(1U)]
        public void TestSetBit(ulong value)
        {
            Int64BitStruct int64 = 0L;
            int64.Bit0 = value;
            Assert.IsTrue(int64 == value);
        }

        [DataTestMethod]
        [DataRow(int.MaxValue, int.MinValue)]
        public void TestLowHigh(int low, int high)
        {
            Int64BitStruct int64 = new Int64BitStruct(low, high);
            Int32BitStruct lowStruct = low;
            Int32BitStruct highStruct = high;
            Assert.IsTrue(int64.Low == lowStruct);
            Assert.IsTrue(int64.High == highStruct);
            unchecked
            {
                Assert.IsTrue(int64 == ((ulong)low + ((ulong)high << 32)));
            }
        }

        [TestMethod]
        public void TestUseCaseExample()
        {
            // Set value = -1, represented as MSB being 1 and the rest 0.
            Int64BitStruct int64 = long.MinValue;

            // Test that MSB is 1.
            Assert.IsTrue(int64.Bit63 == 1);

            // Set MSB to 0 and test that everything else is 0.
            int64.Bit63 = 0;
            Assert.IsTrue(int64 == 0UL);

            // Set the least 31 significant bits (all besides MSB) to 1.
            int64 = long.MaxValue;

            // Set the MSB to 1 and test that everything is 1.
            int64.Bit63 = 1;
            Assert.IsTrue(int64 == ulong.MaxValue);
        }

        [TestMethod]
        public void TestUseCaseExample2()
        {
            // Set all bits to 1. Check low and high values to be equal to having all 32 bits set.
            Int64BitStruct int64 = ulong.MaxValue;
            Assert.IsTrue(int64.Low == uint.MaxValue && int64.High == uint.MaxValue);

            // Set all 63 least significant bits to 1 and MSB to 0.
            int64 = long.MaxValue;
            Assert.IsTrue(int64.Bit63 == 0);

            // Check that high has all bits besides MSB set to 1 and that low has all bits set to 1.
            Assert.IsTrue(int64.High == int.MaxValue && int64.Low == uint.MaxValue);

            // Set the MSB back to 1 and check that all bits are set to 1.
            int64.Bit63 = 1;
            Assert.IsTrue(int64 == ulong.MaxValue);
            Assert.IsTrue(int64.High == uint.MaxValue && int64.Low == uint.MaxValue);
        }
    }
}