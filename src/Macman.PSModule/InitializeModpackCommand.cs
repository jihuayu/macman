using System;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using Macman.Extensions;
using Macman.Io;
using Macman.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Macman.PSModule
{
    [Cmdlet("Initialize", "Modpack")]
    public class InitializeModpackCommand : PSCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipelineByPropertyName = true)]
        [Alias("y")]
        public bool Yes { get; set; } = false;

        [Parameter(
            ValueFromPipelineByPropertyName = true)]
        [Alias("f")]
        public SwitchParameter Force { get; set; } = false;

        protected override void ProcessRecord()
        {
            try
            {
                var ss = new SessionState();
                var path = ss.Path.CurrentFileSystemLocation.Path;
                InitModPack(path, Yes).Wait();
            }
            catch (JsonException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("json解析错误");
                Console.ForegroundColor = ConsoleColor.White;
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task InitModPack(string path, bool yes)
        {
            var main = Path.Combine(path, "manifest.json");
            if (File.Exists(main))
            {
                ConsoleUtils.Error("本文件夹已经存在了manifest.json");
                return;
            }

            var name = String.Empty;
            var author = String.Empty;
            var version = String.Empty;
            if (!yes)
            {
                ConsoleUtils.Green("请输入整合包名字:");
                name = Console.ReadLine();
                ConsoleUtils.Green("请输入作者名字:");
                author = Console.ReadLine();
                ConsoleUtils.Green("请输入mc游戏版本(1.12.2):");
                version = Console.ReadLine();
            }

            if (name.IsNullOrWhiteSpace()) name = String.Empty;

            if (author.IsNullOrWhiteSpace()) author = String.Empty;

            if (version.IsNullOrWhiteSpace()) version = "1.12.2";
            var forge = await ApiManager.GetLastForge(version, true);
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