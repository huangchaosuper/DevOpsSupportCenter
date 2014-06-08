using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.CentralConnect
{
    public class Client
    {
        public string ClientName { get; set; }
        public string Domain { get; set; }
        public string ProductNumber { get; set; }
        public string SerialNumber { get; set; }
        public string CoreSystemModel { get; set; }
        public string MAC { get; internal set; }
        public string OSName { get; set; }
        public string OSVersion { get; set; }
        public string IPAddress { get; internal set; }
        public string CompanyName { get; set; }
        public string PrimaryContact { get; set; }
        public string SecondaryContact { get; set; }
        public string CreateBy { get; set; }
    }
    public class RegisterClientMessage
    {
        public string Environment { get; set; }
        public string ClientName { get; set; }
        public string Domain { get; set; }
        public string HPPID { get; set; }
        public string Password { get; set; }
        public string CSID { get; set; }
        public string OSGDID { get; set; }
        public string HWGDID { get; set; }
        public string RegistrationToken { get; set; }
        public bool Visible { get; set; }
        public string CreateBy { get; set; }
    }
    public class MetricsMessage
    {
        public string Environment { get; set; }
        public string ClientName { get; set; }
        public string Domain { get; set; }
        public string PackageName { get; set; }
        public string SupportPartnerId { get; set; }
        public string ServicePartnerId { get; set; }
        public string CreateBy { get; set; }
    }
}
