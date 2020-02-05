using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace macman
{
    public class TwitchApi
    {
        public static async Task<JsonElement?> GetVersionFileAsync(string id, string version)
        {
            var url = "https://addons-ecs.forgesvc.net/api/v2/addon/" + id + "/files";
            var httpClient = new HttpClient();
            var str = await httpClient.GetStreamAsync(url);
            var j = JsonDocument.Parse(str).RootElement;
            var arr = j.EnumerateArray();
            var list = arr.Where(_ => _.GetProperty("gameVersion").EnumerateArray().Any(i => i.GetString() == version))
                .ToList();
            if (list.Count==0)
            {
                return null;
            }
            return list.OrderByDescending(_ => DateTime.Parse(_.GetProperty("fileDate").GetString())).First();
        }

        public static async Task<IEnumerable<JsonElement>> SearchAsync(string name, string version, int pageCount = 0)
        {
            var httpClient = new HttpClient();
            var url  = "https://addons-ecs.forgesvc.net/api/v2/addon/search?gameId=432&gameVersion=" + version +
                      "&index=" + pageCount + "&pageSize=10&sectionId=6&sort=0&searchFilter=" + name;
            
            var str = await httpClient.GetStringAsync(url);
            return JsonDocument.Parse(str).RootElement.EnumerateArray();
        }
    }
}