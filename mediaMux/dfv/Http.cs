using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace df
{
    public class Http
    {
        public static void HttpDown(string url, string path)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;


            Stream responseStream = response.GetResponseStream();


            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            byte[] bArr = new byte[4096];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
        }



        public async static Task<T> HttpGetJson<T>(string Url, T obj)
        {
            var res = await HttpGet(Url);
            obj = JsonConvert.DeserializeAnonymousType(res, obj);
            return obj;
        }

        public static Task<string> HttpGet(string Url)
        {
            return Task.Run(() =>
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "charset=UTF-8";

                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    response = (HttpWebResponse)ex.Response;
                }

                if (response == null)
                {
                    throw new Exception("net error！");
                }

                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(retString);

                return retString;
            });

        }
    }
}
