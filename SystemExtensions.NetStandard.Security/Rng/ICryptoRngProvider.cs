namespace System.Rng
{
    public interface ICryptoRngProvider
    {
        public void GetBytes(byte[] data);
        public byte[] GetBytes(int byteCount);
        public void GetNonZeroBytes(byte[] data);
        public byte[] GetNonZeroBytes(int byteCount);
    }
}
