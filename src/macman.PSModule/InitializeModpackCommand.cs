using System;
using System.Management.Automation;
using Newtonsoft.Json;

namespace macman
{
    [Cmdlet("Initialize", "Modpack")]
    public class InitializeModpackCommand : PSCmdlet
    {
        [Parameter(
            Position = 0,
            ValueFromPipelineByPropertyName = true)]
        [Alias("y")]
        public bool Yes { get; set; } = false;

        protected override void ProcessRecord()
        {
            try
            {
                var ss = new SessionState();
                var path = ss.Path.CurrentFileSystemLocation.Path;
                Api.InitModpack(path, Yes);
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