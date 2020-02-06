using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace macman
{
    public class Api
    {
        public static void GetMod(string Name, string InstallPath, bool Force)
        {
            var mod = Name.Split('@');
            var name = mod[0];
            var version = mod.Length > 1 ? mod[1] : "1.12.2";
            if (int.TryParse(name, out _))
            {
                Tasks.DownloadModAsync(name, version, InstallPath, Force).Wait();
            }
            else
            {
                var s = Tasks.FindAsync(name, version).Result;
                foreach (var file in s) Tasks.DownloadModAsync(file, version, InstallPath, Force).Wait();
            }
        }

        public static async Task InstallModpack(string NowPath, string Name, bool force)
        {
            var path = Path.Combine(NowPath, "manifest.json");
            var game = Path.Combine(NowPath, Name);
            var mods = Path.Combine(game, "mods");
            if (!File.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("没有在当前目录找到manifest.json");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Util.CreatDirectory(mods);
            var str = File.ReadAllText(path);
            var json = (JObject) JsonConvert.DeserializeObject(str);
            var files = json["files"].Value<JArray>();
            foreach (var i in files)
                await Tasks.DownloadFileAsync(i["projectID"].Value<string>(), i["fileID"].Value<string>(), mods, force);
        }
    }
}