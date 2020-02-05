using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace macman
{
    public class Tasks
    {
        public static async Task DownloadModAsync(string id, string version, string path)
        {
            var json = await TwitchAPI.GetVersionFileAsync(id, version);
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
            Console.WriteLine("下载" +fileName + "中");
            if (!File.Exists(fullPath))
            {
                await Util.DownloadAsync(downloadUrl, fullPath);
            }
            foreach (var dependency in dependencies.Where(dependency => dependency.GetProperty("type").GetInt32() == 3))
            {
                var result =
                    await TwitchAPI.GetVersionFileAsync(dependency.GetProperty("addonId").GetString(), version);
                if (result.HasValue)
                {
                    await DownloadModAsync(result.Value, version, path);
                }
            }
        }

        public static async Task<List<string>> FindAndDl(string name, string version, string type, int pages = 0)
        {
            while (true)
            {
                var arr = await TwitchAPI.Search(name, version, type, pages);
                if (arr.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("没有符合条件的东西");
                    Console.ForegroundColor = ConsoleColor.White;
                    return null;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("编号\t名字\t作者");
                for (var i = 0; i < arr.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(i);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\t" + arr[i]["name"].Value<string>() + "\t" + arr[i]["authors"][0]["name"].Value<string>());
                }

                Console.WriteLine("请输入你要下载的mod编号");
                Console.WriteLine("输入n下一页");
                Console.ForegroundColor = ConsoleColor.White;
                var str = Console.ReadLine();
                if (str == "n" || str == "N" || str == "Next" || str == "next")
                {
                    pages = pages + 1;
                    continue;
                }

                var idList = new List<string>();
                var dlid = str.ToCharArray();
                foreach (var id in dlid)
                {
                    Debug.WriteLine(id);
                    if (id >= '0' && id <= '9')
                    {
                        if (id - '0' > arr.Count)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("输入编号" + id + "有误");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            idList.Add(arr[id - '0']["id"].Value<string>());
                        }
                    }
                }

                if (idList.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("输入编号有误,请输入正确的编号");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                return idList;
                break;
            }
        }
    }
}