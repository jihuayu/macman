﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace macman
{
    public static class Util
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
    }
}