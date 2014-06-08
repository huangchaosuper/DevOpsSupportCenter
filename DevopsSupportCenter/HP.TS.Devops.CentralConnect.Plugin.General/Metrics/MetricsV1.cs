using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

namespace HP.TS.Devops.CentralConnect.Plugin.General.Metrics
{
    public class MetricsV1:IMetrics
    {
        private string DeleteDeviceFullName { get; set; }
        PluginResponse IMetrics.GenerateMetrics(MetricsRequest metricsRequest)
        {
            AnalysisEnhancementArguments(metricsRequest.EnhancementArguments);
            PluginResponse pluginResponse = new PluginResponse();
            pluginResponse.IsSuccess = true;
            pluginResponse.FileName = Guid.NewGuid().ToString();
            pluginResponse.FileContent = this.GenerateZipPackage(pluginResponse.FileName, metricsRequest.Workspace, metricsRequest.ClientInformation, metricsRequest.DeviceInformations);
            return pluginResponse;
        }

        List<EnhancementParameter> IMetrics.GetEnhancementParameters()
        {
            return new List<EnhancementParameter>();
        }

        private void AnalysisEnhancementArguments(Dictionary<string, string> enhancementArguments)
        {
            if (enhancementArguments != null && enhancementArguments.ContainsKey("DeleteDeviceFullName"))
            {
                this.DeleteDeviceFullName = enhancementArguments["DeleteDeviceFullName"];
            }
        }

