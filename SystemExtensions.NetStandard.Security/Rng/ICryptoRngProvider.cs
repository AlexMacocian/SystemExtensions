namespace System.Rng
{
    public interface ICryptoRngProvider
    {
        public void GetBytes(byte[] data);
        public void GetNonZeroBytes(byte[] data);
    }
}
