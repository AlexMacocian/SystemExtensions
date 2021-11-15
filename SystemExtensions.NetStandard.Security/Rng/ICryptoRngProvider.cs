namespace System.Security.Rng
{
    public interface ICryptoRngProvider
    {
        /// <summary>
        /// Populate provided array of bytes with cryptographically secure random bytes.
        /// </summary>
        /// <param name="data">Array of bytes to be populated.</param>
        public void GetBytes(byte[] data);
        /// <summary>
        /// Return an array of cryptographically secure random bytes.
        /// </summary>
        /// <param name="byteCount">Length of the returned array.</param>
        /// <returns>Array containing cryptographically secure random values.</returns>
        public byte[] GetBytes(int byteCount);
        /// <summary>
        /// Populate provided array of bytes with cryptographically secure random non-zero bytes.
        /// </summary>
        /// <param name="data">Array of bytes to be populated.</param>
        public void GetNonZeroBytes(byte[] data);
        /// <summary>
        /// Return an array of cryptographically secure random non-zero bytes.
        /// </summary>
        /// <param name="byteCount">Length of the returned array.</param>
        /// <returns>Array containing cryptographically secure random non-zero values.</returns>
        public byte[] GetNonZeroBytes(int byteCount);
    }
}
