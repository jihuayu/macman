using System;
using System.IO;
using System.Management.Automation;
using Macman.Extensions;
using Macman.Io;
using Macman.Utils;
using Newtonsoft.Json.Linq;

namespace Macman.PSModule
{
    [Cmdlet("Get", "Mod")]
    public class GetModCommand : PSCmdlet
    {
        #region Parameters

        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        [Alias("n")]
        public string Name { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true)]
        [Alias("o")]
        public string Path { get; set; } = "mods";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string GameVersion { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string Version { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true)]
        [Alias("f")]
        public SwitchParameter Force { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias("p")]
        public string Proxy { get; set; }

        #endregion
        protected override void ProcessRecord()
        {
            var currentPath = SessionState.Path.CurrentLocation.Path;
            var targetPath = System.IO.Path.Combine(currentPath, Path);
            Directory.CreateDirectory(targetPath);
            //var p = FileUtils.FindFile(targetPath, "manifest.json");
            //if (p != null)
            //{
            //    var str = File.ReadAllText(p);
            //    version = JObject.Parse(str)["minecraft"]["version"].Value<string>();
            //}

            if (Version.IsNullOrEmpty()) Version = "1.12.2";
            if (int.TryParse(Name, out _))
            {
                DownloadManager.DownloadModAsync(Name, Version, targetPath, Force).Wait();
            }
            else
            {
                var urls = ApiManager.FindAsync(Name, Version).Result;
                foreach (var file in urls) DownloadManager.DownloadModAsync(file, Version, targetPath, Force).Wait();
            }
        }
    }
}