using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Macman.Utils;
using Newtonsoft.Json.Linq;

namespace Macman.Io
{
    internal class DownloadManager
    {
        public static async Task DownloadModAsync(string id, string version, string path, bool force)
        {
            var json = await ApiManager.GetVersionFileAsync(id, version);
            json["addonId"] = id;
            if (json.HasValues) await DownloadModAsync(json, version, path, force);
        }

        public static async Task DownloadFileAsync(string id, string file, string path, bool force)
        {
            var url = await ApiManager.GetDownloadUrl(id, file);
            var names = url.Split('/');
            Console.WriteLine("下载" + names[names.Length - 1] + "中···");
            var p = Path.Combine(path, names[names.Length - 1]);
            await Util.DownloadAsync(url, p, force);
        }

        public static async Task DownloadModAsync(JObject json, string version, string path, bool force)
        {
            var fileName = json["fileName"].Value<string>();
            var downloadUrl = json["downloadUrl"].Value<string>();
            var dependencies = json["dependencies"].Value<JArray>();
            var fullPath = Path.Combine(path, fileName);
            Console.WriteLine("下载" + fileName + "中···");
            FileUtil.SaveModFile(path, json["addonId"].Value<string>(), json["id"].Value<string>());
            if (!File.Exists(fullPath)) await Util.DownloadAsync(downloadUrl, fullPath, force);

            foreach (var dependency in dependencies.Where(dependency => dependency["type"].Value<int>() == 3))
            {
                var result =
                    await ApiManager.GetVersionFileAsync(dependency["addonId"].Value<string>(), version);
                result["addonId"] = dependency["addonId"].Value<string>();
                if (result.HasValues) await DownloadModAsync(result, version, path, force);
            }
        }
    }
}