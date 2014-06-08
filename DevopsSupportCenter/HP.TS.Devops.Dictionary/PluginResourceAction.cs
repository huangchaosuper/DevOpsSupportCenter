using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.Dictionary
{
    public class PluginResourceAction : HP.TS.Devops.Core.Action
    {
        public PluginResourceAction(string connectString)
            : base(connectString)
        { }

        public int AddResource(Resource resource) 
        {
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_AddResource",
                resource.ResourceType,
                resource.FileName,
                resource.FileContent,
                resource.CreateBy);
        }

        public Resource RetrieveResourceByTypeAndFileName(string resourceType,string fileName)
        {
            DataSet dataSet = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(this.ConnectString, "SP_RetrieveResourceByTypeAndFileName", resourceType, fileName);
            Resource resource = new Resource();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                resource.ResourceType = item["ResourceType"] as string;
                resource.FileName = item["FileName"] as string;
                resource.FileContent = item["FileContent"] as byte[];
                resource.CreateBy = item["CreateBy"] as string;
            }
            return resource;
        }

        public int AddPluginMap(PluginMap pluginMap)
        {
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_AddPluginMap",
                pluginMap.PluginClass,
                pluginMap.PluginType,
                pluginMap.Description,
                pluginMap.FileName,
                pluginMap.ClassFullName);
        }

        public List<PluginMap> RetrievePluginMapByClassAndType(string pluginClass,string pluginType)
        {
            DataSet dataSet = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(this.ConnectString, "SP_RetrievePluginMapByClassAndType", pluginClass, pluginType);
            List<PluginMap> pluginMaps = new List<PluginMap>();
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                PluginMap pluginMap = new PluginMap();
                pluginMap.PluginClass = item["PluginClass"] as string;
                pluginMap.PluginType = item["PluginType"] as string;
                pluginMap.Description = item["Description"] as string;
                pluginMap.FileName = item["FileName"] as string;
                pluginMap.ClassFullName = item["ClassFullName"] as string;
                pluginMaps.Add(pluginMap);
            }
            return pluginMaps;
        }
    }
}
