using System;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using Macman.Io;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Macman.PSModule
{
    [Cmdlet("Install", "ModPack")]
    public class InstallModPackCommand : PSCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipelineByPropertyName = true)]
        [Alias("n")]
        public string Name { get; set; } = "minecraft";

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
                InstallModpack(path, Name, Force).Wait();
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

        public static async Task InstallModpack(string path, string Name, bool force)
        {
            var fullPath = Path.Combine(path, "manifest.json");
            var mods = Path.Combine(path, Name, "mods");
            if (!File.Exists(fullPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("没有在当前目录找到manifest.json");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Directory.CreateDirectory(mods);
            var str = File.ReadAllText(fullPath);
            var json = JObject.Parse(str);
            var files = json["files"].Value<JArray>();
            foreach (var i in files)
                await DownloadManager.DownloadFileAsync(i["projectID"].Value<string>(), i["fileID"].Value<string>(),
                    mods, force);
        }
    }
}