using System;

namespace HP.TS.Devops.CentralConnect.OOSRegistration
{
    /// <summary>
    ///   An OOS registration service request.
    /// </summary>
    /// <copyright>
    ///   (C) Copyright 2013 Hewlett-Packard Development Company, L.P.
    /// </copyright>
    /// <author>huang.chao</author>
    public class OOSRegistrationRequest
    {
        private RequestBody RequestBody { get; set; }
        private string Template { get; set; }

        /// <summary>
        ///   Creates a new ClientRegistrationRequest.
        /// </summary>
        public OOSRegistrationRequest(RequestBody requestBody)
        {
            this.RequestBody = requestBody;
            this.Template = Templates.OOSRegistrationRequest;
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
                    //--CSID--
                    this.RequestBody.AutoDetectedSystemSerialNumber,
                    this.RequestBody.TimestampGenerated ?? DateTime.UtcNow.ToString(@"yyyy/MM/dd HH:mm:ss \G\M\T"),
                    this.RequestBody.ProductId,
                    this.RequestBody.SystemModel,
                    this.RequestBody.CoreSystemModel,
                    this.RequestBody.Hostname,
                    this.RequestBody.FQDN,
                    this.RequestBody.IPAddress,
                    this.RequestBody.Client_HPPID,
                    this.RequestBody.SAID ?? "",
                    this.RequestBody.MacAddress,
                    this.RequestBody.OSNameAndVersionString,
                    //--prs--
                    this.RequestBody.PRSAddress1,
                    this.RequestBody.PRSCity,
                    this.RequestBody.PRSRegion,
                    this.RequestBody.PRSPostalCode,
                    this.RequestBody.PRSTimeZone ?? "CDT",
                    this.RequestBody.PRSCountry,
                    //--ISEEEnt---
                    this.RequestBody.ISEEEntEntitlementType ?? "",
                    this.RequestBody.ISEEEntEntitlementId ?? "",
                    this.RequestBody.ISEEEntSerialNumber,
                    this.RequestBody.ISEEEntProductNumber,
                    this.RequestBody.ISEEEntProductId,
                    //--iseecus--
                    this.RequestBody.ISEECusBusiness ?? "Show Business",
                    this.RequestBody.ISEECusName,
                    //--iseeperson--
                    this.RequestBody.ISEEFirstPersonFirstName,
                    this.RequestBody.ISEEFirstPersonLastName,
                    this.RequestBody.ISEEFirstPersonEmailAddress,
                    this.RequestBody.ISEEFirstPersonTelephoneNumber,
                    this.RequestBody.ISEESecondPersonFirstName,
                    this.RequestBody.ISEESecondPersonLastName,
                    this.RequestBody.ISEESecondPersonEmailAddress,
                    this.RequestBody.ISEESecondPersonTelephoneNumber
                );
            }
        }
    }
}