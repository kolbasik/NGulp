using System;
using System.IO;

namespace NGulp
{
    public sealed class GulpFile
    {
        public GulpFile(string path, string fullPath, Stream stream)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (fullPath == null) throw new ArgumentNullException(nameof(fullPath));
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            Path = path;
            FullPath = fullPath;
            Stream = stream;
        }

        public string Path { get; }
        public string FullPath { get; }
        public Stream Stream { get; }
    }
}