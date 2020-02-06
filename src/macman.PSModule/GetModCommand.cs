using System;
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
        public string Path { get; set; } = "mods";

        [Parameter(
            ValueFromPipelineByPropertyName = true)]
        [Alias("f")]
        public bool Force { get; set; } = false;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias("p")]
        public string Proxy { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                var ss = new SessionState();
                var path = ss.Path.CurrentFileSystemLocation.Path;
                Path = System.IO.Path.Combine(path, Path);
                Directory.CreateDirectory(Path);
                Api.GetMod(Name, Path, Force);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}