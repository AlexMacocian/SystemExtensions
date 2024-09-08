using System.Security.Cryptography;

namespace System.Security.Hashing;
public sealed class Sha384HashingService : BaseHashingService<SHA384>, ISha384HashingService
{
    protected override SHA384 GetHashAlgorithm()
    {
        return SHA384.Create();
    }
}
