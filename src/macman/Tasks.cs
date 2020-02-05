using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace macman
{
    public static class Tasks
    {
        public static async Task DownloadModAsync(string id, string version, string path)
        {
            var json = await TwitchApi.GetVersionFileAsync(id, version);
            if (json.HasValue)
            {
                await DownloadModAsync(json.Value, version, path);
            }
        }

        public static async Task DownloadModAsync(JsonElement json, string version, string path)
        {
            var fileName = json.GetProperty("fileName").GetString();
            var downloadUrl = json.GetProperty("downloadUrl").GetString();
            var dependencies = json.GetProperty("dependencies").EnumerateArray().ToList();
            var fullPath = Path.Combine(path, fileName);
            Console.WriteLine("下载" +fileName + "中···");
            if (!File.Exists(fullPath))
            {
                await Util.DownloadAsync(downloadUrl, fullPath);
            }
            foreach (var dependency in dependencies.Where(dependency => dependency.GetProperty("type").GetInt32() == 3))
            {
                var result =
                    await TwitchApi.GetVersionFileAsync(dependency.GetProperty("addonId").GetInt64().ToString(), version);
                if (result.HasValue)
                {
                    await DownloadModAsync(result.Value, version, path);
                }
            }
        }

        public static async Task<List<string>> FindAsync(string name, string version, int pageCount = 0)
        {
            while (true)
            {
                var arr = (await TwitchApi.SearchAsync(name, version, pageCount)).ToList();
                if (arr.Count==0)
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
                    Console.WriteLine("\t" + arr[i].GetProperty("name").GetString() + "\t" + arr[i].GetProperty("authors")[0].GetProperty("name").GetString());
                }

                Console.WriteLine("请输入你要下载的mod编号");
                Console.WriteLine("输入n下一页");
                Console.ForegroundColor = ConsoleColor.White;
                var ids = new List<string>();
                var str = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(str))
                {
                    pageCount++;
                    continue;
                }
                foreach (var s in str.Split())
                {
                    var isNum = int.TryParse(s, out var num);
                    if (!isNum)
                    {
                        continue;
                    }
                    Debug.WriteLine(num);
                    if (num < 0 || num > 9) continue;
                    if (num > arr.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("您所输入的编号[" + num + "]有误");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        ids.Add(arr[num].GetProperty("id").GetInt64().ToString());
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