using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace macman
{
    public static class Tasks
    {
        public static async Task DownloadModAsync(string id, string version, string path, bool force)
        {
            var json = await TwitchApi.GetVersionFileAsync(id, version);
            if (json.HasValues) await DownloadModAsync(json, version, path, force);
        }

        public static async Task DownloadFileAsync(string id, string file, string path, bool force)
        {
            var url = await TwitchApi.GetDownloadUrl(id, file);
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
            if (!File.Exists(fullPath)) await Util.DownloadAsync(downloadUrl, fullPath, force);

            foreach (var dependency in dependencies.Where(dependency => dependency["type"].Value<int>() == 3))
            {
                var result =
                    await TwitchApi.GetVersionFileAsync(dependency["addonId"].Value<string>(), version);
                if (result.HasValues) await DownloadModAsync(result, version, path, force);
            }
        }

        public static async Task<List<string>> FindAsync(string name, string version, int pageCount = 0)
        {
            while (true)
            {
                var arr = (await TwitchApi.SearchAsync(name, version, pageCount)).ToList();
                if (arr.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("没有符合条件的mod");
                    Console.ForegroundColor = ConsoleColor.White;
                    return null;
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
    }
}