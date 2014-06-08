using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace HP.TS.Devops.CentralConnect.ClientRegistration
{
    public class RequestBody
    {
        public string TimestampGenerated { get; set; }
        public string CoreSystemModel { get; set; }
        public string UserApprovedSystemSerialNumber { get; set; }
        public string AutoDetectedSystemSerialNumber { get; set; }
        public string UserApprovedProductId { get; set; }
        public string ProductId { get; set; }
        public string CollectorType { get; set; }
        public string SystemIdent { get; set; }
        public string FQDN { get; set; }
        public string Hostname { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string TimeZone { get; set; }
        public string Country { get; set; }
        public string Business { get; set; }
        public string Name { get; set; }
        public string OSNameAndVersionString { get; set; }
        public string SystemModel { get; set; }
        public string MacAddress { get; set; }
        public string IPAddress { get; set; }
        public string GUID { get; set; }
        //--prs--
        public string PRSCaption { get; set; }
        public string PRSDescription { get; set; }
        public string PRSElementName { get; set; }
        public int PRSDirtyFlag { get; set; }
        public string PRSUniqueName { get; set; }
        public int PRSAddressType { get; set; }
        public string PRSAddress1 { get; set; }
        public string PRSAddress2 { get; set; }
        public string PRSAddress3 { get; set; }
        public string PRSAddress4 { get; set; }
        public string PRSCity { get; set; }
        public string PRSRegion { get; set; }
        public string PRSPostalCode { get; set; }
        public string PRSTimeZone { get; set; }
        public string PRSCountry { get; set; }
        //--prs--
        //--iseecus--
        public string ISEECusCaption { get; set; }
        public string ISEECusDescription { get; set; }
        public string ISEECusElementName { get; set; }
        public int ISEECusDirtyFlag { get; set; }
        public string ISEECusPRSUniqueName { get; set; }
        public string ISEECusBusiness { get; set; }
        public string ISEECusName { get; set; }
        public string ISEECusExternalReferenceSystem0 { get; set; }
        public string ISEECusExternalReferenceSystem1 { get; set; }
        public string ISEECusExternalReferenceKey0 { get; set; }
        public string ISEECusExternalReferenceKey1 { get; set; }
        //--iseecus--
        //--cimsw--
        public string CIMSWCaption { get; set; }
        public string CIMSWDescription { get; set; }
        public string CIMSWElementName { get; set; }
        public string CIMSWInstallDate { get; set; }
        public string CIMSWName { get; set; }
        public int CIMSWOperationalStatus0 { get; set; }
        public int CIMSWOperationalStatus1 { get; set; }
        public int CIMSWOperationalStatus2 { get; set; }
        public string CIMSWStatusDescriptors0 { get; set; }
        public string CIMSWStatusDescriptors1 { get; set; }
        public string CIMSWStatusDescriptors2 { get; set; }
        public string CIMSWIdentifyingNumber { get; set; }
        public string CIMSWProductName { get; set; }
        public string CIMSWVendor { get; set; }
        public string CIMSWVersion { get; set; }
        //--cimsw--
        //--ISEEEnt---
        public string ISEEEntEntitlementType { get; set; }
        public string ISEEEntEntitlementId { get; set; }
        public string ISEEEntSerialNumber { get; set; }
        public string ISEEEntProductNumber { get; set; }
        public string ISEEEntProductId { get; set; }
        public string ISEEEntObligationId { get; set; }
        //--ISEEEnt---
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
