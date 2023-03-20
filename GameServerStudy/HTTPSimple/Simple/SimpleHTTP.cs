using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HTTPSimple.Simple
{
    public class SimpleHTTP
    {
        public static void HowToHTTP()
        {
            string webRequest = "";

            var reqData = new HTTPData();
            reqData.InitData();
            reqData.SetHeaderField("key", "unknown");
            reqData.SetHeaderField("seq", 1);
            reqData.SetBodyField("api_name", "CSharpWebRequest");
            reqData.SetBodyField("client_tick", Environment.TickCount);
            reqData.SetBodyField("user_id", 1000);

            var paramDict = new Dictionary<string, object>();
            paramDict.Add("client_tick", Environment.TickCount);
            reqData.SetBodyField("params", paramDict);

            webRequest = reqData.GetReqJson();

            string url = "http://192.168.56.201/projects/gateway.php";
            string res = "";
            var req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Proxy = null;
            req.CachePolicy = null;
            req.Method = WebRequestMethods.Http.Post;
            req.ContentType = "application/json";
            req.Timeout = 30 * 1000;
            //req.Headers.Add("Authorization", "BASIC SGVsbG8="); // 헤더 추가 방법

            using (var reqStream = req.GetRequestStream())
            {
                var bin = Encoding.UTF8.GetBytes(webRequest);
                reqStream.Write(bin, 0, bin.Length);
            }

            // 블로킹 걸린다.!!
            using (var resp = (HttpWebResponse)req.GetResponse())
            {
                HttpStatusCode Code = resp.StatusCode;
                if (Code != HttpStatusCode.OK)
                {
                    return;
                }
                var respStream = resp.GetResponseStream();
                using (var sr = new StreamReader(respStream))
                {
                    res = sr.ReadToEnd();
                }
            }
            JsonHelper.Deserialize<HTTPData>(reqData, res);
            //reqData.Deserialize
            var result = reqData._responseDict["result"] as Dictionary<string, object>;
            Console.WriteLine(result["client_tick"]);
        }
    }
}
