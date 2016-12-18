using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using LearnAndPlayWithKinect.Other;

namespace LearnAndPlayWithKinect.Cloud
{
    class Request
    {
        public static string PostRequest(string uri, string json)
        {
            Console.WriteLine("----- REQUEST URL: " + uri);
            Console.WriteLine("----- REQUEST JSON: " + json);

            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(uri);

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = json;
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            Console.WriteLine("----- RESPONSE JSON: " + responseString);

            return responseString;
        }

        public static bool CheckConnection()
        {
            if (!NetworkInterface.GetIsNetworkAvailable()) return false;

            Ping ping = new Ping();

            PingReply pingStatus = ping.Send(IPAddress.Parse(Settings.SERVER_IP));

            if (pingStatus.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string DictionaryToJson(Dictionary<string, string> dict)
        {
            var entries = dict.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";
        }
    }
}
