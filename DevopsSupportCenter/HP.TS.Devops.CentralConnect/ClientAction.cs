using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using HP.TS.Devops.Customer;
using System.Web;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Data;
using HP.TS.Devops.Dictionary;
using HP.TS.Devops.CentralConnect.Plugin;
using System.IO;

namespace HP.TS.Devops.CentralConnect
{
    public class ClientAction:HP.TS.Devops.Core.Action
    {
        public ClientAction(string connectString)
            : base(connectString)
        { }
        public CentralConnectCode CreateClient(Client client)
        {
            //Contact
            ContactAction contactAction = new ContactAction(this.ConnectString);
            List<Contact> primaryContacts = contactAction.RetrieveContactByFullName(client.PrimaryContact);
            if (primaryContacts.Count != 1)
            {
                return CentralConnectCode.PrimaryContactNotFound;
            }
            List<Contact> secondaryContacts = contactAction.RetrieveContactByFullName(client.SecondaryContact);
            if (secondaryContacts.Count != 1)
            {
                return CentralConnectCode.SecondaryContactNotFound;
            }
            //Company
            CompanyAction companyAction = new CompanyAction(this.ConnectString);
            List<Company> companys = companyAction.RetrieveCompanyByCompanyName(client.CompanyName);
            if (companys.Count != 1)
            {
                return CentralConnectCode.CompanyNotFound;
            }
            if (Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_CreateCentralConnectClient", client.ClientName, client.Domain, client.ProductNumber, client.SerialNumber, client.CoreSystemModel, client.OSName, client.OSVersion, client.CompanyName, client.PrimaryContact, client.SecondaryContact, client.CreateBy) != 1)
            {
                return CentralConnectCode.CreateClientFail;
            }
            return CentralConnectCode.Success;
        }

        public List<Client> RetrieveClientByClientNameAndDomain(string clientName, string domain)
        {
            DataSet dataSet = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(this.ConnectString, "SP_RetrieveCentralConnectClientByClientNameAndDomain", clientName, domain);
            List<Client> clients = new List<Client>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Client client = new Client();
                client.ClientName = item["ClientName"] as string;
                client.Domain = item["Domain"] as string;
                client.ProductNumber = item["ProductNumber"] as string;
                client.SerialNumber = item["SerialNumber"] as string;
                client.CoreSystemModel = item["CoreSystemModel"] as string;
                client.MAC = item["MAC"] as string;
                client.OSName = item["OSName"] as string;
                client.OSVersion = item["OSVersion"] as string;
                client.IPAddress = HP.TS.Devops.Core.IPHelper.NumberToIP((int)item["IPAddress"]);
                client.CompanyName = item["CompanyName"] as string;
                client.PrimaryContact = item["PrimaryContact"] as string;
                client.SecondaryContact = item["SecondaryContact"] as string;
                client.CreateBy = item["CreateBy"] as string;
                clients.Add(client);
            }
            return clients;
        }

