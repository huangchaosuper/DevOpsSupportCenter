using HP.TS.Devops.CentralConnect;
using HP.TS.Devops.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HP.TS.Devops.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICentralConnectService" in both code and config file together.
    [ServiceContract]
    public interface ICentralConnectService
    {
        [OperationContract]
        ServiceResponse CreateClient(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        CentralConnectServiceResponse RetrieveClientByClientNameAndDomain(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        ServiceResponse RegisterClient(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        CentralConnectServiceResponse RetrieveRegisterClientMessageByEnvironmentAndClientNameAndDomain(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        ServiceResponse VisibleClient(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        ServiceResponse SendClientMetrics(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        ServiceResponse InvisibleClient(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        ServiceResponse CreateDevice(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        CentralConnectServiceResponse RetrieveDeviceByDeviceNameAndDomain(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        ServiceResponse RegisterDevice(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        CentralConnectServiceResponse RetrieveRegisterDeviceMessageByEnvironmentAndDeviceNameAndDomain(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        ServiceResponse SendDeviceCollection(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        ServiceResponse SendDeviceEvent(CentralConnectServiceRequest centralConnectServiceRequest);
        [OperationContract]
        ServiceResponse UnRegisterDevice(CentralConnectServiceRequest centralConnectServiceRequest);
    }
    public class CentralConnectServiceResponse : Core.ServiceResponse
    {
        public List<Client> Clients { get; set; }
        public List<RegisterClientMessage> RegisterClientMessages { get; set; }
        public List<Device> Devices { get; set; }
        public List<RegisterDeviceMessage> RegisterDeviceMessages { get; set; }

    }
    public class CentralConnectServiceRequest : Security.SecurityRequest
    {
        public Client Client { get; set; }
        public RegisterClientMessage RegisterClientMessage { get; set; }
        public Device Device { get; set; }
        public RegisterDeviceMessage RegisterDeviceMessage { get; set; }
        public MetricsEnhancement MetricsEnhancement { get; set; }
        public CollectionEnhancement CollectionEnhancement { get; set; }
        public EventEnhancement EventEnhancement { get; set; }
    }
}
