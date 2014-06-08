using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HP.TS.Devops.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DirectConnectService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DirectConnectService.svc or DirectConnectService.svc.cs at the Solution Explorer and start debugging.
    public class DirectConnectService : IDirectConnectService
    {
        Core.ServiceResponse IDirectConnectService.CreateDevice(DirectConnectServiceRequest directConnectServiceRequest)
        {
            throw new NotImplementedException();
        }

        DirectConnectServiceResponse IDirectConnectService.RetrieveDeviceByDeviceNameAndDomain(DirectConnectServiceRequest directConnectServiceRequest)
        {
            throw new NotImplementedException();
        }

        Core.ServiceResponse IDirectConnectService.RegisterDevice(DirectConnectServiceRequest directConnectServiceRequest)
        {
            throw new NotImplementedException();
        }

        Core.ServiceResponse IDirectConnectService.SendDeviceCollection(DirectConnectServiceRequest directConnectServiceRequest)
        {
            throw new NotImplementedException();
        }

        Core.ServiceResponse IDirectConnectService.SendDeviceEvent(DirectConnectServiceRequest directConnectServiceRequest)
        {
            throw new NotImplementedException();
        }

        Core.ServiceResponse IDirectConnectService.UnRegisterDevice(DirectConnectServiceRequest directConnectServiceRequest)
        {
            throw new NotImplementedException();
        }
    }
}
