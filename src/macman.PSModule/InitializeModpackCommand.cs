using System;
using System.Management.Automation;
using Newtonsoft.Json;

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
                Api.InitModPack(path, Yes).Wait();
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
    }
}