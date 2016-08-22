using System.Net;
using Microsoft.ClearScript.V8;

namespace NGulp.V8
{
    public static class JavaScript
    {
        public static readonly V8ScriptEngine Engine;
        public static readonly WebClient Web;

        static JavaScript()
        {
            Engine = new V8ScriptEngine();
            Web = new WebClient();
        }

        public static void Inject(string url)
        {
            Engine.Execute(Web.DownloadString(url));
        }
    }
}