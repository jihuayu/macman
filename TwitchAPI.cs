using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace fmcl
{
    public class TwitchAPI
    {
        
        public static JObject GetVersionFile(string id, string version)
        {
            var url = "https://addons-ecs.forgesvc.net/api/v2/addon/"+id+"/files";
            var str = Util.GetHttpResponse(url, 6000);
            JArray json = (JArray)JsonConvert.DeserializeObject(str);
            JObject ret = null;
            foreach (var i in json)
            {
                foreach (var j in i["gameVersion"])
                {
                    if (j.Value<string>()==version)
                    {
                        ret = (JObject)(i);
                    }
                }
            }
            return ret;
        }
    }
}