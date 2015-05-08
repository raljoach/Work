using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyManagerTestTool
{
    public class RequestInfo
    {
        public string Uri { get; set; }
        public System.Net.Http.HttpMethod Method { get; set; }
        public string ContentType { get; set; }
        public string UserAgent { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public byte[] BodyContent { get; set; }
    }
}
