using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions
{
    public static class Comparators
    {
        #region Fields
        private static Comparison<int> intComparison = new Comparison<int>(IntegerComparison);
        private static Comparison<float> floatComparison = new Comparison<float>(FloatComparison);
        private static Comparison<double> doubleComparison = new Comparison<double>(DoubleComparison);
        private static Comparison<bool> boolComparison = new Comparison<bool>(BoolComparison);
        private static Comparison<decimal> decimalComparison = new Comparison<decimal>(DecimalComparison);
        private static Comparison<byte> byteComparison = new Comparison<byte>(ByteComparison);
        private static Comparison<long> longComparison = new Comparison<long>(LongComparison);
        private static Comparison<uint> uintComparison = new Comparison<uint>(UintComparison);
        private static Comparison<Int16> int16Comparison = new Comparison<Int16>(Int16Comparison);
        private static Comparison<Int32> int32Comparison = new Comparison<Int32>(Int32Comparison);
        private static Comparison<Int64> int64Comparison = new Comparison<Int64>(int64Comparison);
        private static Comparison<UInt16> uInt16Comparison = new Comparison<UInt16>(uInt16Comparison);
        private static Comparison<UInt32> uInt32Comparison = new Comparison<UInt32>(uInt32Comparison);
        private static Comparison<UInt64> uInt64Comparison = new Comparison<UInt64>(uInt64Comparison);
        #endregion
        #region Properties
        /// <summary>
        /// Comparison between two ints.
        /// </summary>
        public static Comparison<int> Int
        {
            get
            {
                return intComparison;
            }
        }
        /// <summary>
        /// Comparison between two floats.
        /// </summary>
        public static Comparison<float> Float
        {
            get
            {
                return floatComparison;
            }
        }
        /// <summary>
        /// Comparison between two doubles.
        /// </summary>
        public static Comparison<double> Double
        {
            get
            {
                return doubleComparison;
            }
        }
        /// <summary>
        /// Comparison between two decimals.
        /// </summary>
        public static Comparison<decimal> Decimal
        {
            get
            {
                return decimalComparison;
            }
        }
        /// <summary>
        /// Comparison between two bools.
        /// </summary>
        public static Comparison<bool> Bool
        {
            get
            {
                return boolComparison;
            }
        }
        /// <summary>
        /// Comparison between two bytes.
        /// </summary>
        public static Comparison<byte> Byte
        {
            get
            {
                return byteComparison;
            }
        }
        /// <summary>
        /// Comparison between two longs.
        /// </summary>
        public static Comparison<long> Long
        {
            get
            {
                return longComparison;
            }
        }
        /// <summary>
        /// Comparison between two uints.
        /// </summary>
        public static Comparison<uint> Uint
        {
            get
            {
                return uintComparison;
            }
        }
        /// <summary>
        /// Comparison between two int16.
        /// </summary>
        public static Comparison<Int16> Int16
        {
            get
            {
                return int16Comparison;
            }
        }
        /// <summary>
        /// Comparison between two int32.
        /// </summary>
        public static Comparison<Int32> Int32
        {
            get
            {
                return int32Comparison;
            }
        }
        /// <summary>
        /// Comparison between two int64.
        /// </summary>
        public static Comparison<Int64> Int64
        {
            get
            {
                return int64Comparison;
            }
        }
        /// <summary>
        /// Comparison between two uint16.
        /// </summary>
        public static Comparison<UInt16> UInt16
        {
            get
            {
                return uInt16Comparison;
            }
        }
        /// <summary>
        /// Comparison between two uint32.
        /// </summary>
        public static Comparison<UInt32> UInt32
        {
            get
            {
                return uInt32Comparison;
            }
        }
        /// <summary>
        /// Comparison between two uint64.
        /// </summary>
        public static Comparison<UInt64> UInt64
        {
            get
            {
                return uInt64Comparison;
            }
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Compares two UInt64.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int Uint64Comparison(UInt64 x, UInt64 y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two UInt32.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int Uint32Comparison(UInt32 x, UInt32 y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two UInt16.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int Uint16Comparison(UInt16 x, UInt16 y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two Int64.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int Int64Comparison(Int64 x, Int64 y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two UInt32.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int Int32Comparison(Int32 x, Int32 y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two UInt16.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int Int16Comparison(Int16 x, Int16 y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two uint.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int UintComparison(uint x, uint y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two Long.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int LongComparison(long x, long y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two Byte.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int ByteComparison(byte x, byte y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two Decimal.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int DecimalComparison(decimal x, decimal y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two Bool.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x is true and y is false. 0 if they are the same and -1 
        /// if y is true and x is false.</returns>
        private static int BoolComparison(bool x, bool y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x == true && y == false)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two Double.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int DoubleComparison(double x, double y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two Float.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int FloatComparison(float x, float y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Compares two Integer.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>Returns 1 if x > y. 0 if equal and -1 if y > x.</returns>
        private static int IntegerComparison(int x, int y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        #endregion
    }
}
