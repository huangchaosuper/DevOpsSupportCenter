using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HP.TS.Devops.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISecurityService" in both code and config file together.
    [ServiceContract]
    public interface ISecurityService
    {
        [OperationContract]
        SecurityServiceResponse RetrieveTokenByPassword(SecurityServiceRequest securityServiceRequest);
        [OperationContract]
        Core.ServiceResponse AddProjectUser(SecurityServiceRequest securityServiceRequest);
        [OperationContract]
        Core.ServiceResponse RemoveProjectUser(SecurityServiceRequest securityServiceRequest);
        [OperationContract]
        Core.ServiceResponse ResetProjectUserPassword(SecurityServiceRequest securityServiceRequest);
        [OperationContract]
        Core.ServiceResponse ChangeProjectUserPassword(SecurityServiceRequest securityServiceRequest);
        [OperationContract]
        Core.ServiceResponse ChangeAdministratorPassword(SecurityServiceRequest securityServiceRequest);

    }
    public class SecurityServiceResponse : Core.ServiceResponse
    {
        public Security.SecurityRequest SecurityRequest { get; set; }
    }
    public class SecurityServiceRequest : Security.SecurityRequest
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
