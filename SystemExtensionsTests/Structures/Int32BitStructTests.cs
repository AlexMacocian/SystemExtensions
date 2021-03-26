﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Structures.BitStructures;

namespace SystemExtensionsTests.Structures
{
    [TestClass]
    public class Int32BitStructTests
    {
        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(int.MaxValue)]
        public void TestSetValueInt(int value)
        {
            Int32BitStruct int32 = value;
            Assert.IsTrue(int32 == value);
        }

        [DataTestMethod]
        [DataRow(1U)]
        [DataRow(uint.MaxValue)]
        public void TestSetValueUint(uint value)
        {
            Int32BitStruct int32 = value;
            Assert.IsTrue(int32 == value);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void TestGetBit(int value)
        {
            Int32BitStruct int32 = value;
            Assert.IsTrue(value >= 0 ? int32.Bit31 == 0 : int32.Bit31 == 1);
        }

        [DataTestMethod]
        [DataRow(0U)]
        [DataRow(1U)]
        public void TestSetBit(uint value)
        {
            Int32BitStruct int32 = 0;
            int32.Bit0 = value;
            Assert.IsTrue(int32 == value);
        }

        [TestMethod]
        public void TestUseCaseExample()
        {
            // Set value = -1, represented as MSB being 1 and the rest 0.
            Int32BitStruct int32 = int.MinValue;

            // Test that MSB is 1.
            Assert.IsTrue(int32.Bit31 == 1);

            // Set MSB to 0 and test that everything else is 0.
            int32.Bit31 = 0;
            Assert.IsTrue(int32 == 0);

            // Set the least 31 significant bits (all besides MSB) to 1.
            int32 = int.MaxValue;

            // Set the MSB to 1 and test that everything is 1.
            int32.Bit31 = 1;
            Assert.IsTrue(int32 == uint.MaxValue);
        }
    }
}