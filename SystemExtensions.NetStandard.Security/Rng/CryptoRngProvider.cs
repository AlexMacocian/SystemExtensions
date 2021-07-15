using System.Security.Cryptography;

namespace System.Rng
{
    public sealed class CryptoRngProvider : ICryptoRngProvider, IDisposable
    {
        private readonly RNGCryptoServiceProvider rngProvider;
        private bool disposedValue;

        public CryptoRngProvider()
        {
            this.rngProvider = new RNGCryptoServiceProvider();
        }
        public CryptoRngProvider(CspParameters cspParams)
        {
            this.rngProvider = new RNGCryptoServiceProvider(cspParams);
        }


        public void GetBytes(byte[] data)
        {
            this.rngProvider.GetBytes(data);
        }
        public void GetNonZeroBytes(byte[] data)
        {
            this.rngProvider.GetNonZeroBytes(data);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.rngProvider.Dispose();
                }

                this.disposedValue = true;
            }
        }
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
