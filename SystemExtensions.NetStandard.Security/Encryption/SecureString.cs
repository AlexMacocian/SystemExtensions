using System.Security.Cryptography;
using System.Text;

namespace System.Encryption
{
    public sealed class SecureString
    {
        private static byte[] optionalEntropy;
        private byte[] encryptedValue;

        public string Value {
            get => this.encryptedValue is not null ? Encoding.UTF8.GetString(ProtectedData.Unprotect(this.encryptedValue, optionalEntropy, DataProtectionScope.CurrentUser)) : null;
            private set => this.encryptedValue = ProtectedData.Protect(Encoding.UTF8.GetBytes(value), optionalEntropy, DataProtectionScope.CurrentUser);
        }

        public SecureString(string value)
        {
            this.Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is string)
            {
                return this == (obj as string);
            }
            else if (obj is SecureString)
            {
                return this == (obj as SecureString);
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
        public override string ToString()
        {
            return this.Value;
        }

        public static readonly SecureString Empty = new(string.Empty);
        public static implicit operator string(SecureString ss) => ss is null ? string.Empty : ss.Value;
        public static implicit operator SecureString(string s) => new(s);
        public static SecureString operator +(SecureString ss1, SecureString ss2)
        {
            if (ss1 is null) throw new ArgumentNullException(nameof(ss1));
            if (ss2 is null) throw new ArgumentNullException(nameof(ss2));

            return new SecureString(ss1.Value + ss2.Value);
        }
        public static SecureString operator +(SecureString ss1, string s2)
        {
            if (ss1 is null) throw new ArgumentNullException(nameof(ss1));

            return new SecureString(ss1.Value + s2);
        }
        public static SecureString operator +(SecureString ss1, char c)
        {
            if (ss1 is null) throw new ArgumentNullException(nameof(ss1));

            return new SecureString(ss1.Value + c);
        }
        public static bool operator ==(SecureString ss1, SecureString ss2)
        {
            return ss1?.Value == ss2?.Value;
        }
        public static bool operator !=(SecureString ss1, SecureString ss2)
        {
            return !(ss1 == ss2);
        }
        public static bool operator ==(SecureString ss1, string s2)
        {
            return ss1?.Value == s2;
        }
        public static bool operator !=(SecureString ss1, string s2)
        {
            return !(ss1?.Value == s2);
        }

        public static void AddOptionalEntropy(byte[] entropy)
        {
            optionalEntropy = entropy;
        }
    }
}
