using System;

namespace SystemExtensions.Structures.BitStructures
{
    public struct Int32BitStruct : IEquatable<Int32BitStruct>
    {
        public Int32BitStruct(uint value) => this.Value = value;
        public Int32BitStruct(int value) { unchecked { this.Value = (uint)value; } }
        public uint Value { get; set; }
        public uint Bit0 { get => this.Value & 0x1; set => this.Value = (this.Value & ~(0x1U) | (0x1 & value)); }
        public uint Bit1 { get => this.Value >> 1 & 0x1; set => this.Value = (this.Value & ~(0x1U << 1)) | ((0x1 & value) << 1); }
        public uint Bit2 { get => this.Value >> 2 & 0x1; set => this.Value = (this.Value & ~(0x1U << 2)) | ((0x1 & value) << 2); }
        public uint Bit3 { get => this.Value >> 3 & 0x1; set => this.Value = (this.Value & ~(0x1U << 3)) | ((0x1 & value) << 3); }
        public uint Bit4 { get => this.Value >> 4 & 0x1; set => this.Value = (this.Value & ~(0x1U << 4)) | ((0x1 & value) << 4); }
        public uint Bit5 { get => this.Value >> 5 & 0x1; set => this.Value = (this.Value & ~(0x1U << 5)) | ((0x1 & value) << 5); }
        public uint Bit6 { get => this.Value >> 6 & 0x1; set => this.Value = (this.Value & ~(0x1U << 6)) | ((0x1 & value) << 6); }
        public uint Bit7 { get => this.Value >> 7 & 0x1; set => this.Value = (this.Value & ~(0x1U << 7)) | ((0x1 & value) << 7); }
        public uint Bit8 { get => this.Value >> 8 & 0x1; set => this.Value = (this.Value & ~(0x1U << 8)) | ((0x1 & value) << 8); }
        public uint Bit9 { get => this.Value >> 9 & 0x1; set => this.Value = (this.Value & ~(0x1U << 9)) | ((0x1 & value) << 9); }
        public uint Bit10 { get => this.Value >> 10 & 0x1; set => this.Value = (this.Value & ~(0x1U << 10)) | ((0x1 & value) << 10); }
        public uint Bit11 { get => this.Value >> 11 & 0x1; set => this.Value = (this.Value & ~(0x1U << 11)) | ((0x1 & value) << 11); }
        public uint Bit12 { get => this.Value >> 12 & 0x1; set => this.Value = (this.Value & ~(0x1U << 12)) | ((0x1 & value) << 12); }
        public uint Bit13 { get => this.Value >> 13 & 0x1; set => this.Value = (this.Value & ~(0x1U << 13)) | ((0x1 & value) << 13); }
        public uint Bit14 { get => this.Value >> 14 & 0x1; set => this.Value = (this.Value & ~(0x1U << 14)) | ((0x1 & value) << 14); }
        public uint Bit15 { get => this.Value >> 15 & 0x1; set => this.Value = (this.Value & ~(0x1U << 15)) | ((0x1 & value) << 15); }
        public uint Bit16 { get => this.Value >> 16 & 0x1; set => this.Value = (this.Value & ~(0x1U << 16)) | ((0x1 & value) << 16); }
        public uint Bit17 { get => this.Value >> 17 & 0x1; set => this.Value = (this.Value & ~(0x1U << 17)) | ((0x1 & value) << 17); }
        public uint Bit18 { get => this.Value >> 18 & 0x1; set => this.Value = (this.Value & ~(0x1U << 18)) | ((0x1 & value) << 18); }
        public uint Bit19 { get => this.Value >> 19 & 0x1; set => this.Value = (this.Value & ~(0x1U << 19)) | ((0x1 & value) << 19); }
        public uint Bit20 { get => this.Value >> 20 & 0x1; set => this.Value = (this.Value & ~(0x1U << 20)) | ((0x1 & value) << 20); }
        public uint Bit21 { get => this.Value >> 21 & 0x1; set => this.Value = (this.Value & ~(0x1U << 21)) | ((0x1 & value) << 21); }
        public uint Bit22 { get => this.Value >> 22 & 0x1; set => this.Value = (this.Value & ~(0x1U << 22)) | ((0x1 & value) << 22); }
        public uint Bit23 { get => this.Value >> 23 & 0x1; set => this.Value = (this.Value & ~(0x1U << 23)) | ((0x1 & value) << 23); }
        public uint Bit24 { get => this.Value >> 24 & 0x1; set => this.Value = (this.Value & ~(0x1U << 24)) | ((0x1 & value) << 24); }
        public uint Bit25 { get => this.Value >> 25 & 0x1; set => this.Value = (this.Value & ~(0x1U << 25)) | ((0x1 & value) << 25); }
        public uint Bit26 { get => this.Value >> 26 & 0x1; set => this.Value = (this.Value & ~(0x1U << 26)) | ((0x1 & value) << 26); }
        public uint Bit27 { get => this.Value >> 27 & 0x1; set => this.Value = (this.Value & ~(0x1U << 27)) | ((0x1 & value) << 27); }
        public uint Bit28 { get => this.Value >> 28 & 0x1; set => this.Value = (this.Value & ~(0x1U << 28)) | ((0x1 & value) << 28); }
        public uint Bit29 { get => this.Value >> 29 & 0x1; set => this.Value = (this.Value & ~(0x1U << 29)) | ((0x1 & value) << 29); }
        public uint Bit30 { get => this.Value >> 30 & 0x1; set => this.Value = (this.Value & ~(0x1U << 30)) | ((0x1 & value) << 30); }
        public uint Bit31 { get => this.Value >> 31 & 0x1; set => this.Value = (this.Value & ~(0x1U << 31)) | ((0x1 & value) << 31); }

