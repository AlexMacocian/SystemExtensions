using System.Security.Cryptography;

namespace System.Security.Hashing;
public sealed class HMACMD5Service : BaseHMACService<HMACMD5>, IHMACMD5Service
{
    protected override HMACMD5 GetHashAlgorithm(byte[] key)
    {
        return new HMACMD5(key);
    }
}
