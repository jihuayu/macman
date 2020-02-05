using System;
using System.Management.Automation;
using Newtonsoft.Json;

namespace macman
{
    [Cmdlet("Install", "Modpack")]
    public class InstallModpackCommand : PSCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipelineByPropertyName = true)]
        [Alias("n")]
        public string Name { get; set; } = "minecraft";

        [Parameter(
            Position = 1,
            ValueFromPipelineByPropertyName = true)]
        [Alias("f")]
        public bool Force { get; set; } = false;

        protected override void ProcessRecord()
        {
            try
            {
                var ss = new SessionState();
                var path = ss.Path.CurrentFileSystemLocation.Path;
                Api.InstallModpack(path, Name, Force).Wait();
            }
            catch (JsonException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("json解析错误");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}