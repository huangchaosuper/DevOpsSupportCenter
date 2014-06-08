using HP.TS.Devops.CentralConnect.Plugin;
using HP.TS.Devops.Dictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HP.TS.Devops.CentralConnect
{
    public class EnhancementAction : HP.TS.Devops.Core.Action
    {
        private readonly string CentralConnectMetrics = "CentralConnectMetrics";
        private readonly string CentralConnectCollection = "CentralConnectCollection";
        private readonly string CentralConnectEvent = "CentralConnectEvent";
        private readonly string PluginResource = "Plugin";

        public EnhancementAction(string connectString)
            : base(connectString)
        { }

        public PluginResponse InvokeMetrics(MetricsRequest metricsRequest,string metricsType)
        {
            PluginResourceAction pluginResourceAction = new PluginResourceAction(this.ConnectString);
            List<PluginMap> pluginMaps = pluginResourceAction.RetrievePluginMapByClassAndType(CentralConnectMetrics, metricsType);
            if (pluginMaps.Count != 1)
            {
                return new PluginResponse() { IsSuccess = false, Message = Enum.GetName(typeof(CentralConnectCode), CentralConnectCode.PluginMapNotExist) };
            }
            Logger.Write("PluginMaps", pluginMaps[0]);
            PluginMap pluginMap = pluginMaps[0];
            Resource resource = pluginResourceAction.RetrieveResourceByTypeAndFileName(PluginResource, pluginMap.FileName);
            if(string.IsNullOrEmpty(resource.FileName))
            {
                return new PluginResponse() { IsSuccess = false, Message = Enum.GetName(typeof(CentralConnectCode), CentralConnectCode.ResourceFileNotExist) };
            }
            System.Reflection.Assembly assembly = null;
            if (!LoadPluginResourceFile(resource, out assembly) || assembly == null)
            {
                return new PluginResponse() { IsSuccess = false, Message = Enum.GetName(typeof(CentralConnectCode), CentralConnectCode.LoadResourceFileFail) };
            }
            IMetrics iMetrics = (IMetrics)assembly.CreateInstance(pluginMap.ClassFullName);
            if (iMetrics == null)
            {
                return new PluginResponse() { IsSuccess = false, Message = Enum.GetName(typeof(CentralConnectCode), CentralConnectCode.CreateMetricsInstanceFail) };
            }
            DictionaryAction dictionaryAction = new DictionaryAction(this.ConnectString);
            string workspace = dictionaryAction.RetrieveValueByTypeAndKey("Settings", "Workspace");
            metricsRequest.Workspace = System.IO.Path.Combine(workspace, CentralConnectMetrics);
            return iMetrics.GenerateMetrics(metricsRequest);
        }

        private bool LoadPluginResourceFile(Resource resource, out System.Reflection.Assembly assembly)
        {
            DictionaryAction dictionaryAction = new DictionaryAction(this.ConnectString);
            string resources = dictionaryAction.RetrieveValueByTypeAndKey("Settings", "Resources");
            string resourcePath = System.IO.Path.Combine(resources, PluginResource);
            if (!Directory.Exists(resourcePath))
            {
                Directory.CreateDirectory(resourcePath);
            }
            string resourceFile = System.IO.Path.Combine(resourcePath, resource.FileName);
            if (!System.IO.File.Exists(resourceFile))
            {
                using (FileStream fileStream = new FileStream(resourceFile, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    fileStream.Write(resource.FileContent, 0, resource.FileContent.Length);
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            assembly = System.Reflection.Assembly.LoadFrom(resourceFile);
            return true;
        }
    }
}
