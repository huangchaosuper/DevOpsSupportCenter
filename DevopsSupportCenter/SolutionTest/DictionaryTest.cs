using HP.TS.Devops.Dictionary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SolutionTest
{
    [TestClass]
    public class DictionaryTest
    {
        private string ConnectString = @"Data Source=(LocalDB)\v11.0;Integrated Security=SSPI;AttachDbFileName =D:\Crazywolf\Devops\Devops.mdf";

        [TestMethod]
        public void TestAddResourceAndPluginMap()
        {
            PluginResourceAction resourceAction = new PluginResourceAction(ConnectString);
            Resource resource = new Resource();
            resource.ResourceType = "Plugin";

            FileStream fileStream = new FileStream(@"D:\Crazywolf\Devops\DevopsSupportCenter\HP.TS.Devops.CentralConnect.Plugin.General\bin\Debug\HP.TS.Devops.CentralConnect.Plugin.General.dll", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader streamReader = new StreamReader(fileStream);
            byte[] source = new byte[fileStream.Length];
            fileStream.Read(source, 0, (int)fileStream.Length);
            fileStream.Close();
            resource.ResourceType = "Plugin";
            resource.FileName = @"HP.TS.Devops.CentralConnect.Plugin.General.dll";
            resource.FileContent = source;
            resource.CreateBy = "UnitTest";
            int result = resourceAction.AddResource(resource);
            PluginMap pluginMap = new PluginMap();
            pluginMap.PluginClass = "CentralConnectMetrics";
            pluginMap.PluginType = "MetricsV1";
            pluginMap.Description = "Official Metrics Plugin";
            pluginMap.FileName = resource.FileName;
            pluginMap.ClassFullName = "HP.TS.Devops.CentralConnect.Plugin.General.Metrics.MetricsV1";
            result = resourceAction.AddPluginMap(pluginMap);
        }
    }
}
