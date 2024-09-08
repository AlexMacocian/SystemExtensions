using System.Security.Cryptography;

namespace System.Security.Hashing;
public sealed class Sha512HashingService : BaseHashingService<SHA512>, ISha512HashingService
{
    protected override SHA512 GetHashAlgorithm() => SHA512.Create();
}
