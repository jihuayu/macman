﻿using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using macman;

namespace macman
{
    [Cmdlet("Get", "Mod")]
    public class GetModCommand : PSCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [Alias("n")]
        public string Name { get; set; } = "mods";

        [Parameter(
            Position = 1,
            ValueFromPipelineByPropertyName = true)]
        [Alias("o")]
        public string InstallPath { get; set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        protected override void ProcessRecord()
        {
            var mod = Name.Split('@');
            var name = mod[0];
            var ss = new SessionState();
            var path = ss.Path.CurrentFileSystemLocation.Path;

            InstallPath = Path.Combine(path, InstallPath);
            Directory.CreateDirectory(InstallPath);
            var version = mod.Length > 1 ? mod[1] : "1.12.2";
            if (int.TryParse(name,out _))
            {
                Tasks.DownloadModAsync(name, version, InstallPath).Wait();
            }
            else
            {
                var s = Tasks.FindAsync(name, version).Result;
                WriteObject(s);
                foreach (var file in s) Tasks.DownloadModAsync(file, version, InstallPath).Wait();
            }
        }

        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }
}