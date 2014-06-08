using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.CentralConnect.Plugin
{
    public class MetricsRequest
    {
        public Dictionary<string, string> EnhancementArguments { get; set; }
        public ClientInformation ClientInformation { get; set; }
        public List<DeviceInformation> DeviceInformations { get; set; }
        public string Workspace { get; set; }
    }

    public class EventRequest
    {
        public Dictionary<string, string> EnhancementArguments { get; set; }
        public ClientInformation ClientInformation { get; set; }
        public DeviceInformation DeviceInformation { get; set; }
    }

    public class CollectionRequest
    {
        public Dictionary<string, string> EnhancementArguments { get; set; }
        public ClientInformation ClientInformation { get; set; }
        public DeviceInformation DeviceInformation { get; set; }
    }
}
