using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace HP.TS.Devops.CentralConnect.OOSRegistration
{
    public class RequestBody
    {
        public string AutoDetectedSystemSerialNumber { get; set; }
        public string TimestampGenerated { get; set; }
        public string ProductId { get; set; }
        public string SystemModel { get; set; }
        public string CoreSystemModel { get; set; }
        public string Hostname { get; set; }
        public string FQDN { get; set; }
        public string IPAddress { get; set; }
        public string Client_HPPID { get; set; }
        public string SAID { get; set; }
        public string MacAddress { get; set; }
        public string OSNameAndVersionString { get; set; }
        //--prs--
        public string PRSAddress1 { get; set; }
        public string PRSCity { get; set; }
        public string PRSRegion { get; set; }
        public string PRSPostalCode { get; set; }
        public string PRSTimeZone { get; set; }
        public string PRSCountry { get; set; }
        //--prs--
        //--ISEEEnt---
        public string ISEEEntEntitlementType { get; set; }
        public string ISEEEntEntitlementId { get; set; }
        public string ISEEEntSerialNumber { get; set; }
        public string ISEEEntProductNumber { get; set; }
        public string ISEEEntProductId { get; set; }
        //--ISEEEnt---
        //--iseecus--
        public string ISEECusBusiness { get; set; }
        public string ISEECusName { get; set; }
        //--iseecus--
        //--iseeperson--
        public string ISEEFirstPersonFirstName { get; set; }
        public string ISEEFirstPersonLastName { get; set; }
        public string ISEEFirstPersonEmailAddress { get; set; }
        public string ISEEFirstPersonTelephoneNumber { get; set; }

        public string ISEESecondPersonFirstName { get; set; }
        public string ISEESecondPersonLastName { get; set; }
        public string ISEESecondPersonEmailAddress { get; set; }
        public string ISEESecondPersonTelephoneNumber { get; set; }
        //--iseeperson--
    }
}
