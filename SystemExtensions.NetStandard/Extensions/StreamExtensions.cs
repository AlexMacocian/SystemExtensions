using System.IO;
using System.Text;

namespace System.Extensions;

public static class StreamExtensions
{
    public static void DoWhileReading(this Stream stream, Action<int, byte[]> action, int bufferLength = 256)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var buffer = new byte[bufferLength];
        var read = stream.Read(buffer, 0, bufferLength);
        while(read > 0)
        {
            action(bufferLength, buffer);
            read = stream.Read(buffer, 0, bufferLength);
        }
    }

    public static byte[] ReadBytes(this Stream stream, int count)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        var buffer = new byte[count];
        stream.Read(buffer, 0, count);
        return buffer;
    }

    public static Stream Rewind(this Stream stream)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (!stream.CanSeek)
        {
            throw new InvalidOperationException("Stream doesn't support rewinding");
        }

        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    public static string ReadUntil(this StreamReader sr, string delim)
    {
        var sb = new StringBuilder();
        var found = false;

        while (!found && !sr.EndOfStream)
        {
            for (var i = 0; i < delim.Length; i++)
            {
                var c = (char)sr.Read();
                sb.Append(c);

                if (c != delim[i])
                {
                    break;
                }

                if (i == delim.Length - 1)
                {
                    sb.Remove(sb.Length - delim.Length, delim.Length);
                    found = true;
                }
            }
        }

        return sb.ToString();
    }
}
