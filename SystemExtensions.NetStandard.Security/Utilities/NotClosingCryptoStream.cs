using System.IO;
using System.Security.Cryptography;

namespace System.Security.Utilities
{
    internal sealed class NotClosingCryptoStream : CryptoStream
    {
        public NotClosingCryptoStream(Stream stream, ICryptoTransform transform, CryptoStreamMode mode)
            : base(stream, transform, mode)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.HasFlushedFinalBlock)
            {
                this.FlushFinalBlock();
            }

            base.Dispose(false);
        }
    }
}
