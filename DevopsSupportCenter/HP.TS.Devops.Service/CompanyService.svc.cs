using HP.TS.Devops.Customer;
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CompanyService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CompanyService.svc or CompanyService.svc.cs at the Solution Explorer and start debugging.
    public class CompanyService : ICompanyService
    {
        private readonly string ConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["connectString"].ConnectionString;
        Core.ServiceResponse ICompanyService.CreateCompany(CompanyServiceRequest companyServiceRequest)
        {
            if (companyServiceRequest.Company == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Company should not null" };
            }

            if (string.IsNullOrEmpty(companyServiceRequest.Company.CompanyName)
                || string.IsNullOrEmpty(companyServiceRequest.Company.CountryCode)
                || string.IsNullOrEmpty(companyServiceRequest.Company.StateProvince)
                || string.IsNullOrEmpty(companyServiceRequest.Company.City)
                || string.IsNullOrEmpty(companyServiceRequest.Company.Street)
                || string.IsNullOrEmpty(companyServiceRequest.Company.PostalCode)
                || string.IsNullOrEmpty(companyServiceRequest.Company.CreateBy))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "CompanyName CountryCode StateProvince City Street PostalCode CreateBy should not null" };
            }
            
            if (string.IsNullOrEmpty(companyServiceRequest.Id)
                || string.IsNullOrEmpty(companyServiceRequest.Token)
                || string.IsNullOrEmpty(companyServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }
            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, companyServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = companyServiceRequest.Id + " of type " + companyServiceRequest.Type + " have no access of " + message };
            }

            CompanyAction companyAction = new CompanyAction(this.ConnectString);
            if (companyAction.CreateCompany(companyServiceRequest.Company) <= 0)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "Create Company Fail" };
            }

            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }

        CompanyServiceResponse ICompanyService.RetrieveCompanyByCompanyName(CompanyServiceRequest companyServiceRequest)
        {
            if (companyServiceRequest.Company == null)
            {
                return new CompanyServiceResponse() { Code = 400, Message = "Company should not null" };
            }

            if (string.IsNullOrEmpty(companyServiceRequest.Company.CompanyName))
            {
                return new CompanyServiceResponse() { Code = 400, Message = "CompanyName should not null" };
            }

            if (string.IsNullOrEmpty(companyServiceRequest.Id)
                || string.IsNullOrEmpty(companyServiceRequest.Token)
                || string.IsNullOrEmpty(companyServiceRequest.Type))
            {
                return new CompanyServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, companyServiceRequest, out message))
            {
                return new CompanyServiceResponse() { Code = 403, Message = companyServiceRequest.Id + " of type " + companyServiceRequest.Type + " have no access of " + message };
            }

            CompanyAction companyAction = new CompanyAction(this.ConnectString);
            List<Company> companys = companyAction.RetrieveCompanyByCompanyName(companyServiceRequest.Company.CompanyName);
            return new CompanyServiceResponse() { Code = 0, Message = "Success", Companys = companys };
        }

        Core.ServiceResponse ICompanyService.RemoveCompanyByCompanyName(CompanyServiceRequest companyServiceRequest)
        {
            if (companyServiceRequest.Company == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Company should not null" };
            }

            if (string.IsNullOrEmpty(companyServiceRequest.Company.CompanyName))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "CompanyName should not null" };
            }

            if (string.IsNullOrEmpty(companyServiceRequest.Id)
                || string.IsNullOrEmpty(companyServiceRequest.Token)
                || string.IsNullOrEmpty(companyServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, companyServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = companyServiceRequest.Id + " of type " + companyServiceRequest.Type + " have no access of " + message };
            }

            CompanyAction companyAction = new CompanyAction(this.ConnectString);
            if (companyAction.RemoveCompanyByCompanyName(companyServiceRequest.Company.CompanyName) <= 0)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "Remove Company Fail" };
            }

            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }
    }
}
