using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace HP.TS.Devops.CentralConnect.ClientRegistration
{
    /// <summary>
    ///   A client registration service request.
    /// </summary>
    /// <copyright>
    ///   (C) Copyright 2011 Hewlett-Packard Development Company, L.P.
    /// </copyright>
    /// <author>Mark McDowell</author>
    public class ClientRegistrationRequest
    {
        private RequestBody RequestBody { get; set; }
        private string Template { get; set; }
        /// <summary>
        ///   Creates a new ClientRegistrationRequest.
        /// </summary>
        public ClientRegistrationRequest(RequestBody requestBody)
        {
            this.RequestBody = requestBody;
            this.Template = Templates.ClientRegistrationRequest;
        }

        /// <summary>
        ///   Converts the request to a string.
        /// </summary>
        /// <returns>the request as a string</returns>
        public string RequestString
        {
            get
            {
                return string.Format(Template,
                    this.RequestBody.TimestampGenerated ?? DateTime.UtcNow.ToString(@"yyyy/MM/dd HH:mm:ss \G\M\T"),
                    this.RequestBody.CoreSystemModel,
                    this.RequestBody.UserApprovedSystemSerialNumber ?? "",
                    this.RequestBody.AutoDetectedSystemSerialNumber,
                    this.RequestBody.UserApprovedProductId ?? "",
                    this.RequestBody.ProductId,
                    this.RequestBody.CollectorType ?? "InsightRS",
                    this.RequestBody.SystemIdent ?? "",
                    this.RequestBody.FQDN,
                    this.RequestBody.Hostname,
                    this.RequestBody.OSNameAndVersionString,
                    this.RequestBody.SystemIdent ?? "",
                    this.RequestBody.SystemModel,
                    this.RequestBody.MacAddress,
                    this.RequestBody.IPAddress,
                    this.RequestBody.GUID ?? "",
                    this.RequestBody.City ?? "",
                    this.RequestBody.Region ?? "",
                    this.RequestBody.PostalCode ?? "",
                    this.RequestBody.TimeZone ?? "MDT",
                    this.RequestBody.Country ?? "",
                    this.RequestBody.Business ?? "Show Business",
                    this.RequestBody.Name ?? "",
                    //--prsdata--
                    this.RequestBody.PRSCaption,
                    this.RequestBody.PRSDescription ?? "",
                    this.RequestBody.PRSElementName ?? "",
                    this.RequestBody.PRSDirtyFlag,
                    this.RequestBody.PRSUniqueName,
                    this.RequestBody.PRSAddressType,
                    this.RequestBody.PRSAddress1,
                    this.RequestBody.PRSAddress2 ?? "",
                    this.RequestBody.PRSAddress3 ?? "",
                    this.RequestBody.PRSAddress4 ?? "",
                    this.RequestBody.PRSCity,
                    this.RequestBody.PRSRegion,
                    this.RequestBody.PRSPostalCode,
                    this.RequestBody.PRSTimeZone ?? "MDT",
                    this.RequestBody.PRSCountry,
                    //--prsdata--
                    //----ISEECustomer---
                    this.RequestBody.ISEECusCaption,
                     this.RequestBody.ISEECusDescription ?? "",
                     this.RequestBody.ISEECusElementName ?? "",
                     this.RequestBody.ISEECusDirtyFlag,
                     this.RequestBody.ISEECusPRSUniqueName,
                     this.RequestBody.ISEECusBusiness ?? "Show Business",
                     this.RequestBody.ISEECusName,
                     this.RequestBody.ISEECusExternalReferenceSystem0 ?? "",
                     this.RequestBody.ISEECusExternalReferenceSystem1 ?? "",
                     this.RequestBody.ISEECusExternalReferenceKey0 ?? "",
                     this.RequestBody.ISEECusExternalReferenceKey1 ?? "",
                    //----ISEECustomer---
                    //--cimsw--
                     this.RequestBody.CIMSWCaption ?? "",
                     this.RequestBody.CIMSWDescription ?? "",
                     this.RequestBody.CIMSWElementName ?? "",
                     this.RequestBody.CIMSWInstallDate ?? DateTime.UtcNow.ToString("O"),
                     this.RequestBody.CIMSWName ?? "",
                     this.RequestBody.CIMSWOperationalStatus0,
                     this.RequestBody.CIMSWOperationalStatus1,
                     this.RequestBody.CIMSWOperationalStatus2,
                     this.RequestBody.CIMSWStatusDescriptors0 ?? "",
                     this.RequestBody.CIMSWStatusDescriptors1 ?? "",
                     this.RequestBody.CIMSWStatusDescriptors2 ?? "",
                     this.RequestBody.CIMSWIdentifyingNumber ?? "",
                     this.RequestBody.CIMSWProductName ?? "",
                     this.RequestBody.CIMSWVendor ?? "",
                     this.RequestBody.CIMSWVersion ?? "",
                    //--cimsw--
                    //--iseeent--
                     this.RequestBody.ISEEEntEntitlementType ?? "",
                     this.RequestBody.ISEEEntEntitlementId ?? "",
                     this.RequestBody.ISEEEntSerialNumber,
                     this.RequestBody.ISEEEntProductNumber,
                     this.RequestBody.ISEEEntProductId,
                     this.RequestBody.ISEEEntObligationId ?? "",
                    //--iseeent--
                    //--iseeperson--
                    this.RequestBody.ISEEFirstPersonFirstName,
                    this.RequestBody.ISEEFirstPersonLastName,
                    this.RequestBody.ISEEFirstPersonEmailAddress,
                    this.RequestBody.ISEEFirstPersonTelephoneNumber,
                    this.RequestBody.ISEESecondPersonFirstName,
                    this.RequestBody.ISEESecondPersonLastName,
                    this.RequestBody.ISEESecondPersonEmailAddress,
                    this.RequestBody.ISEESecondPersonTelephoneNumber
                    //--iseeperson--
                );
            }
        }
    }
}