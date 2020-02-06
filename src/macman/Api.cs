using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace macman
{
    public static class Api
    {
        public static void GetMod(string name, string installPath, bool force)
        {
            var mod = name.Split('@');
            if (mod.Length>1)
            {
                name = mod[0];
            }
            var version = mod.Length > 1 ? mod[1] : "1.12.2";
            if (int.TryParse(name, out _))
            {
                Tasks.DownloadModAsync(name, version, installPath, force).Wait();
            }
            else
            {
                var s = Tasks.FindAsync(name, version).Result;
                foreach (var file in s) Tasks.DownloadModAsync(file, version, installPath, force).Wait();
            }
        }

        public static async Task InstallModpack(string nowPath, string name, bool force)
        {
            var path = Path.Combine(nowPath, "manifest.json");
            var game = Path.Combine(nowPath, name);
            var mods = Path.Combine(game, "mods");
            if (!File.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("没有在当前目录找到manifest.json");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Util.CreateDirectory(mods);
            var str = File.ReadAllText(path);
            var json = (JObject) JsonConvert.DeserializeObject(str);
            var files = json["files"].Value<JArray>();
            foreach (var i in files)
                await Tasks.DownloadFileAsync(i["projectID"].Value<string>(), i["fileID"].Value<string>(), mods, force);
        }
    }
}