using System;
using System.IO;
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

        [Parameter(
            Position = 2,
            ValueFromPipelineByPropertyName = true)]
        [Alias("f")]
        public bool Force { get; set; } = false;
        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        protected override void ProcessRecord()
        {
            try
            {
                var ss = new SessionState();
                var path = ss.Path.CurrentFileSystemLocation.Path;
                InstallPath = Path.Combine(path, InstallPath);
                Directory.CreateDirectory(InstallPath);
                Api.GetMod(Name,InstallPath,Force);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }
}