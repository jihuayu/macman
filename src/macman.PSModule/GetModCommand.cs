﻿using System;
using System.IO;
using System.Management.Automation;

namespace macman
{
    [Cmdlet("Get", "Mod")]
    public class GetModCommand : PSCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        [Alias("n")]
        public string Name { get; set; }

        [Parameter(
            Position = 1,
            ValueFromPipelineByPropertyName = true)]
        [Alias("o")]
        public string InstallPath { get; set; } = "mods";

        [Parameter(
            Position = 2,
            ValueFromPipelineByPropertyName = true)]
        [Alias("f")]
        public bool Force { get; set; } = false;

        protected override void ProcessRecord()
        {
            try
            {
                var ss = new SessionState();
                var path = ss.Path.CurrentFileSystemLocation.Path;
                InstallPath = Path.Combine(path, InstallPath);
                Directory.CreateDirectory(InstallPath);
                Api.GetMod(Name, InstallPath, Force);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}