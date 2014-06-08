using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.CentralConnect.Plugin
{
    public class DeviceInformation
    {
        public string ClientName { get; set; }
        public string ClientDomain { get; set; }
        public string DeviceName { get; set; }
        public string Domain { get; set; }
        public string ProductNumber { get; set; }
        public string SerialNumber { get; set; }
        public string CoreSystemModel { get; set; }
        public string MAC { get; set; }
        public string OSName { get; set; }
        public string OSVersion { get; set; }
        public string IPAddress { get; set; }
        public string OSGDID { get; set; }
        public string OSGDIDUsn { get; set; }
        public string HWGDID { get; set; }
        public string HWGDIDUsn { get; set; }
        public string CollectionProblem { get; set; }
        public string MonitoringProblem { get; set; }
    }
}
