using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Macman.Utils
{
    public static class DownloadUtils
    {
        public static async Task<Stream> GetStreamAsync(string url) => await GetStreamAsync(new Uri(url));
        public static async Task<Stream> GetStreamAsync(Uri uri)
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetStreamAsync(uri).ConfigureAwait(false);
        }

        public static async Task DownloadFileAsync(string url, string path, bool force)
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
    }
}
