using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyManagerTestTool
{
    class KeyManagerInfo
    {
        public KeyManagerInfo(KeyManType type)
        {
            this.Type = type;
        }

        public KeyManType Type { get; set; }

        public string Host { get; set; }

        public string WebPort { get; set; }

        public string ServicePort { get; set; }
    }
}
