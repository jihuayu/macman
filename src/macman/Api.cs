namespace macman
{
    public class Api
    {
        public static void GetMod(string Name,string InstallPath,bool Force)
        {
            var mod = Name.Split('@');
            var name = mod[0];
            var version = mod.Length > 1 ? mod[1] : "1.12.2";
            if (int.TryParse(name,out _))
            {
                Tasks.DownloadModAsync(name, version, InstallPath,Force).Wait();
            }
            else
            {
                var s = Tasks.FindAsync(name, version).Result;
                foreach (var file in s) Tasks.DownloadModAsync(file, version, InstallPath,Force).Wait();
            }
        }
    }
}