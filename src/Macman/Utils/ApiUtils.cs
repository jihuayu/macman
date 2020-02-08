using System;
using System.Collections.Generic;
using System.Text;

namespace Macman.Utils
{
    public static class ApiUtils
    {
        public static string GetDownloadUrl(string fileId,string fileName)=> $"https://edge.forgecdn.net/files/{fileId.Substring(0,5)}/{fileId.Substring(5,fileId.Length-4)}/{fileName}";
    }
}
