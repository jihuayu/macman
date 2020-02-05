using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace macman
{
    public class Util
    {
        public static async Task DownloadAsync(string url, string path)
        {
            var httpClient = new HttpClient();
            Debug.WriteLine("开始下载" + url);
            try
            {
                using var stream = await httpClient.GetStreamAsync(url);
                using var fs = File.Create(path);
                await stream.CopyToAsync(fs);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Debug.WriteLine("下载完成" + url);
        }

        

        public static void createdir(string filefullpath)

        {
            var bexistfile = false;
            if (File.Exists(filefullpath))
            {
                bexistfile = true;
            }
            else //判断路径中的文件夹是否存在
            {
                var dirpath = filefullpath.Substring(0, filefullpath.LastIndexOf('\\'));
                var pathes = dirpath.Split('\\');
                if (pathes.Length > 1)
                {
                    var path = pathes[0];
                    for (var i = 1; i < pathes.Length; i++)
                    {
                        path += "\\" + pathes[i];
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    }
                }
            }
        }
    }
}