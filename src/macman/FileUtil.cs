using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace macman
{
    public static class FileUtil
    {
        public static void SaveModFile(string path, string projectID, string fileID)
        {
            var main = Util.FindFile(path, "manifest.json");
            if (main == null) return;
            var str = File.ReadAllText(main);
            var obj = (JObject) JsonConvert.DeserializeObject(str);
            var files = obj["files"].Value<JArray>();
            var json = new JObject
            {
                ["projectID"] = int.Parse(projectID), ["fileID"] = int.Parse(fileID), ["required"] = true
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
    }
}