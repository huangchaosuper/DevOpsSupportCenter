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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ContactService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ContactService.svc or ContactService.svc.cs at the Solution Explorer and start debugging.
    public class ContactService : IContactService
    {
        private readonly string ConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["connectString"].ConnectionString;

        Core.ServiceResponse IContactService.CreateContact(ContactServiceRequest contactServiceRequest)
        {
            if (contactServiceRequest.Contact == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Contact should not null" };
            }

            if (string.IsNullOrEmpty(contactServiceRequest.Contact.FirstName)
                || string.IsNullOrEmpty(contactServiceRequest.Contact.LastName)
                || string.IsNullOrEmpty(contactServiceRequest.Contact.EmailAddress)
                || string.IsNullOrEmpty(contactServiceRequest.Contact.Phone)
                || string.IsNullOrEmpty(contactServiceRequest.Contact.CreateBy))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "FirstName LastName EmailAddress Phone CreateBy should not null" };
            }
            
            if (string.IsNullOrEmpty(contactServiceRequest.Id)
                || string.IsNullOrEmpty(contactServiceRequest.Token)
                || string.IsNullOrEmpty(contactServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }
            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, contactServiceRequest,out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = contactServiceRequest.Id + " of type " + contactServiceRequest.Type + " have no access of " + message };
            }

            ContactAction contactAction = new ContactAction(this.ConnectString);
            if (contactAction.CreateContact(contactServiceRequest.Contact) <= 0)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "Create Contact Fail" };
            }

            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }

        ContactServiceResponse IContactService.RetrieveContactByFullName(ContactServiceRequest contactServiceRequest)
        {
            if (contactServiceRequest.Contact == null)
            {
                return new ContactServiceResponse() { Code = 400, Message = "Contact should not null" };
            }

            if (string.IsNullOrEmpty(contactServiceRequest.Contact.FullName))
            {
                return new ContactServiceResponse() { Code = 400, Message = "ContactName should not null" };
            }

            if (string.IsNullOrEmpty(contactServiceRequest.Id)
                || string.IsNullOrEmpty(contactServiceRequest.Token)
                || string.IsNullOrEmpty(contactServiceRequest.Type))
            {
                return new ContactServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, contactServiceRequest, out message))
            {
                return new ContactServiceResponse() { Code = 403, Message = contactServiceRequest.Id + " of type " + contactServiceRequest.Type + " have no access of " + message };
            }

            ContactAction contactAction = new ContactAction(this.ConnectString);
            List<Contact> contacts = contactAction.RetrieveContactByFullName(contactServiceRequest.Contact.FullName);
            return new ContactServiceResponse() { Code = 0, Message = "Success", Contacts = contacts };
        }

        Core.ServiceResponse IContactService.RemoveContactByFullName(ContactServiceRequest contactServiceRequest)
        {
            if (contactServiceRequest.Contact == null)
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Contact should not null" };
            }

            if (string.IsNullOrEmpty(contactServiceRequest.Contact.FullName))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "ContactName should not null" };
            }

            if (string.IsNullOrEmpty(contactServiceRequest.Id)
                || string.IsNullOrEmpty(contactServiceRequest.Token)
                || string.IsNullOrEmpty(contactServiceRequest.Type))
            {
                return new Core.ServiceResponse() { Code = 400, Message = "Id Type and Token should not null" };
            }

            string message = string.Empty;
            if (HP.TS.Devops.Security.SecurityCode.Success != HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, contactServiceRequest, out message))
            {
                return new Core.ServiceResponse() { Code = 403, Message = contactServiceRequest.Id + " of type " + contactServiceRequest.Type + " have no access of " + message };
            }

            ContactAction contactAction = new ContactAction(this.ConnectString);
            if (contactAction.RemoveContactByFullName(contactServiceRequest.Contact.FullName) <= 0)
            {
                return new Core.ServiceResponse() { Code = 500, Message = "Remove Contact Fail" };
            }

            return new Core.ServiceResponse() { Code = 0, Message = "Success" };
        }
    }
}
