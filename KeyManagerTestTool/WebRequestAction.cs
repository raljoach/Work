using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace KeyManagerTestTool
{
    class WebRequestAction
    {
        private HttpWebRequest _webRequest = null;

        public WebRequestAction(RequestInfo request)
        {
            this.RequestInfo = request;
            _webRequest = CreateWebRequest();
        }

        public RequestInfo RequestInfo { get; set; }
        public string Content { get; set; }

        public void Execute()
        {
            //write bytes to request stream
            _webRequest.ContentLength = (RequestInfo.BodyContent == null) ? 0 : RequestInfo.BodyContent.Length;
            if (_webRequest != null && RequestInfo.BodyContent != null && RequestInfo.BodyContent.Length > 0)
            {
                using (var stream = _webRequest.GetRequestStream())
                {
                    stream.Write(RequestInfo.BodyContent, 0, RequestInfo.BodyContent.Length);
                }
            }
            using (var response = _webRequest.GetResponse() as HttpWebResponse)
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    Content = rd.ReadToEnd();
                }
            }
        }

        private HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(this.RequestInfo.Uri);
            webRequest.ContentType = this.RequestInfo.ContentType;
            webRequest.Accept = this.RequestInfo.ContentType;
            webRequest.Method = this.RequestInfo.Method.ToString();
            webRequest.UserAgent = this.RequestInfo.UserAgent;
            if (RequestInfo.Headers != null)
            {
                foreach (var header in RequestInfo.Headers)
                {
                    webRequest.Headers.Add(header.Key.ToString(), header.Value.ToString());
                }
            }

            return webRequest;
        }        
    }
}
