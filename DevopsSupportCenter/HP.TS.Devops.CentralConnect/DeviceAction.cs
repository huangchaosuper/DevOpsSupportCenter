using HP.TS.Devops.Customer;
using HP.TS.Devops.Dictionary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace HP.TS.Devops.CentralConnect
{
    public class DeviceAction:HP.TS.Devops.Core.Action
    {
        public DeviceAction(string connectString)
            : base(connectString)
        { }

        public CentralConnectCode CreateDevice(Device device)
        {
            ClientAction clientAction = new ClientAction(this.ConnectString);
            List<Client> clients = clientAction.RetrieveClientByClientNameAndDomain(device.ClientName, device.ClientDomain);
            if (clients.Count != 1)
            {
                return CentralConnectCode.ClientNotFound;
            }
            if (Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_CreateCentralConnectDevice", device.DeviceName, device.Domain, device.ProductNumber, device.SerialNumber, device.CoreSystemModel, device.OSName, device.OSVersion, device.ClientName, device.ClientDomain, device.CreateBy) != 1)
            {
                return CentralConnectCode.CreateDeviceFail;
            }
            return CentralConnectCode.Success;
        }

        public List<Device> RetrieveDeviceByDeviceNameAndDomain(string deviceName, string domain)
        {
            DataSet dataSet = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(this.ConnectString, "SP_RetrieveCentralConnectDeviceByDeviceNameAndDomain", deviceName, domain);
            List<Device> devices = new List<Device>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Device device = new Device();
                device.DeviceName = item["DeviceName"] as string;
                device.Domain = item["Domain"] as string;
                device.ProductNumber = item["ProductNumber"] as string;
                device.SerialNumber = item["SerialNumber"] as string;
                device.CoreSystemModel = item["CoreSystemModel"] as string;
                device.MAC = item["MAC"] as string;
                device.OSName = item["OSName"] as string;
                device.OSVersion = item["OSVersion"] as string;
                device.IPAddress = HP.TS.Devops.Core.IPHelper.NumberToIP((int)item["IPAddress"]);
                device.ClientName = item["ClientName"] as string;
                device.ClientDomain = item["ClientDomain"] as string;
                device.CreateBy = item["CreateBy"] as string;
                devices.Add(device);
            }
            return devices;
        }

        public CentralConnectCode RegisterDevice(string environment, string deviceName, string domain, string createBy)
        {
            List<RegisterDeviceMessage> registerDeviceMessages = this.RetrieveRegisterDeviceMessageByEnvironmentAndDeviceNameAndDomain(environment, deviceName, domain);
            if (registerDeviceMessages.Count > 0)
            {
                return CentralConnectCode.RegisterDeviceMessageExist;
            }
            DictionaryAction dictionaryAction = new DictionaryAction(this.ConnectString);
            string serviceUrl = string.Format("{0}/{1}", dictionaryAction.RetrieveServiceUrl(environment), "OOSRegistration/OOSRegistrationService.svc");
            Logger.Write(string.Format("ServiceUrl={0},DeviceName={1},Domain={2}", serviceUrl, deviceName, domain));
            //Device
            List<Device> devices = this.RetrieveDeviceByDeviceNameAndDomain(deviceName, domain);
            if (devices.Count != 1)
            {
                return CentralConnectCode.DeviceNotFound;
            }
            Logger.Write("Retrieve Device", devices[0]);
            Device device = devices[0];
            //RegisterClientMessage
            ClientAction clientAction = new ClientAction(this.ConnectString);
            List<RegisterClientMessage> registerClientMessages = clientAction.RetrieveRegisterClientMessageByEnvironmentAndClientNameAndDomain(environment, device.ClientName, device.ClientDomain);
            if (registerClientMessages.Count != 1)
            {
                return CentralConnectCode.ClientNotRegister;
            }
            Logger.Write("RegisterClientMessage", registerClientMessages[0]);
            RegisterClientMessage registerClientMessage = registerClientMessages[0];
            //Client
            List<Client> clients = clientAction.RetrieveClientByClientNameAndDomain(device.ClientName, device.ClientDomain);
            if (clients.Count != 1)
            {
                return CentralConnectCode.ClientNotFound;
            }
            Logger.Write("Client", clients[0]);
            Client client = clients[0];
            //Contact
            ContactAction contactAction = new ContactAction(this.ConnectString);
            List<Contact> primaryContacts = contactAction.RetrieveContactByFullName(client.PrimaryContact);
            if (primaryContacts.Count != 1)
            {
                return CentralConnectCode.PrimaryContactNotFound;
            }
            Logger.Write("Retrieve PrimaryContacts", primaryContacts[0]);
            Contact primaryContact = primaryContacts[0];
            List<Contact> secondaryContacts = contactAction.RetrieveContactByFullName(client.SecondaryContact);
            if (secondaryContacts.Count != 1)
            {
                return CentralConnectCode.SecondaryContactNotFound;
            }
            Logger.Write("Retrieve SecondaryContact", secondaryContacts[0]);
            Contact secondaryContact = secondaryContacts[0];
            //Company
            CompanyAction companyAction = new CompanyAction(this.ConnectString);
            List<Company> companys = companyAction.RetrieveCompanyByCompanyName(client.CompanyName);
            if (companys.Count != 1)
            {
                return CentralConnectCode.CompanyNotFound;
            }
            Logger.Write("Retrieve Company", companys[0]);
            Company company = companys[0];
            //Request
            HP.TS.Devops.CentralConnect.OOSRegistration.RequestBody requestBody = new HP.TS.Devops.CentralConnect.OOSRegistration.RequestBody()
            {
                AutoDetectedSystemSerialNumber = device.SerialNumber,
                ProductId = device.ProductNumber,
                SystemModel = device.CoreSystemModel,
                CoreSystemModel = device.CoreSystemModel,
                Hostname = device.DeviceName,
                FQDN = string.Format("{0}.{1}", device.DeviceName, device.Domain),
                IPAddress = device.IPAddress,
                Client_HPPID = registerClientMessage.HPPID,
                MacAddress = device.MAC,
                OSNameAndVersionString = string.Format("{0},{1}", device.OSName, device.OSVersion),
                //--prs--
                PRSAddress1 = company.Street,
                PRSCity = company.City,
                PRSRegion = company.StateProvince,
                PRSPostalCode = company.PostalCode,
                PRSCountry = company.CountryCode,
                //--ISEEEnt---
                ISEEEntSerialNumber = client.SerialNumber,
                ISEEEntProductNumber = client.ProductNumber,
                ISEEEntProductId = client.ProductNumber,
                //--iseecus--
                ISEECusName = client.CompanyName,
                //--iseeperson--
                ISEEFirstPersonFirstName = primaryContact.FirstName,
                ISEEFirstPersonLastName = primaryContact.LastName,
                ISEEFirstPersonEmailAddress = primaryContact.EmailAddress,
                ISEEFirstPersonTelephoneNumber = primaryContact.Phone,

                ISEESecondPersonFirstName = secondaryContact.FirstName,
                ISEESecondPersonLastName = secondaryContact.LastName,
                ISEESecondPersonEmailAddress = secondaryContact.EmailAddress,
                ISEESecondPersonTelephoneNumber = secondaryContact.Phone,
            };
            Logger.Write("requestBody", requestBody);
            HP.TS.Devops.CentralConnect.OOSRegistration.OOSRegistrationServiceResult oosRegistrationServiceResult = this.RegisterDeviceRequest(serviceUrl, requestBody, registerClientMessage.CSID, registerClientMessage.OSGDID, registerClientMessage.RegistrationToken);
            if (!oosRegistrationServiceResult.IsSuccess)
            {
                Logger.Write("OOSRegistrationServiceResult Fail", oosRegistrationServiceResult);
                if (oosRegistrationServiceResult.Error.IsClientError)
                {
                    return CentralConnectCode.RegisterDeviceClientError;
                }
                else if (oosRegistrationServiceResult.Error.IsReRegisterError)
                {
                    return CentralConnectCode.RegisterDeviceReRegisterError;
                }
                else if (oosRegistrationServiceResult.Error.IsTransientError)
                {
                    return CentralConnectCode.RegisterDeviceTransientError;
                }
            }
            RegisterDeviceMessage registerDeviceMessage = new RegisterDeviceMessage();
            registerDeviceMessage.Environment = environment;
            registerDeviceMessage.DeviceName = deviceName;
            registerDeviceMessage.Domain = domain;
            registerDeviceMessage.OSGDID = oosRegistrationServiceResult.Gdid;
            registerDeviceMessage.OSGDIDUsn = oosRegistrationServiceResult.OSGdidUsn.ToString();
            registerDeviceMessage.HWGDID = oosRegistrationServiceResult.HWGdid;
            registerDeviceMessage.HWGDIDUsn = oosRegistrationServiceResult.HWGdidUsn.ToString();
            registerDeviceMessage.CreateBy = createBy;

            if (this.AddRegisterDeviceMessage(registerDeviceMessage) != 1)
            {
                return CentralConnectCode.AddRegisterDeviceMessageFail;
            }
            return CentralConnectCode.Success;
        }
        private int AddRegisterDeviceMessage(RegisterDeviceMessage registerDeviceMessage)
        {
            this.Logger.Write("Add RegisterDeviceMessage", registerDeviceMessage);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_AddCentralConnectRegisterDeviceMessage",
                registerDeviceMessage.Environment,
                registerDeviceMessage.DeviceName,
                registerDeviceMessage.Domain,
                registerDeviceMessage.OSGDID,
                registerDeviceMessage.OSGDIDUsn,
                registerDeviceMessage.HWGDID,
                registerDeviceMessage.HWGDIDUsn,
                registerDeviceMessage.CreateBy);
        }
        private HP.TS.Devops.CentralConnect.OOSRegistration.OOSRegistrationServiceResult RegisterDeviceRequest(string serviceUrl, HP.TS.Devops.CentralConnect.OOSRegistration.RequestBody requestBody, string clientCSID, string clientOSGDID, string clientRegistrationToken)
        {
            WSHttpBinding wsb = new WSHttpBinding(SecurityMode.Transport);
            wsb.SendTimeout = new TimeSpan(10, 10, 10);
            wsb.ReceiveTimeout = new TimeSpan(10, 10, 10);
            wsb.OpenTimeout = new TimeSpan(10, 10, 10);
            wsb.CloseTimeout = new TimeSpan(10, 10, 10);
            wsb.MaxBufferPoolSize = 500000000;
            wsb.MaxReceivedMessageSize = 500000000;
            HP.TS.Devops.CentralConnect.OOSRegistration.OOSRegistrationServiceClient oosRegistrationServiceClient = new HP.TS.Devops.CentralConnect.OOSRegistration.OOSRegistrationServiceClient(wsb, new EndpointAddress(serviceUrl));
            HP.TS.Devops.CentralConnect.OOSRegistration.OOSRegistrationRequest oosRegistrationRequest = new HP.TS.Devops.CentralConnect.OOSRegistration.OOSRegistrationRequest(requestBody);
            string request = oosRegistrationRequest.RequestString;
            Logger.Write("oosRegistrationRequest.RequestString-" + request);
            HP.TS.Devops.CentralConnect.OOSRegistration.OOSRegistrationServiceResult oosRegistrationServiceResult = oosRegistrationServiceClient.Register(new HP.TS.Devops.CentralConnect.OOSRegistration.IseeWebServicesHeader()
                {
                    CSID = clientCSID, // send web request with CSID of Device not client
                    GDID = clientOSGDID, // OSGDID from client
                    registrationToken = clientRegistrationToken  //client registration token
                },
                  request  //whole template xml
            );
            Logger.Write("OOSRegistrationServiceResult", request);
            return oosRegistrationServiceResult;
        }
        public List<RegisterDeviceMessage> RetrieveRegisterDeviceMessageByEnvironmentAndDeviceNameAndDomain(string environment, string deviceName, string domain)
        {
            DataSet dataSet = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(this.ConnectString, "SP_RetrieveCentralConnectRegisterDeviceMessageByEnvironmentAndDeviceNameAndDomain", environment, deviceName, domain);
            List<RegisterDeviceMessage> registerDeviceMessages = new List<RegisterDeviceMessage>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                RegisterDeviceMessage registerDeviceMessage = new RegisterDeviceMessage();
                registerDeviceMessage.Environment = item["Environment"] as string;
                registerDeviceMessage.DeviceName = item["DeviceName"] as string;
                registerDeviceMessage.Domain = item["Domain"] as string;
                registerDeviceMessage.OSGDID = item["OSGDID"] as string;
                registerDeviceMessage.OSGDIDUsn = item["OSGDIDUsn"] as string;
                registerDeviceMessage.HWGDID = item["HWGDID"] as string;
                registerDeviceMessage.HWGDIDUsn = item["HWGDIDUsn"] as string;
                registerDeviceMessage.CollectionProblem = item["CollectionProblem"] as string;
                registerDeviceMessage.MonitoringProblem = item["MonitoringProblem"] as string;
                registerDeviceMessage.CreateBy = item["CreateBy"] as string;
                registerDeviceMessages.Add(registerDeviceMessage);
            }
            return registerDeviceMessages;
        }

        public int SendEvent()
        {
            throw new NotImplementedException();
        }

        public int SendCollection()
        {
            throw new NotImplementedException();
        }
    }
}