        public CentralConnectCode RegisterClient(string environment,string clientName,string domain,string createBy)
        {
            List<RegisterClientMessage> registerClientMessages = this.RetrieveRegisterClientMessageByEnvironmentAndClientNameAndDomain(environment, clientName, domain);
            if (registerClientMessages.Count > 0)
            {
                return CentralConnectCode.RegisterClientMessageExist;
            }
            DictionaryAction dictionaryAction = new DictionaryAction(this.ConnectString);
            string serviceUrl = string.Format("{0}/{1}", dictionaryAction.RetrieveServiceUrl(environment), "ClientRegistration/ClientRegistrationService.asmx");
            Logger.Write(string.Format("ServiceUrl={0},ClientName={1},Domain={2}", serviceUrl, clientName, domain));
            //Client
            List<Client> clients = this.RetrieveClientByClientNameAndDomain(clientName, domain);
            if (clients.Count != 1)
            {
                return CentralConnectCode.ClientNotFound;
            }
            Logger.Write("Retrieve Client", clients[0]);
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
            HP.TS.Devops.CentralConnect.ClientRegistration.RequestBody requestBody = new HP.TS.Devops.CentralConnect.ClientRegistration.RequestBody()
            {
                CoreSystemModel = client.CoreSystemModel,
                ProductId = client.ProductNumber,
                FQDN = string.Format("{0}.{1}", client.ClientName, client.Domain),
                Hostname = client.ClientName,
                OSNameAndVersionString = string.Format("{0},{1}", client.OSName, client.OSVersion),
                SystemModel = client.CoreSystemModel,
                MacAddress = client.MAC,
                IPAddress = client.IPAddress,
                PRSCaption = client.CompanyName,
                PRSDirtyFlag = 1,
                PRSUniqueName = client.CompanyName,
                PRSAddress1 = company.Street,
                PRSCity = company.City,
                PRSRegion = company.StateProvince,
                PRSPostalCode = company.PostalCode,
                PRSCountry = company.CountryCode,
                ISEECusCaption = client.CompanyName,
                ISEECusPRSUniqueName =client.CompanyName,
                ISEECusName = client.CompanyName,
                ISEEEntSerialNumber = client.SerialNumber,
                ISEEEntProductNumber = client.ProductNumber,
                ISEEEntProductId = client.ProductNumber,
                
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
            string xmlEscapedCSID = string.Empty;
            HP.TS.Devops.CentralConnect.ClientRegistration.ClientRegistrationResult result = this.RegisterClientRequest(serviceUrl, requestBody, out xmlEscapedCSID);
            Logger.Write("ClientRegistration Result", result);
            //load to database
            if (!result.IsSuccess)
            {
                return CentralConnectCode.ClientRegistrationFail;
            }
            RegisterClientMessage registerClientMessage = new RegisterClientMessage();
            registerClientMessage.Environment = environment;
            registerClientMessage.ClientName = clientName;
            registerClientMessage.Domain = domain;
            registerClientMessage.CSID = xmlEscapedCSID;
            registerClientMessage.OSGDID = result.Gdid;
            registerClientMessage.RegistrationToken = result.RegistrationToken;
            registerClientMessage.HWGDID = result.Gdid;
            registerClientMessage.CreateBy = createBy;

            if (this.AddRegisterClientMessage(registerClientMessage) != 1)
            {
                return CentralConnectCode.AddRegisterClientMessageFail;
            }
            return CentralConnectCode.Success;
        }
        private int AddRegisterClientMessage(RegisterClientMessage registerClientMessage)
        {
            this.Logger.Write("Add RegisterClientMessage", registerClientMessage);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_AddCentralConnectRegisterClientMessage",
                registerClientMessage.Environment,
                registerClientMessage.ClientName,
                registerClientMessage.Domain,
                registerClientMessage.HPPID,
                registerClientMessage.Password,
                registerClientMessage.CSID,
                registerClientMessage.OSGDID,
                registerClientMessage.HWGDID,
                registerClientMessage.RegistrationToken,
                registerClientMessage.CreateBy);
        }
        public List<RegisterClientMessage> RetrieveRegisterClientMessageByEnvironmentAndClientNameAndDomain(string environment,string clientName, string domain)
        {
            DataSet dataSet = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(this.ConnectString, "SP_RetrieveCentralConnectRegisterClientMessageByEnvironmentAndClientNameAndDomain",environment, clientName, domain);
            List<RegisterClientMessage> registerClientMessages = new List<RegisterClientMessage>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                RegisterClientMessage registerClientMessage = new RegisterClientMessage();
                registerClientMessage.Environment = item["Environment"] as string;
                registerClientMessage.ClientName = item["ClientName"] as string;
                registerClientMessage.Domain = item["Domain"] as string;
                registerClientMessage.HPPID = item["HPPID"] as string;
                registerClientMessage.Password = item["Password"] as string;
                registerClientMessage.CSID = item["CSID"] as string;
                registerClientMessage.OSGDID = item["OSGDID"] as string;
                registerClientMessage.HWGDID = item["HWGDID"] as string;
                registerClientMessage.RegistrationToken = item["RegistrationToken"] as string;
                registerClientMessage.Visible = (bool)item["Visible"];
                registerClientMessage.CreateBy = item["CreateBy"] as string;
                registerClientMessages.Add(registerClientMessage);
            }
            return registerClientMessages;
        }
        public CentralConnectCode VisibleClient(string environment, string clientName, string domain,string hppid,string password)
        {
            List<RegisterClientMessage> registerClientMessages = this.RetrieveRegisterClientMessageByEnvironmentAndClientNameAndDomain(environment, clientName, domain);
            if (registerClientMessages.Count != 1)
            {
                return CentralConnectCode.RegisterClientMessageNotExist;
            }
            Logger.Write("Retrieve registerClientMessages", registerClientMessages[0]);
            RegisterClientMessage registerClientMessage = registerClientMessages[0];
            registerClientMessage.HPPID = hppid;
            registerClientMessage.Password = password;
            DictionaryAction dictionaryAction = new DictionaryAction(this.ConnectString);
            string serviceUrl = string.Format("{0}/{1}", dictionaryAction.RetrieveServiceUrl(environment), "RemoteSupportServices/RemoteSupportServices.svc");
            Logger.Write(string.Format("ServiceUrl={0},ClientName={1},Domain={2}", serviceUrl, clientName, domain));
            if (this.VisibleClientRequest(serviceUrl, registerClientMessage) != 0)
            {
                return CentralConnectCode.VisibleClientFail;
            }
            if (UpdateCentralConnectRegisterClientMessageHPPIDAndPassowrdByEnvironmentAndClientNameAndDomain(environment, clientName, domain, hppid, password) != 1)
            {
                return CentralConnectCode.UpdateHPPIDAndPasswordFail;
            }
            return CentralConnectCode.Success;
        }
        private int UpdateCentralConnectRegisterClientMessageHPPIDAndPassowrdByEnvironmentAndClientNameAndDomain(string environment, string clientName, string domain, string hppid, string password)
        {
            this.Logger.Write(string.Format("Update RegisterClientMessage Environment={0} ClientName={1} Domain={2} HPPID={3} Password={4}", environment, clientName, domain, hppid, password));
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_UpdateCentralConnectRegisterClientMessageHPPIDAndPassowrdByEnvironmentAndClientNameAndDomain",
                environment,
                clientName,
                domain,
                hppid,
                password);
        }
        public CentralConnectCode InvisibleClient(string environment, string clientName, string domain)
        {
            List<RegisterClientMessage> registerClientMessages = this.RetrieveRegisterClientMessageByEnvironmentAndClientNameAndDomain(environment, clientName, domain);
            if (registerClientMessages.Count != 1)
            {
                return CentralConnectCode.RegisterClientMessageNotExist;
            }
            Logger.Write("Retrieve registerClientMessages", registerClientMessages[0]);
            RegisterClientMessage registerClientMessage = registerClientMessages[0];
            DictionaryAction dictionaryAction = new DictionaryAction(this.ConnectString);
            string serviceUrl = string.Format("{0}/{1}", dictionaryAction.RetrieveServiceUrl(environment), "RemoteSupportServices/RemoteSupportServices.svc");
            Logger.Write(string.Format("ServiceUrl={0},ClientName={1},Domain={2}", serviceUrl, clientName, domain));
            if (this.InVisibleClientRequest(serviceUrl, registerClientMessage) != 0)
            {
                return CentralConnectCode.InvisibleClientFail;
            }
            return CentralConnectCode.Success;
        }
        public CentralConnectCode CreateAndSendMetrics(MetricsEnhancement metricsEnhancement)
        {
            //Client
            ClientAction clientAction = new ClientAction(this.ConnectString);
            List<Client> clients = clientAction.RetrieveClientByClientNameAndDomain(metricsEnhancement.ClientName, metricsEnhancement.Domain);
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
            //registerClientMessages
            List<RegisterClientMessage> registerClientMessages = this.RetrieveRegisterClientMessageByEnvironmentAndClientNameAndDomain(metricsEnhancement.Environment, metricsEnhancement.ClientName, metricsEnhancement.Domain);
            if (registerClientMessages.Count != 1)
            {
                return CentralConnectCode.RegisterClientMessageNotExist;
            }
            Logger.Write("Retrieve registerClientMessages", registerClientMessages[0]);
            RegisterClientMessage registerClientMessage = registerClientMessages[0];
            //devices
            List<Device> devices = this.RetrieveRegisterDeviceByEnvironmentAndClientNameAndClientDomain(metricsEnhancement.Environment, metricsEnhancement.ClientName, metricsEnhancement.Domain);
            if (devices.Count == 0)
            {
                return CentralConnectCode.ClientNotFoundAssociatedDevice;
            }
            Logger.Write("Retrieve devices", devices);

            MetricsRequest metricsRequest = new MetricsRequest();
            //ClientInformation
            ClientInformation clientInformation = new ClientInformation();
            clientInformation.ClientName = metricsEnhancement.ClientName;
            clientInformation.Domain = metricsEnhancement.Domain;
            clientInformation.ProductNumber = client.ProductNumber;
            clientInformation.SerialNumber = client.SerialNumber;
            clientInformation.CoreSystemModel = client.CoreSystemModel;
            clientInformation.MAC = client.MAC;
            clientInformation.OSName = client.OSName;
            clientInformation.OSVersion = client.OSVersion;
            clientInformation.IPAddress = client.IPAddress;
            clientInformation.HPPID = registerClientMessage.HPPID;
            clientInformation.Password = registerClientMessage.Password;
            clientInformation.CSID = registerClientMessage.CSID;
            clientInformation.OSGDID = registerClientMessage.OSGDID;
            clientInformation.HWGDID = registerClientMessage.HWGDID;
            clientInformation.RegistrationToken = registerClientMessage.RegistrationToken;
            clientInformation.CompanyName = client.CompanyName;
            clientInformation.CountryCode = company.CountryCode;
            clientInformation.StateProvince = company.StateProvince;
            clientInformation.City = company.City;
            clientInformation.Street = company.Street;
            clientInformation.PostalCode = company.PostalCode;
            clientInformation.PrimaryContact = client.PrimaryContact;
            clientInformation.PrimaryFirstName = primaryContact.FirstName;
            clientInformation.PrimaryLastName = primaryContact.LastName;
            clientInformation.PrimaryEmailAddress = primaryContact.EmailAddress;
            clientInformation.PrimaryPhone = primaryContact.Phone;
            clientInformation.SecondaryContact = client.SecondaryContact;
            clientInformation.SecondaryFirstName = secondaryContact.FirstName;
            clientInformation.SecondaryLastName = secondaryContact.LastName;
            clientInformation.SecondaryEmailAddress = secondaryContact.EmailAddress;
            clientInformation.SecondaryPhone = secondaryContact.Phone;
            clientInformation.SupportPartnerId = metricsEnhancement.SupportPartnerId;
            clientInformation.ServicePartnerId = metricsEnhancement.ServicePartnerId;
            metricsRequest.ClientInformation = clientInformation;
            //deviceinformation
            DeviceAction deviceAction = new DeviceAction(this.ConnectString);
            List<DeviceInformation> deviceInformations = new List<DeviceInformation>();
            foreach (Device item in devices)
            {
                DeviceInformation deviceInformation = new DeviceInformation();
                deviceInformation.ClientName = client.ClientName;
                deviceInformation.ClientDomain = client.Domain;
                deviceInformation.DeviceName = item.DeviceName;
                deviceInformation.Domain = item.Domain;
                deviceInformation.ProductNumber = item.ProductNumber;
                deviceInformation.SerialNumber = item.SerialNumber;
                deviceInformation.CoreSystemModel = item.CoreSystemModel;
                deviceInformation.MAC = item.MAC;
                deviceInformation.OSName = item.OSName;
                deviceInformation.OSVersion = item.OSVersion;
                deviceInformation.IPAddress = item.IPAddress;
                List<RegisterDeviceMessage> registerDeviceMessages = deviceAction.RetrieveRegisterDeviceMessageByEnvironmentAndDeviceNameAndDomain(metricsEnhancement.Environment, item.DeviceName, item.Domain);
                if (registerDeviceMessages.Count != 1)
                {
                    return CentralConnectCode.RegisterDeviceMessageNotExist;
                }
                RegisterDeviceMessage registerDeviceMessage = registerDeviceMessages[0];
                deviceInformation.OSGDID = registerDeviceMessage.OSGDID;
                deviceInformation.OSGDIDUsn = registerDeviceMessage.OSGDIDUsn;
                deviceInformation.HWGDID = registerDeviceMessage.HWGDID;
                deviceInformation.HWGDIDUsn = registerDeviceMessage.HWGDIDUsn;
                deviceInformation.CollectionProblem = registerDeviceMessage.CollectionProblem;
                deviceInformation.MonitoringProblem = registerDeviceMessage.MonitoringProblem;
                deviceInformations.Add(deviceInformation);
            }
            metricsRequest.DeviceInformations = deviceInformations;
            //enhancementArguments
            Dictionary<string, string> enhancementArguments = new Dictionary<string, string>();
            if (metricsEnhancement.Arguments != null)
            {
                foreach (Argument item in metricsEnhancement.Arguments)
                {
                    if (!enhancementArguments.ContainsKey(item.key))
                    {
                        enhancementArguments.Add(item.key, item.value);
                    }
                }
            }
            metricsRequest.EnhancementArguments = enhancementArguments;
            //InvokeMetrics Plugin
            EnhancementAction enhancementAction = new EnhancementAction(this.ConnectString);
            PluginResponse result = enhancementAction.InvokeMetrics(metricsRequest,metricsEnhancement.Type);
            if (!result.IsSuccess)
            {
                Logger.Write("CreateMetricsFail-" + result.Message);
                return CentralConnectCode.CreateMetricsFail;
            }
            //SendMetrics
            DictionaryAction dictionaryAction = new DictionaryAction(this.ConnectString);
            string serviceUrl = string.Format("{0}/{1}", dictionaryAction.RetrieveServiceUrl(metricsEnhancement.Environment), "DataPackageReceiver/DataPackageReceiver.asmx");
            DataPackageSubmissionResult dataPackageSubmissionResult =this.SendMetricsRequest(serviceUrl, 
                registerClientMessage.CSID, 
                registerClientMessage.HWGDID,
                registerClientMessage.OSGDID, 
                registerClientMessage.RegistrationToken, 
                result.FileName, 
                result.FileContent);
            if (!dataPackageSubmissionResult.IsSuccess)
            {
                if (dataPackageSubmissionResult.Error.IsClientError)
                {
                    return CentralConnectCode.SendMetricsClientError;
                }
                else if (dataPackageSubmissionResult.Error.IsReRegisterError)
                {
                    return CentralConnectCode.SendMetricsReRegisterError;
                }
                else if (dataPackageSubmissionResult.Error.IsTransientError)
                {
                    return CentralConnectCode.SendMetricsTransientError;
                }
            }
            MetricsMessage metricsMessage = new MetricsMessage();
            metricsMessage.Environment = metricsEnhancement.Environment;
            metricsMessage.ClientName = metricsEnhancement.ClientName;
            metricsMessage.Domain = metricsEnhancement.Domain;
            metricsMessage.PackageName = result.FileName;
            metricsMessage.ServicePartnerId = metricsEnhancement.ServicePartnerId;
            metricsMessage.SupportPartnerId = metricsEnhancement.SupportPartnerId;
            metricsMessage.CreateBy = "TBD";

            if (this.AddMetricsMessage(metricsMessage) != 1)
            {
                return CentralConnectCode.AddMetricsMessageFail;
            }
            return CentralConnectCode.Success;
        }

        private int AddMetricsMessage(MetricsMessage metricsMessage)
        {
            this.Logger.Write("Add MetricsMessage", metricsMessage);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_AddCentralConnectMetricsMessage",
                metricsMessage.Environment,
                metricsMessage.ClientName,
                metricsMessage.Domain,
                metricsMessage.PackageName,
                metricsMessage.SupportPartnerId,
                metricsMessage.ServicePartnerId,
                metricsMessage.CreateBy);
        }

        public List<Device> RetrieveRegisterDeviceByEnvironmentAndClientNameAndClientDomain(string environment, string clientName, string domain)
        {
            DataSet dataSet = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(this.ConnectString, "SP_RetrieveCentralConnectRegisterDeviceByEnvironmentAndClientNameAndClientDomain", environment,clientName, domain);
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
        private int InVisibleClientRequest(string serviceUrl, RegisterClientMessage registerClientMessage)
        {
            string KEY = "a6iwIJKu";
            string IVector = "a6iwIJKu";
            string encryptedpassword = descrypto(registerClientMessage.Password, KEY, IVector);

            RemoteSupportServicesClient RSSC = new RemoteSupportServicesClient(new WSHttpBinding(SecurityMode.Transport),
                      new EndpointAddress(serviceUrl));

            www.hp.com.isee.webservices.ServiceError serviceError;
            string gdid;
            int boMultiplier;
            bool rtn = RSSC.DisableVisibility(new www.hp.com.isee.webservices.IseeWebServicesHeader()
                {
                    CSID = registerClientMessage.CSID,
                    GDID = registerClientMessage.OSGDID,
                    OSID = "",
                    registrationToken = registerClientMessage.RegistrationToken
                }, encryptedpassword, registerClientMessage.HPPID, out serviceError, out gdid, out boMultiplier);
            this.Logger.Write(string.Format("EnableVisibility HPPID={0} Result={1} ServiceError={2} gdid={3} boMultiplier={4}", registerClientMessage.HPPID, rtn, serviceError, gdid, boMultiplier));
            if (!rtn)
            {
                return -1;
            }
            return 0;
        }
        private int VisibleClientRequest(string serviceUrl, RegisterClientMessage registerClientMessage)
        {
            string KEY = "a6iwIJKu";
            string IVector = "a6iwIJKu";
            string encryptedpassword = descrypto(registerClientMessage.Password, KEY, IVector);

            RemoteSupportServicesClient RSSC = new RemoteSupportServicesClient(new WSHttpBinding(SecurityMode.Transport),
                      new EndpointAddress(serviceUrl));

            www.hp.com.isee.webservices.ServiceError serviceError;
            string gdid;
            int boMultiplier;
            bool rtn = RSSC.EnableVisibility(new www.hp.com.isee.webservices.IseeWebServicesHeader()
                {
                    CSID = registerClientMessage.CSID,
                    GDID = registerClientMessage.OSGDID,
                    OSID = "",
                    registrationToken = registerClientMessage.RegistrationToken
                }, encryptedpassword, registerClientMessage.HPPID, out serviceError, out gdid, out boMultiplier);
            this.Logger.Write(string.Format("EnableVisibility HPPID={0} Result={1} ServiceError={2} gdid={3} boMultiplier={4}", registerClientMessage.HPPID, rtn, serviceError, gdid, boMultiplier));
            if (!rtn)
            {
                return -1;
            }
            return 0;
        }
        private string descrypto(string Password, string Key, string IV)
        {
            DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();

            //cryptic.Key = ASCIIEncoding.ASCII.GetBytes("a6iwIJKu");
            //cryptic.IV = ASCIIEncoding.ASCII.GetBytes("a6iwIJKu");
            //byte[] data = ASCIIEncoding.ASCII.GetBytes("Pwd12345");

            cryptic.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            cryptic.IV = ASCIIEncoding.ASCII.GetBytes(IV);
            byte[] data = ASCIIEncoding.ASCII.GetBytes(Password);

            cryptic.Mode = CipherMode.ECB;
            ICryptoTransform encptor = cryptic.CreateEncryptor();
            byte[] encryptedbytes = encptor.TransformFinalBlock(data, 0, data.Length);
            string outputxy = Convert.ToBase64String(encryptedbytes);
            return outputxy;
        }
        private HP.TS.Devops.CentralConnect.ClientRegistration.ClientRegistrationResult RegisterClientRequest(string serviceUrl, HP.TS.Devops.CentralConnect.ClientRegistration.RequestBody requestBody, out string xmlEscapedCSID)
        {
            HP.TS.Devops.CentralConnect.ClientRegistration.ClientRegistrationService clientRegistrationService = new HP.TS.Devops.CentralConnect.ClientRegistration.ClientRegistrationService(serviceUrl);
            HP.TS.Devops.CentralConnect.ClientRegistration.ClientRegistrationRequest clientRegistrationRequest = new HP.TS.Devops.CentralConnect.ClientRegistration.ClientRegistrationRequest(requestBody);
            string requestString = clientRegistrationRequest.RequestString;
            requestString = requestString.Replace("&", @"&amp;");
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(requestString);
            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            xmlnsManager.AddNamespace("isee", "http://www.hp.com/schemas/isee/5.00/event");
            XmlNode csidNode = xmlDoc.SelectSingleNode("/isee:ISEE-Registration/RegistrationSource/HP_OOSIdentifiers/CSID", xmlnsManager);
            xmlEscapedCSID = csidNode.OuterXml;
            Logger.Write("RegisterClientRequest xmlDoc.OuterXml-" + xmlDoc.OuterXml);
            return clientRegistrationService.RegisterClient2(xmlDoc.InnerXml);
        }

        private DataPackageSubmissionResult SendMetricsRequest(string serviceUrl,string csid,string hwGdid,string osGuid,string registrationToken,string fileName,byte[] attachment)
        {
            DataPackageReceiverSoapClient dprs = new DataPackageReceiverSoapClient(new BasicHttpBinding("DataPackageReceiverSoap"), new EndpointAddress(serviceUrl));
            return dprs.SubmitDataPackage(new IseeWebServicesHeader() { 
                CSID = csid,
                GDID = hwGdid,
                OSID = osGuid,
                registrationToken = registrationToken
            }, fileName, attachment);
        }
    }
}
