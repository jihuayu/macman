using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace fmcl
{
    public class Tasks
    {
        public static async Task DownloadMcmod(string id, string version, string path)
        {
            JObject o = await TwitchAPI.GetVersionFile(id, version);
            DownloadMcmod(o, version, path);
        }

        public static async Task DownloadMcmod(JObject o, string version, string path)
        {
            Console.WriteLine("下载" + o["fileName"].Value<string>() + "中");
            if(!File.Exists(path + o["fileName"].Value<string>()))
                Util.Download(o["downloadUrl"].Value<string>(), path + o["fileName"].Value<string>());
            foreach (var de in o["dependencies"])
            {
                if (de["type"].Value<int>() == 3)
                {
                    DownloadMcmod(await TwitchAPI.GetVersionFile(de["addonId"].Value<string>(), version), version, path);
                }
            }
        }

        public static async Task<List<string>> FindAndDl(string name, string version, string type, int pages = 0)
        {
            var arr =  await TwitchAPI.Search(name, version, type, pages);
            if (arr.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("没有符合条件的东西");
                Console.ForegroundColor = ConsoleColor.White;
                return null;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("编号\t名字\t作者");
            for (int i = 0; i < arr.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(i);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\t" + arr[i]["name"].Value<string>() + "\t" +
                                  arr[i]["authors"][0]["name"].Value<string>());
            }

            Console.WriteLine("请输入你要下载的mod编号");
            Console.WriteLine("输入n下一页");
            Console.ForegroundColor = ConsoleColor.White;
            var str = Console.ReadLine();
            if (str == "n" || str == "N" || str == "Next" || str == "next")
            {
                return await FindAndDl(name, version, type, pages + 1);
            }
            var idList = new List<string>();
            var dlid = str.ToCharArray();
            foreach (var id in dlid)
            {
                Util.Debug(id);
                if (id>='0'&&id<='9')
                {
                    if (id-'0'> arr.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("输入编号"+id+"有误");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        idList.Add(arr[id-'0']["id"].Value<string>());
                    }
                }
            }

            if (idList.Count==0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("输入编号有误,请输入正确的编号");
                Console.ForegroundColor = ConsoleColor.White;
                return await FindAndDl(name, version, type, pages);
            }

            return idList;
        }
    }
}