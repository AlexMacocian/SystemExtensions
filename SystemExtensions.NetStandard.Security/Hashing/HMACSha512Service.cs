using System.Security.Cryptography;

namespace System.Security.Hashing;
public sealed class HMACSha512Service : BaseHMACService<HMACSHA512>, IHMACSha512Service
{
    protected override HMACSHA512 GetHashAlgorithm(byte[] key)
    {
        return new HMACSHA512(key);
    }
}
