using System;
using Newtonsoft.Json.Linq;

namespace fmcl
{
    public class Tasks
    {
        public static void DownloadMcmod(string id,string version,string path)
        {
            JObject o = TwitchAPI.GetVersionFile(id, version);
            DownloadMcmod1(o, version, path);
        }

        private static void DownloadMcmod1(JObject o, string version, string path)
        {
            Util.Download(o["downloadUrl"].Value<string>(), path + o["fileName"].Value<string>());
            foreach (var de in o["dependencies"])
            {
                if (de["type"].Value<int>()==3)
                {
                    
                    DownloadMcmod1(TwitchAPI.GetVersionFile(de["addonId"].Value<string>(), version),version,path);
                }
            }
        }
    }
}