using System.IO;

namespace System.Extensions;

public static class BytesExtensions
{
    public static byte[] ReadAllBytes(this Stream stream)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        var buffer = new byte[256];
        using var ms = new MemoryStream();
        int read;
        while ((read = stream.Read(buffer, 0, 256)) > 0)
        {
            ms.Write(buffer, 0, read);
        }

        return ms.ToArray();
    }
}
