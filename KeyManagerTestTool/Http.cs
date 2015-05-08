using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace KeyManagerTestTool
{
    public class Http
    {
        public static T Get<T>(RequestInfo req)
        {
            var content = Get(req);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static string Get(RequestInfo req)
        {
            req.Method = HttpMethod.Get;
            LogRequest(req);
            
            WebRequestAction action = new WebRequestAction(req);
            action.Execute();

            if (action.Content == null)
            {
                throw new InvalidOperationException("GET response has no Content!");
            }
            Console.WriteLine("Response:");
            Console.WriteLine(action.Content);
            return action.Content;
        }

        private static void LogRequest(RequestInfo req)
        {
            Console.WriteLine(
                "{0} Request Uri={1}\r\nContentType={2}\r\nHeaders={3}\r\nContentLength={4}", 
                req.Method, req.Uri, req.ContentType, GetString(req.Headers), (req.BodyContent == null) ? 0 : req.BodyContent.Length);
        }

        private static string GetString(Dictionary<string, string> headers)
        {
            if (headers == null || headers.Count == 0)
                return string.Empty;

            return string.Join(",\r\n", headers.Select(x => string.Format("{0}:{1}", x.Key, x.Value)).ToArray());
        }
    }
}
