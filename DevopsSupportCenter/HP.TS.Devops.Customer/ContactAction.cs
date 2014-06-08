using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HP.TS.Devops.Customer
{
    public class ContactAction:HP.TS.Devops.Core.Action
    {
        public ContactAction(string connectString)
            : base(connectString)
        { }
        public int CreateContact(Contact contact)
        {
            this.Logger.Write("Create Contact", contact);
            contact.FullName = string.Format("{0},{1}", contact.FirstName, contact.LastName);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_CreateContact", contact.FullName, contact.FirstName, contact.LastName, contact.EmailAddress, contact.Phone, contact.CreateBy);
        }
        public List<Contact> RetrieveContactByFullName(string fullName)
        {
            DataSet dataSet = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(this.ConnectString, "SP_RetrieveContactByFullName", fullName);
            List<Contact> contacts = new List<Contact>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Contact contact = new Contact();
                contact.FullName = item["FullName"] as string;
                contact.FirstName = item["FirstName"] as string;
                contact.LastName = item["LastName"] as string;
                contact.EmailAddress = item["EmailAddress"] as string;
                contact.Phone = item["Phone"] as string;
                contact.CreateBy = item["CreateBy"] as string;
                contacts.Add(contact);
            }
            return contacts;
        }
        public int RemoveContactByFullName(string fullName)
        {
            this.Logger.Write("Remove Contact " + fullName);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_RemoveContactByFullName", fullName);
        }
    }
}
