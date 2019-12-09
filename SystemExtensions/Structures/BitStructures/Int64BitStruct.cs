using System;

namespace SystemExtensions.Structures.BitStructures
{
    public struct Int64BitStruct : IEquatable<Int64BitStruct>
    {
        public Int64BitStruct(uint low, uint high)
        {
            this.Value = low + ((ulong)high << 32);
        }

        public Int64BitStruct(int low, int high)
        {
            unchecked
            {
                this.Value = (uint)low + ((ulong)high << 32);
            }
        }

        public Int64BitStruct(long value)
        {
            unchecked
            {
                this.Value = (ulong)value;
            }
        }

        public Int64BitStruct(ulong value)
        {
            this.Value = value;
        }

        public ulong Value { get; set; }
        public uint Low { get => (uint)(this.Value & 0xFFFFFFFF); set => this.Value = (this.Value & ~(0xFFFFFFFF)) | (0xFFFFFFFF & value); }
        public uint High { get => (uint)((this.Value >> 32) & 0xFFFFFFFF); set => this.Value = (this.Value & ~(0xFFFFFFFF00000000)) | ((ulong)(0xFFFFFFFF & value) << 32); }
        public ulong Bit0 { get => this.Value & 0x1; set => this.Value = (this.Value & ~(0x1UL) | (0x1 & value)); }
        public ulong Bit1 { get => this.Value >> 1 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 1)) | ((0x1 & value) << 1); }
        public ulong Bit2 { get => this.Value >> 2 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 2)) | ((0x1 & value) << 2); }
        public ulong Bit3 { get => this.Value >> 3 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 3)) | ((0x1 & value) << 3); }
        public ulong Bit4 { get => this.Value >> 4 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 4)) | ((0x1 & value) << 4); }
        public ulong Bit5 { get => this.Value >> 5 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 5)) | ((0x1 & value) << 5); }
        public ulong Bit6 { get => this.Value >> 6 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 6)) | ((0x1 & value) << 6); }
        public ulong Bit7 { get => this.Value >> 7 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 7)) | ((0x1 & value) << 7); }
        public ulong Bit8 { get => this.Value >> 8 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 8)) | ((0x1 & value) << 8); }
        public ulong Bit9 { get => this.Value >> 9 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 9)) | ((0x1 & value) << 9); }
        public ulong Bit10 { get => this.Value >> 10 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 10)) | ((0x1 & value) << 10); }
        public ulong Bit11 { get => this.Value >> 11 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 11)) | ((0x1 & value) << 11); }
        public ulong Bit12 { get => this.Value >> 12 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 12)) | ((0x1 & value) << 12); }
        public ulong Bit13 { get => this.Value >> 13 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 13)) | ((0x1 & value) << 13); }
        public ulong Bit14 { get => this.Value >> 14 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 14)) | ((0x1 & value) << 14); }
        public ulong Bit15 { get => this.Value >> 15 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 15)) | ((0x1 & value) << 15); }
        public ulong Bit16 { get => this.Value >> 16 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 16)) | ((0x1 & value) << 16); }
        public ulong Bit17 { get => this.Value >> 17 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 17)) | ((0x1 & value) << 17); }
        public ulong Bit18 { get => this.Value >> 18 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 18)) | ((0x1 & value) << 18); }
        public ulong Bit19 { get => this.Value >> 19 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 19)) | ((0x1 & value) << 19); }
        public ulong Bit20 { get => this.Value >> 20 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 20)) | ((0x1 & value) << 20); }
        public ulong Bit21 { get => this.Value >> 21 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 21)) | ((0x1 & value) << 21); }
        public ulong Bit22 { get => this.Value >> 22 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 22)) | ((0x1 & value) << 22); }
        public ulong Bit23 { get => this.Value >> 23 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 23)) | ((0x1 & value) << 23); }
        public ulong Bit24 { get => this.Value >> 24 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 24)) | ((0x1 & value) << 24); }
        public ulong Bit25 { get => this.Value >> 25 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 25)) | ((0x1 & value) << 25); }
        public ulong Bit26 { get => this.Value >> 26 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 26)) | ((0x1 & value) << 26); }
        public ulong Bit27 { get => this.Value >> 27 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 27)) | ((0x1 & value) << 27); }
        public ulong Bit28 { get => this.Value >> 28 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 28)) | ((0x1 & value) << 28); }
        public ulong Bit29 { get => this.Value >> 29 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 29)) | ((0x1 & value) << 29); }
        public ulong Bit30 { get => this.Value >> 30 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 30)) | ((0x1 & value) << 30); }
        public ulong Bit31 { get => this.Value >> 31 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 31)) | ((0x1 & value) << 31); }
        public ulong Bit32 { get => this.Value >> 32 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 32)) | ((0x1 & value) << 32); }
        public ulong Bit33 { get => this.Value >> 33 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 33)) | ((0x1 & value) << 33); }
        public ulong Bit34 { get => this.Value >> 34 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 34)) | ((0x1 & value) << 34); }
        public ulong Bit35 { get => this.Value >> 35 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 35)) | ((0x1 & value) << 35); }
        public ulong Bit36 { get => this.Value >> 36 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 36)) | ((0x1 & value) << 36); }
        public ulong Bit37 { get => this.Value >> 37 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 37)) | ((0x1 & value) << 37); }
        public ulong Bit38 { get => this.Value >> 38 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 38)) | ((0x1 & value) << 38); }
        public ulong Bit39 { get => this.Value >> 39 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 39)) | ((0x1 & value) << 39); }
        public ulong Bit40 { get => this.Value >> 40 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 40)) | ((0x1 & value) << 40); }
        public ulong Bit41 { get => this.Value >> 41 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 41)) | ((0x1 & value) << 41); }
        public ulong Bit42 { get => this.Value >> 42 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 42)) | ((0x1 & value) << 42); }
        public ulong Bit43 { get => this.Value >> 43 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 43)) | ((0x1 & value) << 43); }
        public ulong Bit44 { get => this.Value >> 44 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 44)) | ((0x1 & value) << 44); }
        public ulong Bit45 { get => this.Value >> 45 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 45)) | ((0x1 & value) << 45); }
        public ulong Bit46 { get => this.Value >> 46 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 46)) | ((0x1 & value) << 46); }
        public ulong Bit47 { get => this.Value >> 47 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 47)) | ((0x1 & value) << 47); }
        public ulong Bit48 { get => this.Value >> 48 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 48)) | ((0x1 & value) << 48); }
        public ulong Bit49 { get => this.Value >> 49 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 49)) | ((0x1 & value) << 49); }
        public ulong Bit50 { get => this.Value >> 50 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 50)) | ((0x1 & value) << 50); }
        public ulong Bit51 { get => this.Value >> 51 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 51)) | ((0x1 & value) << 51); }
        public ulong Bit52 { get => this.Value >> 52 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 52)) | ((0x1 & value) << 52); }
        public ulong Bit53 { get => this.Value >> 53 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 53)) | ((0x1 & value) << 53); }
        public ulong Bit54 { get => this.Value >> 54 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 54)) | ((0x1 & value) << 54); }
        public ulong Bit55 { get => this.Value >> 55 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 55)) | ((0x1 & value) << 55); }
        public ulong Bit56 { get => this.Value >> 56 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 56)) | ((0x1 & value) << 56); }
        public ulong Bit57 { get => this.Value >> 57 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 57)) | ((0x1 & value) << 57); }
        public ulong Bit58 { get => this.Value >> 58 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 58)) | ((0x1 & value) << 58); }
        public ulong Bit59 { get => this.Value >> 59 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 59)) | ((0x1 & value) << 59); }
        public ulong Bit60 { get => this.Value >> 60 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 60)) | ((0x1 & value) << 60); }
        public ulong Bit61 { get => this.Value >> 61 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 61)) | ((0x1 & value) << 61); }
        public ulong Bit62 { get => this.Value >> 62 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 62)) | ((0x1 & value) << 62); }
        public ulong Bit63 { get => this.Value >> 63 & 0x1; set => this.Value = (this.Value & ~(0x1UL << 63)) | ((0x1 & value) << 63); }

        public static implicit operator Int64BitStruct(ulong value) => new Int64BitStruct(value);
        public static implicit operator Int64BitStruct(long value) => new Int64BitStruct(value);
        public static implicit operator long(Int64BitStruct value) => (long)value.Value;
        public static implicit operator ulong(Int64BitStruct value) => value.Value;
        public static bool operator ==(Int64BitStruct first, Int64BitStruct second)
        {
            return first.Value == second.Value;
        }

        public static bool operator !=(Int64BitStruct first, Int64BitStruct second)
        {
            return first.Value != second.Value;
        }

        public static bool operator ==(Int64BitStruct first, ulong second)
        {
            return first.Value == second;
        }

        public static bool operator !=(Int64BitStruct first, ulong second)
        {
            return first.Value != second;
        }

        public static bool operator ==(Int64BitStruct first, long second)
        {
            unchecked
            {
                return first.Value == (ulong)second;
            }
        }

        public static bool operator !=(Int64BitStruct first, long second)
        {
            unchecked
            {
                return first.Value != (ulong)second;
            }
        }

        public static bool operator >=(Int64BitStruct first, Int64BitStruct second)
        {
            return first.Value >= second.Value;
        }

        public static bool operator <=(Int64BitStruct first, Int64BitStruct second)
        {
            return first.Value <= second.Value;
        }

        public static bool operator >=(Int64BitStruct first, long second)
        {
            unchecked
            {
                return first.Value >= (ulong)second;
            }
        }

        public static bool operator <=(Int64BitStruct first, long second)
        {
            unchecked
            {
                return first.Value <= (ulong)second;
            }
        }

        public static bool operator >=(Int64BitStruct first, ulong second)
        {
            unchecked
            {
                return first.Value >= second;
            }
        }

        public static bool operator <=(Int64BitStruct first, ulong second)
        {
            unchecked
            {
                return first.Value <= second;
            }
        }

        public bool Equals(Int64BitStruct other)
        {
            return this.Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Int64BitStruct)
            {
                return this.Equals((Int64BitStruct)obj);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}