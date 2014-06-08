using HP.TS.Devops.Core;
using HP.TS.Devops.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HP.TS.Devops.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IContactService" in both code and config file together.
    [ServiceContract]
    public interface IContactService
    {
        [OperationContract]
        ServiceResponse CreateContact(ContactServiceRequest contactServiceRequest);
        [OperationContract]
        ContactServiceResponse RetrieveContactByFullName(ContactServiceRequest contactServiceRequest);
        [OperationContract]
        ServiceResponse RemoveContactByFullName(ContactServiceRequest contactServiceRequest);
    }
    public class ContactServiceResponse : Core.ServiceResponse
    {
        public List<Contact> Contacts { get; set; }
    }
    public class ContactServiceRequest : Security.SecurityRequest
    {
        public Contact Contact { get; set; }
    }
}
