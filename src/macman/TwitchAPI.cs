using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace macman
{
    public static class TwitchApi
    {
        public static async Task<JObject> GetVersionFileAsync(string id, string version)
        {
            var url = "https://addons-ecs.forgesvc.net/api/v2/addon/" + id + "/files";
            var httpClient = new HttpClient();
            var str = await httpClient.GetStringAsync(url);
            var arr = JArray.Parse(str);
            var list = arr.Where(_ => _["gameVersion"].Value<JArray>().Any(i => i.Value<string>() == version))
                .ToList();
            if (list.Count == 0) return null;
            return (JObject) list.OrderByDescending(_ => DateTime.Parse(_["fileDate"].Value<string>())).First();
        }

        public static async Task<JArray> SearchAsync(string name, string version, int pageCount = 0)
        {
            var httpClient = new HttpClient();
            var url = "https://addons-ecs.forgesvc.net/api/v2/addon/search?gameId=432&gameVersion=" + version +
                      "&index=" + pageCount + "&pageSize=10&sectionId=6&sort=0&searchFilter=" + name;

            var str = await httpClient.GetStringAsync(url);
            return JArray.Parse(str);
        }

        public static async Task<string> GetDownloadUrl(string project, string file)
        {
            var httpClient = new HttpClient();
            var url = "https://addons-ecs.forgesvc.net/api/v2/addon/" + project + "/file/" + file + "/download-url";

            var str = await httpClient.GetStringAsync(url);
            return str;
        }

        public static async Task<string> GetLastForge(string version, bool last)
        {
            var httpClient = new HttpClient();
            var url = "https://addons-ecs.forgesvc.net/api/v2/minecraft/modloader";

            var str = await httpClient.GetStringAsync(url);
            var arr = (JArray) JsonConvert.DeserializeObject(str);
            var list = arr.Where(_ => _["gameVersion"].Value<string>() == version)
                .Where(_ => _["latest"].Value<bool>() == last)
                .Where(_ => _["recommended"].Value<bool>() == !last)
                .ToList();
            if (list.Count == 0)
            {
                list = arr.Where(_ => _["gameVersion"].Value<string>() == version)
                    .Where(_ => _["latest"].Value<bool>())
                    .ToList();
            }

            if (list.Count > 0)
            {
                return list[0]["name"].Value<string>();
            }

            return "";
        }
    }
}