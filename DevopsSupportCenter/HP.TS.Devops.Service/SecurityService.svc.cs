using HP.TS.Devops.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HP.TS.Devops.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SecurityService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SecurityService.svc or SecurityService.svc.cs at the Solution Explorer and start debugging.
    public class SecurityService : ISecurityService
    {
        private readonly string ConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["connectString"].ConnectionString;
        SecurityServiceResponse ISecurityService.RetrieveTokenByPassword(SecurityServiceRequest securityServiceRequest)
        {
            if (string.IsNullOrEmpty(securityServiceRequest.UserName) 
                || string.IsNullOrEmpty(securityServiceRequest.CurrentPassword)
                || string.IsNullOrEmpty(securityServiceRequest.Type))
            {
                return new SecurityServiceResponse() { Code = 400, Message = "Type UserName and CurrentPassword should not null" };
            }

            if(!securityServiceRequest.Type.Equals("Administrator")&&!securityServiceRequest.Type.Equals("ProjectUser"))
            {
                return new SecurityServiceResponse() { Code = 400, Message = "Type should be Administrator or ProjectUser" };
            }

            SecurityAction securityAction = new SecurityAction(this.ConnectString);
            Guid token = securityAction.RetrieveTokenByPassword(securityServiceRequest.UserName,securityServiceRequest.CurrentPassword,securityServiceRequest.Type);
            if(token == Guid.Empty)
            {
                return new SecurityServiceResponse() { Code = 500, Message = "Username or Password not correct" };
            }

            return new SecurityServiceResponse() { 
                Code = 0, 
                Message = "Success", 
                SecurityRequest = new SecurityRequest() {
                    Id = securityServiceRequest.UserName,
                    Type = securityServiceRequest.Type,
                    Token = token.ToString()
                }
            };
        }

        Core.ServiceResponse ISecurityService.AddProjectUser(SecurityServiceRequest securityServiceRequest)
        {
            if (string.IsNullOrEmpty(securityServiceRequest.Id)
                || string.IsNullOrEmpty(securityServiceRequest.Token)
                || string.IsNullOrEmpty(securityServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }
            
            if (!securityServiceRequest.Type.Equals("Administrator") && !securityServiceRequest.Type.Equals("ProjectUser"))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Type should be Administrator or ProjectUser" };
            }

            if (string.IsNullOrEmpty(securityServiceRequest.UserName))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "UserName should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, securityServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = securityServiceRequest.Id + " of type " + securityServiceRequest.Type + " have no access of " + message };
            }

            SecurityAction securityAction = new SecurityAction(this.ConnectString);
            if (securityAction.AddProjectUser(securityServiceRequest.UserName) <= 0)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "Add ProjectUser Fail" };
            }
            return new Core.ServiceResponse()
            {
                Code = 0,
                Message = "Success"
            };
        }

        Core.ServiceResponse ISecurityService.RemoveProjectUser(SecurityServiceRequest securityServiceRequest)
        {
            if (string.IsNullOrEmpty(securityServiceRequest.Id)
                || string.IsNullOrEmpty(securityServiceRequest.Token)
                || string.IsNullOrEmpty(securityServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            if (!securityServiceRequest.Type.Equals("Administrator") && !securityServiceRequest.Type.Equals("ProjectUser"))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Type should be Administrator or ProjectUser" };
            }

            if (string.IsNullOrEmpty(securityServiceRequest.UserName))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "UserName should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, securityServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = securityServiceRequest.Id + " of type " + securityServiceRequest.Type + " have no access of " + message };
            }

            SecurityAction securityAction = new SecurityAction(this.ConnectString);
            if (securityAction.RemoveProjectUser(securityServiceRequest.UserName) <= 0)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "Remove ProjectUser Fail" };
            }
            return new Core.ServiceResponse()
            {
                Code = 0,
                Message = "Success"
            };
        }

        Core.ServiceResponse ISecurityService.ResetProjectUserPassword(SecurityServiceRequest securityServiceRequest)
        {
            if (string.IsNullOrEmpty(securityServiceRequest.Id)
                 || string.IsNullOrEmpty(securityServiceRequest.Token)
                 || string.IsNullOrEmpty(securityServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            if (!securityServiceRequest.Type.Equals("Administrator") && !securityServiceRequest.Type.Equals("ProjectUser"))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Type should be Administrator or ProjectUser" };
            }

            if (string.IsNullOrEmpty(securityServiceRequest.UserName))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "UserName should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, securityServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = securityServiceRequest.Id + " of type " + securityServiceRequest.Type + " have no access of " + message };
            }

            SecurityAction securityAction = new SecurityAction(this.ConnectString);
            if (securityAction.ResetProjectUserPassword(securityServiceRequest.UserName) <= 0)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "Reset ProjectUser Password Fail" };
            }
            return new Core.ServiceResponse()
            {
                Code = 0,
                Message = "Success"
            };
        }

        Core.ServiceResponse ISecurityService.ChangeProjectUserPassword(SecurityServiceRequest securityServiceRequest)
        {
            if (string.IsNullOrEmpty(securityServiceRequest.Id)
                 || string.IsNullOrEmpty(securityServiceRequest.Token)
                 || string.IsNullOrEmpty(securityServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            if (!securityServiceRequest.Type.Equals("Administrator") && !securityServiceRequest.Type.Equals("ProjectUser"))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Type should be Administrator or ProjectUser" };
            }

            if (string.IsNullOrEmpty(securityServiceRequest.UserName)
                || string.IsNullOrEmpty(securityServiceRequest.CurrentPassword)
                || string.IsNullOrEmpty(securityServiceRequest.NewPassword))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "UserName CurrentPassword and NewPassword should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, securityServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = securityServiceRequest.Id + " of type " + securityServiceRequest.Type + " have no access of " + message };
            }

            SecurityAction securityAction = new SecurityAction(this.ConnectString);
            if (securityAction.ChangeProjectUserPassword(securityServiceRequest.UserName, securityServiceRequest.CurrentPassword, securityServiceRequest.NewPassword) <= 0)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "Change ProjectUser Password Fail" };
            }
            return new Core.ServiceResponse()
            {
                Code = 0,
                Message = "Success"
            };
        }

        Core.ServiceResponse ISecurityService.ChangeAdministratorPassword(SecurityServiceRequest securityServiceRequest)
        {
            if (string.IsNullOrEmpty(securityServiceRequest.Id)
                 || string.IsNullOrEmpty(securityServiceRequest.Token)
                 || string.IsNullOrEmpty(securityServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            if (!securityServiceRequest.Type.Equals("Administrator") && !securityServiceRequest.Type.Equals("ProjectUser"))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Type should be Administrator or ProjectUser" };
            }

            if (string.IsNullOrEmpty(securityServiceRequest.UserName)
                || string.IsNullOrEmpty(securityServiceRequest.CurrentPassword)
                || string.IsNullOrEmpty(securityServiceRequest.NewPassword))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "UserName CurrentPassword and NewPassword should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, securityServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = securityServiceRequest.Id + " of type " + securityServiceRequest.Type + " have no access of " + message };
            }

            SecurityAction securityAction = new SecurityAction(this.ConnectString);
            if (securityAction.ChangeAdministratorPassword(securityServiceRequest.UserName, securityServiceRequest.CurrentPassword, securityServiceRequest.NewPassword) <= 0)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "Change Administrator Password Fail" };
            }
            return new Core.ServiceResponse()
            {
                Code = 0,
                Message = "Success"
            };
        }
    }
}
