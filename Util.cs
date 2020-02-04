using System.IO;
using System.Net;
using System.Text;

namespace fmcl
{
    public class Util
    {
        public static bool IfNum(string str)
        {
            bool flag = true;
            foreach (var c in str)
            {
                if (c > '9' || c < '0')
                {
                    flag = false;
                }
            }

            return flag;
        }

        public static string GetHttpResponse(string url, int Timeout)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = Timeout;

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public static void Download(string url, string path)
        {
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile(url,path);
        }
    }
}