using System;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;

namespace fmcl
{
    public class Tasks
    {
        public static void DownloadMcmod(string id,string version,string path)
        {
            JObject o = TwitchAPI.GetVersionFile(id, version);
            DownloadMcmod(o, version, path);
        }

        public static void DownloadMcmod(JObject o, string version, string path)
        {
            Util.Download(o["downloadUrl"].Value<string>(), path + o["fileName"].Value<string>());
            foreach (var de in o["dependencies"])
            {
                if (de["type"].Value<int>()==3)
                {
                    
                    DownloadMcmod(TwitchAPI.GetVersionFile(de["addonId"].Value<string>(), version),version,path);
                }
            }
        }

        public static string FindAndDl(string name,string version,string type,int pages = 0)
        {
            var arr = TwitchAPI.Search(name, version, type,pages);
            if (arr.Count==0)
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
                Console.WriteLine("\t"+arr[i]["name"].Value<string>()+"\t"+arr[i]["authors"][0]["name"].Value<string>());
            }
            Console.WriteLine("请输入你要下载的mod编号");
            Console.WriteLine("输入n下一页");
            Console.ForegroundColor = ConsoleColor.White;
            var str = Console.ReadLine();
            if (str=="n"||str=="N")
            {
                return FindAndDl(name, version, type, pages + 1);
            }
            if (Util.IfNum(str))
            {
                if (int.Parse(str)>arr.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("输入编号有误,请重新输入");
                    Console.ForegroundColor = ConsoleColor.White;
                    return FindAndDl(name,version,type,pages);
                }
                return arr[int.Parse(str)]["id"].Value<string>();
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("输入编号有误,请输入正确的编号");
            Console.ForegroundColor = ConsoleColor.White;
            return FindAndDl(name,version,type,pages);
        }
    }
}