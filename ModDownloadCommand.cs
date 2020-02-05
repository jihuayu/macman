using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace fmcl
{
    [Cmdlet("Add", "mcmod")]
    public class ModDownloadCommand : PSCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string ModName { get; set; } = "";

        [Parameter(
            Position = 1,
            ValueFromPipelineByPropertyName = true)]
        public string Path { get; set; } = "\\";

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            var mod = ModName.Split('@');
            var name = mod[0];
            SessionState ss = new SessionState();
            string p = ss.Path.CurrentFileSystemLocation.Path;

            Path = p + @"\" + Path;
            Path.Replace('/', '\\');
            if (!Path.EndsWith("\\"))
            {
                Path += "\\";
            }

            Util.createdir(Path);
            var version = mod.Length > 1 ? mod[1] : "1.12.2";
            if (Util.IfNum(name))
            {
                Tasks.DownloadMcmod(name, version, Path);
            }
            else
            {
                var s = Tasks.FindAndDl(name, version, "mcmod").Result;
                WriteObject(s);
                Task.Run(async () =>
                {
                    foreach (var file in s)
                    {
                        Tasks.DownloadMcmod(file, version, Path);
                    }
                });
            }
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }
}