        private byte[] GenerateZipPackage(string fileName, string workSpace, ClientInformation clientInformation, List<DeviceInformation> deviceInformations)
        {
            string path = System.IO.Path.Combine(workSpace, fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string content = Resources.Metrics.Metrics.index;
            content = content.Replace(@"{{CSID}}",clientInformation.CSID);
            content = content.Replace(@"{{PrimaryFirstName}}", clientInformation.PrimaryFirstName);
            content = content.Replace(@"{{PrimaryLastName}}", clientInformation.PrimaryLastName);
            content = content.Replace(@"{{PrimaryEmailAddress}}", clientInformation.PrimaryEmailAddress);
            content = content.Replace(@"{{PrimaryPhone}}", clientInformation.PrimaryPhone);
            content = content.Replace(@"{{SecondaryFirstName}}", clientInformation.SecondaryFirstName);
            content = content.Replace(@"{{SecondaryLastName}}", clientInformation.SecondaryLastName);
            content = content.Replace(@"{{SecondaryEmailAddress}}", clientInformation.SecondaryEmailAddress);
            content = content.Replace(@"{{SecondaryPhone}}", clientInformation.SecondaryPhone);
            //updating client company in Index
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);
            XmlNode xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/PRS_Address/Address1");
            xmlNode.InnerText = clientInformation.Street;
            xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/PRS_Address/City");
            xmlNode.InnerText = clientInformation.City;
            xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/PRS_Address/PostalCode");
            xmlNode.InnerText = clientInformation.PostalCode;
            xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/PRS_Address/Country");
            xmlNode.InnerText = clientInformation.CountryCode;
            xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/PRS_Address/Region");
            xmlNode.InnerText = clientInformation.StateProvince;
            xmlNode = xmlDocument.SelectSingleNode("*/HP_ISEECustomer/Name");
            xmlNode.InnerText = clientInformation.CompanyName;
            string index = System.IO.Path.Combine(path, "index.xml");
            xmlDocument.Save(index);
            List<string> endpointList = new List<string>();
            //updateing device serial number and product number
            foreach (DeviceInformation item in deviceInformations)
            {
                string endpoint = Guid.NewGuid().ToString().Replace("-", "");
                string deviceFullName = string.Format("{0}.{1}", item.DeviceName, item.Domain);
                string osNameAndVersion = string.Format("{0},{1}", item.OSName, item.OSVersion);
                content = Resources.Metrics.Metrics.endpoint;
                content = content.Replace(@"{{PrimaryFirstName}}", clientInformation.PrimaryFirstName);
                content = content.Replace(@"{{PrimaryLastName}}", clientInformation.PrimaryLastName);
                content = content.Replace(@"{{PrimaryEmailAddress}}", clientInformation.PrimaryEmailAddress);
                content = content.Replace(@"{{PrimaryPhone}}", clientInformation.PrimaryPhone);
                content = content.Replace(@"{{SecondaryFirstName}}", clientInformation.SecondaryFirstName);
                content = content.Replace(@"{{SecondaryLastName}}", clientInformation.SecondaryLastName);
                content = content.Replace(@"{{SecondaryEmailAddress}}", clientInformation.SecondaryEmailAddress);
                content = content.Replace(@"{{SecondaryPhone}}", clientInformation.SecondaryPhone);
                //CSID
                xmlDocument.LoadXml(content);
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='AutoDetectedSystemSerialNumber']");
                xmlNode.Attributes["value"].Value = item.SerialNumber;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='TimestampGenerated']");
                xmlNode.Attributes["value"].Value = DateTime.UtcNow.ToString(@"yyyy/MM/dd HH:mm:ss \G\M\T");
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='ProductId']");
                xmlNode.Attributes["value"].Value = item.ProductNumber;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='SystemModel']");
                xmlNode.Attributes["value"].Value = item.CoreSystemModel;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='CoreSystemModel']");
                xmlNode.Attributes["value"].Value = item.CoreSystemModel;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='Hostname']");
                xmlNode.Attributes["value"].Value = item.DeviceName;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='FQDN']");
                xmlNode.Attributes["value"].Value = deviceFullName;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='IPAddress']");
                xmlNode.Attributes["value"].Value = item.IPAddress;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='MacAddress']");
                xmlNode.Attributes["value"].Value = item.MAC;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='HPID']");
                xmlNode.Attributes["value"].Value = clientInformation.HPPID;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_OOSIdentifiers/CSID/Section/Property[@name='OSNameAndVersionString']");
                xmlNode.Attributes["value"].Value = osNameAndVersion;
                //PN&SN
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_ISEEEntitlementParameters/SerialNumber");
                xmlNode.InnerText = item.SerialNumber;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_ISEEEntitlementParameters/ProductNumber");
                xmlNode.InnerText = item.ProductNumber;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/HP_ISEEEntitlementParameters/ProductId");
                xmlNode.InnerText = item.ProductNumber;
                //delete device flag
                xmlNode = xmlDocument.SelectSingleNode("*/LDID");
                xmlNode.InnerText = endpoint;
                if (deviceFullName.Equals(this.DeleteDeviceFullName))
                {
                    xmlNode = xmlDocument.SelectSingleNode("*/LDID");
                    xmlNode.Attributes["state"].Value = "disabled";
                    xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/ObjectOfServiceProperties/Property[@name='Transport_Enabled']");
                    xmlNode.Attributes["value"].Value = "0";
                }
                //Service&Support Partner Id
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/ObjectOfServiceProperties/Property[@name='CPServiceProviderID']");
                xmlNode.Attributes["value"].Value = clientInformation.ServicePartnerId;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/ObjectOfServiceProperties/Property[@name='CPSupportProviderID']");
                xmlNode.Attributes["value"].Value = clientInformation.SupportPartnerId;
                //collectionFlag
                if (!"false".Equals(item.CollectionProblem))
                {
                    xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/ObjectOfServiceProperties/Property[@name='Service::Server_Basic_Configuration_Collection::functional']");
                    xmlNode.Attributes["value"].Value = "false";
                }
                //monitor Flag
                if (!"false".Equals(item.MonitoringProblem))
                {
                    xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/ObjectOfServiceProperties/Property[@name='Service::Query Connection::functional']");
                    xmlNode.Attributes["value"].Value = "false";
                    xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/ObjectOfServiceProperties/Property[@name='Service::Subscription Manager::functional']");
                    xmlNode.Attributes["value"].Value = "false";
                }
                //company
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/PRS_Address/Address1");
                xmlNode.InnerText = clientInformation.Street;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/PRS_Address/City");
                xmlNode.InnerText = clientInformation.City;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/PRS_Address/PostalCode");
                xmlNode.InnerText = clientInformation.PostalCode;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/PRS_Address/Country");
                xmlNode.InnerText = clientInformation.CountryCode;
                xmlNode = xmlDocument.SelectSingleNode("*/AssociatedDevice/PRS_Address/Region");
                xmlNode.InnerText = clientInformation.StateProvince;
                xmlNode = xmlDocument.SelectSingleNode("*/HP_ISEECustomer/Name");
                xmlNode.InnerText = clientInformation.CompanyName;
                xmlDocument.Save(System.IO.Path.Combine(path, endpoint+".xml"));
                endpointList.Add(endpoint);
            }
            //attach endpoint detail information to index
            xmlDocument.Load(index);
            xmlNode = xmlDocument.SelectSingleNode("*//AssociatedDevice");
            foreach (string item in endpointList)
            {
                XmlElement xmlElement = xmlDocument.CreateElement("PRS_Attachment");
                xmlDocument.DocumentElement.InsertAfter(xmlElement, xmlNode);
                XmlNode parentNode = xmlDocument.SelectSingleNode("*/PRS_Attachment");

                xmlElement = xmlDocument.CreateElement("Caption");
                parentNode.AppendChild(xmlElement);

                xmlElement = xmlDocument.CreateElement("Description");
                parentNode.AppendChild(xmlElement);

                xmlElement = xmlDocument.CreateElement("DirtyFlag");
                xmlElement.InnerText = "0";
                parentNode.AppendChild(xmlElement);

                xmlElement = xmlDocument.CreateElement("AttachmentName");
                xmlElement.InnerText = item + ".xml";
                parentNode.AppendChild(xmlElement);

                xmlElement = xmlDocument.CreateElement("AttachmentObject");
                parentNode.AppendChild(xmlElement);

                xmlElement = xmlDocument.CreateElement("AttachmentReference");
                parentNode.AppendChild(xmlElement);

                xmlElement = xmlDocument.CreateElement("AttachmentSize");
                FileInfo fileInfo = new FileInfo(System.IO.Path.Combine(path, item + ".xml"));
                xmlElement.InnerText = fileInfo.Length.ToString();
                parentNode.AppendChild(xmlElement);

                xmlElement = xmlDocument.CreateElement("AttachmentType");
                xmlElement.InnerText = "1";
                parentNode.AppendChild(xmlElement);

                xmlElement = xmlDocument.CreateElement("Format");
                xmlElement.InnerText = "xml";
                parentNode.AppendChild(xmlElement);

                xmlElement = xmlDocument.CreateElement("Protocol");
                parentNode.AppendChild(xmlElement);
            }
            xmlNode = xmlDocument.SelectSingleNode("*/OOS_Support/Property[@name='MetricsTime']");
            xmlNode.Attributes["value"].Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            xmlDocument.Save(index);

            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddDirectory(path);
                zipFile.Save(path+".zip");
            }
            System.IO.Directory.Delete(path,true);
            FileStream fileStream = new FileStream(path+".zip", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader streamReader = new StreamReader(fileStream);
            byte[] source = new byte[fileStream.Length];
            fileStream.Read(source, 0, (int)fileStream.Length);
            return source;
        }
    }
}
