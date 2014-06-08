using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using HP.TS.Devops.Core;

namespace HP.TS.Devops.Customer
{
    public class CompanyAction:HP.TS.Devops.Core.Action
    {
        public CompanyAction(string connectString)
            : base(connectString)
        {
        }
        public int CreateCompany(Company company)
        {
            this.Logger.Write("Create Company",company);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_CreateCompany", company.CompanyName, company.CountryCode, company.StateProvince, company.City, company.Street, company.PostalCode, company.CreateBy);
        }
        public List<Company> RetrieveCompanyByCompanyName(string companyName)
        {
            DataSet dataSet = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(this.ConnectString, "SP_RetrieveCompanyByCompanyName", companyName);
            List<Company> companys = new List<Company>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Company company = new Company();
                company.CompanyName = item["CompanyName"] as string;
                company.CountryCode = item["CountryCode"] as string;
                company.StateProvince = item["StateProvince"] as string;
                company.City = item["City"] as string;
                company.Street = item["Street"] as string;
                company.PostalCode = item["PostalCode"] as string;
                company.CreateBy = item["CreateBy"] as string;
                companys.Add(company);
            }
            return companys;
        }
        public int RemoveCompanyByCompanyName(string companyName)
        {
            this.Logger.Write("Remove Company " + companyName);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_RemoveCompanyByCompanyName", companyName);
        }
    }
}
