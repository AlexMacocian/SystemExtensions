﻿using System.IO;

namespace System.Extensions
{
    public static class StreamExtensions
    {
        public static void DoWhileReading(this Stream stream, Action<int, byte[]> action, int bufferLength = 256)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            if (action is null) throw new ArgumentNullException(nameof(action));

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
            if (stream is null) throw new ArgumentNullException(nameof(stream));

            var buffer = new byte[count];
            stream.Read(buffer, 0, count);
            return buffer;
        }

        public static Stream Rewind(this Stream stream)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));

            if (!stream.CanSeek) throw new InvalidOperationException("Stream doesn't support rewinding");

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
