using HP.TS.Devops.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using HP.TS.Devops.Core;

namespace HP.TS.Devops.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICompanyService" in both code and config file together.
    [ServiceContract]
    public interface ICompanyService
    {
        [OperationContract]
        ServiceResponse CreateCompany(CompanyServiceRequest companyServiceRequest);
        [OperationContract]
        CompanyServiceResponse RetrieveCompanyByCompanyName(CompanyServiceRequest companyServiceRequest);
        [OperationContract]
        ServiceResponse RemoveCompanyByCompanyName(CompanyServiceRequest companyServiceRequest);
    }
    public class CompanyServiceResponse:Core.ServiceResponse
    {
        public List<Company> Companys { get; set; }
    }
    public class CompanyServiceRequest:Security.SecurityRequest
    {
        public Company Company { get; set; }
    }
}
