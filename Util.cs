using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace fmcl
{
    public class Util
    {
        public static bool DEBUG = true;

        public static bool IfNum(string str)
        {
            bool flag = true;
            foreach (var c in str)
            {
                if (c > '9' || c < '0')
                {
                    flag = false;
                }
            }

            return flag;
        }
        public static async Task<string> GetHttpResponse(string url, int Timeout)
        {
            HttpClient client = new HttpClient();

            Debug("请求" + url);
            try	
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(url);
                Debug("请求完成" + url);
                return responseBody;
            }  
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
            return null;
        }

        public static async Task Download(string url, string path)
        {
            WebClient myWebClient = new WebClient();
            Debug("开始下载" + url);
            try
            {
                myWebClient.DownloadFileAsync(new Uri(url), path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            Debug("下载完成" + url);
        }

        public static void Debug(object o)
        {
            if (DEBUG)
            {
                Console.WriteLine(o);
            }
        }
        public static void createdir(string filefullpath)

        {

            bool bexistfile = false;
            if (File.Exists(filefullpath))
            {
                bexistfile = true;
            }
            else //判断路径中的文件夹是否存在
            {
                string dirpath = filefullpath.Substring(0, filefullpath.LastIndexOf('\\'));
                string[] pathes = dirpath.Split('\\');
                if (pathes.Length > 1)
                {
                    string path = pathes[0];
                    for (int i = 1; i < pathes.Length; i++)
                    {
                        path += "\\" + pathes[i];
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                }
            }
        }
        
    }
}