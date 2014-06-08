using HP.TS.Devops.CentralConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HP.TS.Devops.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CentralConnectService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CentralConnectService.svc or CentralConnectService.svc.cs at the Solution Explorer and start debugging.
    public class CentralConnectService : ICentralConnectService
    {
        private readonly string ConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["connectString"].ConnectionString;

        Core.ServiceResponse ICentralConnectService.CreateClient(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.Client == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Client should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Client.ClientName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.Domain)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.ProductNumber)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.SerialNumber)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.CoreSystemModel)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.OSName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.OSVersion)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.CompanyName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.PrimaryContact)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.SecondaryContact)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.CreateBy))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "ClientName Domain ProductNumber SerialNumber CoreSystemModel OSName OSVersion CompanyName PrimaryContact SecondaryContact CreateBy should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }

            ClientAction clientAction = new ClientAction(this.ConnectString);
            CentralConnectCode centralConnectCode = clientAction.CreateClient(centralConnectServiceRequest.Client);
            if (CentralConnectCode.Success != centralConnectCode)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "CreateClient Fail of " + Enum.GetName(typeof(CentralConnectCode), centralConnectCode) };
            }

            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }

        CentralConnectServiceResponse ICentralConnectService.RetrieveClientByClientNameAndDomain(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.Client == null)
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "Client should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Client.ClientName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Client.Domain))
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "ClientName Domain should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new CentralConnectServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }

            ClientAction clientAction = new ClientAction(this.ConnectString);
            List<Client> clients = clientAction.RetrieveClientByClientNameAndDomain(centralConnectServiceRequest.Client.ClientName, centralConnectServiceRequest.Client.Domain);
            return new CentralConnectServiceResponse() { Code = 0, Message = "Success", Clients = clients };
        }

        Core.ServiceResponse ICentralConnectService.RegisterClient(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.RegisterClientMessage == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "RegisterClientMessage should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.Environment)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.ClientName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.Domain)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.CreateBy))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Environment ClientName Domain CreateBy should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }
            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }
            ClientAction clientAction = new ClientAction(this.ConnectString);
            CentralConnectCode centralConnectCode = clientAction.RegisterClient(centralConnectServiceRequest.RegisterClientMessage.Environment, 
                centralConnectServiceRequest.RegisterClientMessage.ClientName, 
                centralConnectServiceRequest.RegisterClientMessage.Domain,
                centralConnectServiceRequest.RegisterClientMessage.CreateBy);
            if (CentralConnectCode.Success != centralConnectCode)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "RegisterClient Fail of " + Enum.GetName(typeof(CentralConnectCode), centralConnectCode) };
            }
            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }

        CentralConnectServiceResponse ICentralConnectService.RetrieveRegisterClientMessageByEnvironmentAndClientNameAndDomain(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.RegisterClientMessage == null)
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "RegisterClientMessage should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.Environment)
                ||string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.ClientName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.Domain))
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "Environment ClientName Domain should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new CentralConnectServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }

            ClientAction clientAction = new ClientAction(this.ConnectString);
            List<RegisterClientMessage> registerClientMessages = clientAction.RetrieveRegisterClientMessageByEnvironmentAndClientNameAndDomain(centralConnectServiceRequest.RegisterClientMessage.Environment, centralConnectServiceRequest.RegisterClientMessage.ClientName, centralConnectServiceRequest.RegisterClientMessage.Domain);
            return new CentralConnectServiceResponse() { Code = 0, Message = "Success", RegisterClientMessages = registerClientMessages };
        }

        Core.ServiceResponse ICentralConnectService.VisibleClient(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.RegisterClientMessage == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "RegisterClientMessage should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.Environment)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.ClientName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.Domain)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.HPPID)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.Password))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Environment ClientName Domain HPPID Password should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }
            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }
            ClientAction clientAction = new ClientAction(this.ConnectString);
            CentralConnectCode clientCode = clientAction.VisibleClient(centralConnectServiceRequest.RegisterClientMessage.Environment, 
                centralConnectServiceRequest.RegisterClientMessage.ClientName, 
                centralConnectServiceRequest.RegisterClientMessage.Domain,
                centralConnectServiceRequest.RegisterClientMessage.HPPID,
                centralConnectServiceRequest.RegisterClientMessage.Password);
            if (CentralConnectCode.Success != clientCode)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "RegisterClient Fail of " + Enum.GetName(typeof(CentralConnectCode), clientCode) };
            }
            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }

        Core.ServiceResponse ICentralConnectService.SendClientMetrics(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.MetricsEnhancement == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "MetricsEnhancement should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.MetricsEnhancement.Environment)
                || string.IsNullOrEmpty(centralConnectServiceRequest.MetricsEnhancement.ClientName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.MetricsEnhancement.Domain)
                || string.IsNullOrEmpty(centralConnectServiceRequest.MetricsEnhancement.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Environment ClientName Domain Type should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }
            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }
            ClientAction clientAction = new ClientAction(this.ConnectString);
            CentralConnectCode clientCode = clientAction.CreateAndSendMetrics(centralConnectServiceRequest.MetricsEnhancement);
            if (CentralConnectCode.Success != clientCode)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "CreateAndSendMetrics Fail of " + Enum.GetName(typeof(CentralConnectCode), clientCode) };
            }
            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }

        Core.ServiceResponse ICentralConnectService.InvisibleClient(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.RegisterClientMessage == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "RegisterClientMessage should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.Environment)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.ClientName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterClientMessage.Domain))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Environment ClientName Domain should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }
            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }
            ClientAction clientAction = new ClientAction(this.ConnectString);
            CentralConnectCode clientCode = clientAction.InvisibleClient(centralConnectServiceRequest.RegisterClientMessage.Environment,
                centralConnectServiceRequest.RegisterClientMessage.ClientName,
                centralConnectServiceRequest.RegisterClientMessage.Domain);
            if (CentralConnectCode.Success != clientCode)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "RegisterClient Fail of " + Enum.GetName(typeof(CentralConnectCode), clientCode) };
            }
            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }

        Core.ServiceResponse ICentralConnectService.CreateDevice(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.Device == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Device should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Device.DeviceName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Device.Domain)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Device.ProductNumber)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Device.SerialNumber)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Device.CoreSystemModel)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Device.OSName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Device.OSVersion)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Device.ClientName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Device.ClientDomain)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Device.CreateBy))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "DeviceName Domain ProductNumber SerialNumber CoreSystemModel OSName OSVersion ClientName ClientDomain CreateBy should not null" };
            }

            if ((centralConnectServiceRequest.Device.DeviceName == centralConnectServiceRequest.Device.ClientName)
                && (centralConnectServiceRequest.Device.Domain == centralConnectServiceRequest.Device.ClientDomain))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "DeviceName Domain should not same as ClientName ClientDomain" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }

            DeviceAction deviceAction = new DeviceAction(this.ConnectString);
            CentralConnectCode centralConnectCode = deviceAction.CreateDevice(centralConnectServiceRequest.Device);
            if (CentralConnectCode.Success != centralConnectCode)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "CreateDevice Fail of " + Enum.GetName(typeof(CentralConnectCode), centralConnectCode) };
            }

            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }

        CentralConnectServiceResponse ICentralConnectService.RetrieveDeviceByDeviceNameAndDomain(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.Device == null)
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "Device should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Device.DeviceName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Device.Domain))
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "DeviceName Domain should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new CentralConnectServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }

            DeviceAction deviceAction = new DeviceAction(this.ConnectString);
            List<Device> devices = deviceAction.RetrieveDeviceByDeviceNameAndDomain(centralConnectServiceRequest.Device.DeviceName, centralConnectServiceRequest.Device.Domain);
            return new CentralConnectServiceResponse() { Code = 0, Message = "Success", Devices = devices };
        }

        Core.ServiceResponse ICentralConnectService.RegisterDevice(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.RegisterDeviceMessage == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "RegisterDeviceMessage should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.RegisterDeviceMessage.Environment)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterDeviceMessage.DeviceName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterDeviceMessage.Domain)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterDeviceMessage.CreateBy))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Environment DeviceName Domain CreateBy should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }
            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }
            DeviceAction deviceAction = new DeviceAction(this.ConnectString);
            CentralConnectCode centralConnectCode = deviceAction.RegisterDevice(centralConnectServiceRequest.RegisterDeviceMessage.Environment,
                centralConnectServiceRequest.RegisterDeviceMessage.DeviceName,
                centralConnectServiceRequest.RegisterDeviceMessage.Domain,
                centralConnectServiceRequest.RegisterDeviceMessage.CreateBy);
            if (CentralConnectCode.Success != centralConnectCode)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "RegisterDevice Fail of " + Enum.GetName(typeof(CentralConnectCode), centralConnectCode) };
            }
            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }


        CentralConnectServiceResponse ICentralConnectService.RetrieveRegisterDeviceMessageByEnvironmentAndDeviceNameAndDomain(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            if (centralConnectServiceRequest.RegisterDeviceMessage == null)
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "RegisterDeviceMessage should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.RegisterDeviceMessage.Environment)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterDeviceMessage.DeviceName)
                || string.IsNullOrEmpty(centralConnectServiceRequest.RegisterDeviceMessage.Domain))
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "Environment DeviceName Domain should not null" };
            }

            if (string.IsNullOrEmpty(centralConnectServiceRequest.Id)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Token)
                || string.IsNullOrEmpty(centralConnectServiceRequest.Type))
            {
                return new CentralConnectServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, centralConnectServiceRequest, out message))
            {
                return new CentralConnectServiceResponse() { Code = 403, Message = centralConnectServiceRequest.Id + " of type " + centralConnectServiceRequest.Type + " have no access of " + message };
            }

            DeviceAction deviceAction = new DeviceAction(this.ConnectString);
            List<RegisterDeviceMessage> registerDeviceMessages = deviceAction.RetrieveRegisterDeviceMessageByEnvironmentAndDeviceNameAndDomain(centralConnectServiceRequest.RegisterDeviceMessage.Environment, centralConnectServiceRequest.RegisterDeviceMessage.DeviceName, centralConnectServiceRequest.RegisterDeviceMessage.Domain);
            return new CentralConnectServiceResponse() { Code = 0, Message = "Success", RegisterDeviceMessages = registerDeviceMessages };
        }

        Core.ServiceResponse ICentralConnectService.SendDeviceCollection(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            throw new NotImplementedException();
        }

        Core.ServiceResponse ICentralConnectService.SendDeviceEvent(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            throw new NotImplementedException();
        }

        Core.ServiceResponse ICentralConnectService.UnRegisterDevice(CentralConnectServiceRequest centralConnectServiceRequest)
        {
            throw new NotImplementedException();
        }

    }
}
