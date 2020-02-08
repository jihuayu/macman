using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Macman.Utils
{
    public static class FileUtils
    {
        public static void SaveModFile(string path, string projectId, string fileId)
        {
            var main = FindFile(path, "manifest.json");
            if (main == null) return;
            var str = File.ReadAllText(main);
            var obj = (JObject) JsonConvert.DeserializeObject(str);
            var files = obj["files"].Value<JArray>();
            var json = new JObject
            {
                ["projectID"] = Int32.Parse(projectId), ["fileID"] = Int32.Parse(fileId), ["required"] = true
            };
            foreach (var file in files)
                if (file["projectID"].Value<string>().Equals(json["projectID"].Value<string>()))
                {
                    file.Remove();
                    files.Add(json);
                    return;
                }

            files.Add(json);
            File.WriteAllText(main, obj.ToString());
        }
        public static async Task SaveFileAsync(Stream srcStream, string path)
        {
            var fs = File.Create(path);
            await srcStream.CopyToAsync(fs).ContinueWith(t =>
            {
                fs.Dispose();
                srcStream.Dispose();
            }).ConfigureAwait(false);
        }

        public static string FindFile(string dir, string filename)
        {
            if (File.Exists(Path.Combine(dir, filename))) return Path.Combine(dir, filename);

            if (dir.Equals(Path.GetPathRoot(dir))) return null;
            return FindFile(Path.GetDirectoryName(dir), filename);
        }
    }

}