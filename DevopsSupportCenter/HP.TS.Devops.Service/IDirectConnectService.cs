using HP.TS.Devops.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HP.TS.Devops.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDirectConnectService" in both code and config file together.
    [ServiceContract]
    public interface IDirectConnectService
    {
        [OperationContract]
        ServiceResponse CreateDevice(DirectConnectServiceRequest directConnectServiceRequest);
        [OperationContract]
        DirectConnectServiceResponse RetrieveDeviceByDeviceNameAndDomain(DirectConnectServiceRequest directConnectServiceRequest);
        [OperationContract]
        ServiceResponse RegisterDevice(DirectConnectServiceRequest directConnectServiceRequest);
        [OperationContract]
        ServiceResponse SendDeviceCollection(DirectConnectServiceRequest directConnectServiceRequest);
        [OperationContract]
        ServiceResponse SendDeviceEvent(DirectConnectServiceRequest directConnectServiceRequest);
        [OperationContract]
        ServiceResponse UnRegisterDevice(DirectConnectServiceRequest directConnectServiceRequest);
    }
    public class DirectConnectServiceResponse : Core.ServiceResponse
    {
    }
    public class DirectConnectServiceRequest : Security.SecurityRequest
    {
    }
}
