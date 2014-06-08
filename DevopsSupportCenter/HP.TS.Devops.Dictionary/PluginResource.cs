using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.Dictionary
{
    public class Resource
    {
        public string ResourceType { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string CreateBy { get; set; }
    }
    public class PluginMap
    {
        public string PluginClass { get; set; }
        public string PluginType { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string ClassFullName { get; set; }
    }
}