        public static implicit operator Int32BitStruct(uint value) => new Int32BitStruct(value);
        public static implicit operator Int32BitStruct(int value) => new Int32BitStruct(value);
        public static implicit operator int(Int32BitStruct value) => (int)value.Value;
        public static implicit operator uint(Int32BitStruct value) => value.Value;
        public static bool operator ==(Int32BitStruct first, Int32BitStruct second)
        {
            return first.Value == second.Value;
        }

        public static bool operator !=(Int32BitStruct first, Int32BitStruct second)
        {
            return first.Value != second.Value;
        }

        public static bool operator ==(Int32BitStruct first, uint second)
        {
            return first.Value == second;
        }

        public static bool operator !=(Int32BitStruct first, uint second)
        {
            return first.Value != second;
        }

        public static bool operator ==(Int32BitStruct first, int second)
        {
            unchecked
            {
                return first.Value == (uint)second;
            }
        }

        public static bool operator !=(Int32BitStruct first, int second)
        {
            unchecked
            {
                return first.Value != (uint)second;
            }
        }

        public static bool operator >=(Int32BitStruct first, Int32BitStruct second)
        {
            return first.Value >= second.Value;
        }

        public static bool operator <=(Int32BitStruct first, Int32BitStruct second)
        {
            return first.Value <= second.Value;
        }

        public static bool operator >=(Int32BitStruct first, int second)
        {
            unchecked
            {
                return first.Value >= (uint)second;
            }
        }

        public static bool operator <=(Int32BitStruct first, int second)
        {
            unchecked
            {
                return first.Value <= (uint)second;
            }
        }

        public static bool operator >=(Int32BitStruct first, uint second)
        {
            return first.Value >= second;
        }

        public static bool operator <=(Int32BitStruct first, uint second)
        {
            return first.Value <= second;
        }

        public static bool operator >(Int32BitStruct first, Int32BitStruct second)
        {
            return first.Value > second.Value;
        }

        public static bool operator <(Int32BitStruct first, Int32BitStruct second)
        {
            return first.Value < second.Value;
        }

        public static bool operator >(Int32BitStruct first, int second)
        {
            unchecked
            {
                return first.Value > (uint)second;
            }
        }

        public static bool operator <(Int32BitStruct first, int second)
        {
            unchecked
            {
                return first.Value < (uint)second;
            }
        }

        public static bool operator >(Int32BitStruct first, uint second)
        {
            return first.Value > second;
        }

        public static bool operator <(Int32BitStruct first, uint second)
        {
            return first.Value < second;
        }

        public bool Equals(Int32BitStruct other)
        {
            return this.Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Int32BitStruct)
            {
                return this.Equals((Int32BitStruct)obj);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return (this.Value).GetHashCode();
        }
    }
}