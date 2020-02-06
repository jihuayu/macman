using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Macman.Io
{
    public static class ApiManager
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

        public static async Task<List<string>> FindAsync(string name, string version, int pageCount = 0)
        {
            while (true)
            {
                var arr = (await SearchAsync(name, version, pageCount)).ToList();
                if (arr.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("没有符合条件的mod");
                    Console.ForegroundColor = ConsoleColor.White;
                    return new List<string>();
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("编号\t名称\t作者");
                for (var i = 0; i < arr.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(i);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\t" + arr[i]["name"].Value<string>() + "\t" +
                                      arr[i]["authors"][0]["name"].Value<string>());
                }

                Console.WriteLine("请输入您所要下载的mod编号");
                Console.WriteLine("输入n下一页");
                Console.ForegroundColor = ConsoleColor.White;
                var ids = new List<string>();
                var str = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(str))
                {
                    pageCount++;
                    continue;
                }

                foreach (var s in str.ToCharArray())
                {
                    Debug.WriteLine(s);
                    if (s > '9' || s < '0') continue;

                    var num = s - '0';
                    Debug.WriteLine(num);
                    if (num > arr.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("您所输入的编号[" + num + "]有误");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        ids.Add(arr[num]["id"].Value<string>());
                    }
                }

                if (ids.Count != 0) return ids;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("您所输入的编号有误,请输入正确的编号");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static async Task<string> GetLastForge(string version, bool last)
        {
            var httpClient = new HttpClient();
            var url = "https://addons-ecs.forgesvc.net/api/v2/minecraft/modloader";

            var str = await httpClient.GetStringAsync(url);
            var arr = JArray.Parse(str);
            var list = arr.Where(_ => _["gameVersion"].Value<string>() == version)
                .Where(_ => _["latest"].Value<bool>() == last)
                .Where(_ => _["recommended"].Value<bool>() == !last)
                .ToList();
            if (list.Count == 0)
                list = arr.Where(_ => _["gameVersion"].Value<string>() == version)
                    .Where(_ => _["latest"].Value<bool>())
                    .ToList();

            if (list.Count > 0) return list[0]["name"].Value<string>();

            return "";
        }
    }
}