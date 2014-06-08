using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace HP.TS.Devops.CentralConnect.ClientRegistration
{
    [WebServiceBinding(Name = "ClientRegistrationServiceSoap",
        Namespace = "http://www.hp.com/isee/webservices/")]
    public class ClientRegistrationService : SoapHttpClientProtocol
    {

        public ClientRegistrationService(string url)
        {
            Url = url;
        }

        [SoapDocumentMethod("http://www.hp.com/isee/webservices/RegisterClient2",
            RequestNamespace = "http://www.hp.com/isee/webservices/",
            ResponseNamespace = "http://www.hp.com/isee/webservices/",
            Use = SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public ClientRegistrationResult RegisterClient2(string request)
        {
            //object[] results = new object[100];
            try
            {
                object[] results = Invoke("RegisterClient2", new object[] { request });
                //results = Invoke("RegisterClient2", new object[] { request });
                //Console.WriteLine("--------------------");
                //Console.WriteLine(results[0].ToString());
                //Console.WriteLine("--------------------");
                return ((ClientRegistrationResult)(results[0]));
            }
            catch (Exception e)
            {
                ClientRegistrationResult ecsr = new ClientRegistrationResult();
                ecsr.IsSuccess = false;
                ecsr.RegistrationToken = "";
                ecsr.Gdid = "";
                Console.WriteLine("\nError: " + e.Message);
                Console.WriteLine("\nClient Registration Failed. Possible Reasons: This client may have linked with incorrect service.");

                return ecsr;
            }

        }
    }

    [Serializable]
    [XmlType(Namespace = "http://www.hp.com/isee/webservices/")]
    public abstract class ServiceResult
    {
        public bool IsSuccess;
        public ServiceError Error;
        public string Gdid;
        public int BackoffMultiplier;
    }

    [Serializable]
    [XmlType(Namespace = "http://www.hp.com/isee/webservices/")]
    public class ServiceError
    {
        public int Code;
        public bool IsTransientError;
        public bool IsClientError;
        public bool IsReRegisterError;
        public string Message;
    }

    [Serializable]
    [XmlType(Namespace = "http://www.hp.com/isee/webservices/")]
    public class ClientRegistrationResult : ServiceResult
    {
        public string RegistrationToken;
    }
}
