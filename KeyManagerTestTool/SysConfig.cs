using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyManagerTestTool
{
    class SysConfig
    {
        public int EnvDetailsRamp { get; set; }

        public string EnvDetailsReadFirst { get; set; }

        public string EnvDetailsWrites { get; set; }

        public string KeyManagerDualMode { get; set; }

        public string KeyManagerRemoteMode { get; set; }
    }
}
