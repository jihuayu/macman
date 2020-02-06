using System;
using System.IO;
using System.Threading.Tasks;
using Macman.Extensions;
using Macman.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Macman
{
    public static class Api
    {
        public static void GetMod(string Name, string InstallPath, bool Force)
        {
            var mod = Name.Split('@');
            var name = mod[0];
            var p = Util.FindFile(InstallPath, "manifest.json");

            var version = mod.Length > 1 ? mod[1] : "";
            if (p != null)
            {
                var str = File.ReadAllText(p);
                version = ((JObject) JsonConvert.DeserializeObject(str))["minecraft"]["version"].Value<string>();
            }

            if (version.IsNullOrEmpty()) version = "1.12.2";
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

            Util.CreateDirectory(mods);
            var str = File.ReadAllText(path);
            var json = (JObject) JsonConvert.DeserializeObject(str);
            var files = json["files"].Value<JArray>();
            foreach (var i in files)
                await Tasks.DownloadFileAsync(i["projectID"].Value<string>(), i["fileID"].Value<string>(), mods, force);
        }

        public static async Task InitModPack(string path, bool yes)
        {
            var main = Path.Combine(path, "manifest.json");
            if (File.Exists(main))
            {
                ConsoleUtil.Error("本文件夹已经存在了manifest.json");
                return;
            }

            var name = string.Empty;
            var author = string.Empty;
            var version = string.Empty;
            if (!yes)
            {
                ConsoleUtil.Green("请输入整合包名字:");
                name = Console.ReadLine();
                ConsoleUtil.Green("请输入作者名字:");
                author = Console.ReadLine();
                ConsoleUtil.Green("请输入mc游戏版本(1.12.2):");
                version = Console.ReadLine();
            }

            if (name.IsNullOrWhiteSpace()) name = string.Empty;

            if (author.IsNullOrWhiteSpace()) author = string.Empty;

            if (version.IsNullOrWhiteSpace()) version = "1.12.2";
            var forge = await TwitchApi.GetLastForge(version, true);
            var obj = new JObject();
            var minecraft = new JObject {["version"] = version};
            var modLoaders = new JObject {["id"] = forge, ["primary"] = true};
            minecraft["modLoaders"] = modLoaders;
            obj["minecraft"] = minecraft;
            obj["manifestType"] = "minecraftModpack";
            obj["manifestVersion"] = 1;
            obj["name"] = name;
            obj["version"] = "1.0.0";
            obj["author"] = author;
            obj["files"] = new JArray();
            File.WriteAllText(main, obj.ToString());
        }
    }
}