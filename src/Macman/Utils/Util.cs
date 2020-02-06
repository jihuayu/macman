﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Macman.Utils
{
    public static class Util
    {
        public static async Task DownloadAsync(string url, string path, bool force)
        {
            if (!force && File.Exists(path)) return;
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

        public static void CreateDirectory(string dir)
        {
            if (File.Exists(Path.GetPathRoot(dir))) CreateDirectory(Path.GetPathRoot(dir));
            Directory.CreateDirectory(dir);
        }

        public static string FindFile(string dir, string filename)
        {
            if (File.Exists(Path.Combine(dir, filename))) return Path.Combine(dir, filename);

            if (dir.Equals(Path.GetPathRoot(dir))) return null;
            return FindFile(Path.GetDirectoryName(dir), filename);
        }
    }
}