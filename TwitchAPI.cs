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
            var str = Util.GetHttpResponse(url, 10000);
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

        public static JArray Search(string name,string version,string type,int pages = 0)
        {
            var url = "";
            if (type=="mcmod")
            {
                url =  "https://addons-ecs.forgesvc.net/api/v2/addon/search?gameId=432&gameVersion="+version+"&index="+pages+"&pageSize=10&sectionId=6&sort=0&searchFilter="+name;
            }

            return (JArray)JsonConvert.DeserializeObject(Util.GetHttpResponse(url, 10000));
        }
    }
}