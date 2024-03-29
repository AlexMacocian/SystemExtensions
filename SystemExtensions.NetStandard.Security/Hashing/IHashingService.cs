﻿using System.IO;
using System.Threading.Tasks;

namespace System.Security.Hashing;

public interface IHashingService
{
    Task<string> Hash(string raw);
    Task<byte[]> Hash(byte[] raw);
    Task<Stream> Hash(Stream raw);
}
