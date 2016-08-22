using System.IO;

namespace NGulp.V8
{
    public static class CoffeeScript
    {
        static CoffeeScript()
        {
            JavaScript.Inject(@"http://coffeescript.org/extras/coffee-script.js");
        }

        public static Stream Compile(Stream stream)
        {
            var coffeescript = GulpConvert.ToString(stream);
            var javascript = JavaScript.Engine.Script.CoffeeScript.compile(coffeescript);
            return GulpConvert.ToStream(javascript);
        }
    }
}