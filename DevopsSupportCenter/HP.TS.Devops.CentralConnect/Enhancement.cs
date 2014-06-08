using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.CentralConnect
{
    public class Argument
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class Enhancement
    {
        public string Environment { get; set; }
        public string Type { get; set; }
        public List<Argument> Arguments { get; set; }
    }
    public class MetricsEnhancement : Enhancement
    {
        public string ClientName { get; set; }
        public string Domain { get; set; }
        public string SupportPartnerId { get; set; }
        public string ServicePartnerId { get; set; }
    }
    public class CollectionEnhancement : Enhancement
    {
        public string DeviceName { get; set; }
        public string Domain { get; set; }
    }
    public class EventEnhancement : Enhancement
    {
        public string DeviceName { get; set; }
        public string Domain { get; set; }
    }
}
