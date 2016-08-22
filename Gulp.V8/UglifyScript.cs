using System.IO;

namespace NGulp.V8
{
    public static class UglifyScript
    {
        static UglifyScript()
        {
            JavaScript.Inject(@"http://fmarcia.info/jsmin/jsmin.js");
        }

        public static Stream Compile(Stream stream)
        {
            var javascript = GulpConvert.ToString(stream);
            var minifiedjs = JavaScript.Engine.Script.jsmin(javascript);
            return GulpConvert.ToStream(minifiedjs);
        }
    }
}
