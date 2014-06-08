using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.CentralConnect.Plugin
{
    public class PluginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}
