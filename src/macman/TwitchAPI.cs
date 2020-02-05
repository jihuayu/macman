using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace macman
{
    public class TwitchAPI
    {
        public static async Task<JsonElement?> GetVersionFileAsync(string id, string version)
        {
            var url = "https://addons-ecs.forgesvc.net/api/v2/addon/" + id + "/files";
            var httpClient = new HttpClient();
            var str = await httpClient.GetStreamAsync(url);
            var j = JsonDocument.Parse(str).RootElement;
            foreach (var i in j.EnumerateArray())
            {
                if (i.GetProperty("gameVersion").EnumerateArray().Any(_ => _.GetString() == version))
                {
                    return j;
                }
                return null;
            }
        }

        public static async Task<JArray> Search(string name, string version, string type, int pages = 0)
        {
            var url = "";
            if (type == "mod")
                url = "https://addons-ecs.forgesvc.net/api/v2/addon/search?gameId=432&gameVersion=" + version +
                      "&index=" + pages + "&pageSize=10&sectionId=6&sort=0&searchFilter=" + name;
            
            var str = await Util.GetHttpResponse(url, 10000);
            Util.Debug(str);
            return (JArray) JsonConvert.DeserializeObject(str);
        }
    }
}