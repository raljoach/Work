using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyManagerTestTool
{
    public enum AccountAction
    {
        Login_Information
    }

    public class RestApi
    {        
        public RestApi(string protocol, string host, string version)
        {
            this.BaseUrl = string.Format("{0}://{1}/restapi/{2}", protocol, host, version);
        }

        private string BaseUrl
        {
            get;
            set;
        }

        public string AccountUrl(AccountAction acctAction)
        {
            return string.Format("{0}/{1}",this.BaseUrl, acctAction);
        }
    }
}
