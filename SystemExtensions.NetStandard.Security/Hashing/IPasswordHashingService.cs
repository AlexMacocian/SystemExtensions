using System.Threading.Tasks;

namespace System.Security.Hashing;

public interface IPasswordHashingService
{
    /// <summary>
    /// Hash provided raw password and return the hashed value.
    /// </summary>
    /// <param name="raw">Base64 encoded password.</param>
    /// <param name="salt">Base64 encoded salt to be used for hashing.</param>
    /// <param name="length">Length of the resulting hash.</param>
    /// <param name="iterations">Number of hashing iterations to be performed.</param>
    /// <remarks>
    /// The length of the hash will vary due to the base64 encoding of the resulting hash.
    /// </remarks>
    /// <returns>Base64 encoded hashed password.</returns>
    Task<string> Hash(string raw, string salt, int length, int iterations);
    /// <summary>
    /// Hash provided raw password and return the hashed value.
    /// </summary>
    /// <param name="raw">Password to be hashed.</param>
    /// <param name="salt">Salt to be used for hashing.</param>
    /// <param name="length">Length of the resulting hash.</param>
    /// <param name="iterations">Number of hashing iterations to be performed.</param>
    /// <returns>Hashed password.</returns>
    Task<byte[]> Hash(byte[] raw, byte[] salt, int length, int iterations);
    /// <summary>
    /// Verify provided password against a hashed password.
    /// </summary>
    /// <param name="hash">Base64 encoding of the previously hashed password.</param>
    /// <param name="password">Base64 encoding of the password to be verified.</param>
    /// <param name="salt">Salt used to hash the previous password.</param>
    /// <param name="iterations">Number of iterations used to hash the previous password.</param>
    /// <returns>Returns true if password matches the previously hashed password. Otherwise returns false.</returns>
    Task<bool> VerifyPassword(string hash, string password, string salt, int iterations);
    /// <summary>
    /// Verify provided password against a hashed password.
    /// </summary>
    /// <param name="hash">Previously hashed password.</param>
    /// <param name="password">Password to be verified.</param>
    /// <param name="salt">Salt used to hash the previous password.</param>
    /// <param name="iterations">Number of iterations used to hash the previous password.</param>
    /// <returns>Returns true if password matches the previously hashed password. Otherwise returns false.</returns>
    Task<bool> VerifyPassword(byte[] hash, byte[] password, byte[] salt, int iterations);
}
