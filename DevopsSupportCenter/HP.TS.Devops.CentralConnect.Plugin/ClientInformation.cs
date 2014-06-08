using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.CentralConnect.Plugin
{
    public class ClientInformation
    {
        //client
        public string ClientName { get; set; }
        public string Domain { get; set; }
        public string ProductNumber { get; set; }
        public string SerialNumber { get; set; }
        public string CoreSystemModel { get; set; }
        public string MAC { get; set; }
        public string OSName { get; set; }
        public string OSVersion { get; set; }
        public string IPAddress { get; set; }
        public string HPPID { get; set; }
        public string Password { get; set; }
        public string CSID { get; set; }
        public string OSGDID { get; set; }
        public string HWGDID { get; set; }
        public string RegistrationToken { get; set; }
        //Company
        public string CompanyName { get; set; }
        public string CountryCode { get; set; }
        public string StateProvince { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        //PrimaryContact
        public string PrimaryContact { get; set; }
        public string PrimaryFirstName { get; set; }
        public string PrimaryLastName { get; set; }
        public string PrimaryEmailAddress { get; set; }
        public string PrimaryPhone { get; set; }
        //SecondaryContact
        public string SecondaryContact { get; set; }
        public string SecondaryFirstName { get; set; }
        public string SecondaryLastName { get; set; }
        public string SecondaryEmailAddress { get; set; }
        public string SecondaryPhone { get; set; }
        //PartnerId
        public string SupportPartnerId { get; set; }
        public string ServicePartnerId { get; set; }

    }
}
