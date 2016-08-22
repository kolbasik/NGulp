using System;
using System.IO;
using System.Text;

namespace NGulp
{
    public static class GulpConvert
    {
        public static string ToString(Stream stream)
        {
            stream.Position = 0;
            return new StreamReader(stream).ReadToEnd();
        }

        public static Stream ToStream(object source)
        {
            return ToStream(Convert.ToString(source));
        }

        public static Stream ToStream(string text)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(text));
        }
    }
}