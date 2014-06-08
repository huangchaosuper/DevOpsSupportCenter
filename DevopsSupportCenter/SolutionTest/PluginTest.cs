using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HP.TS.Devops.CentralConnect.Plugin;
using Ionic.Zip;
using HP.TS.Devops.Dictionary;
using System.IO;

namespace SolutionTest
{
    [TestClass]
    public class PluginTest
    {
        private string ConnectString = @"Data Source=(LocalDB)\v11.0;Integrated Security=SSPI;AttachDbFileName =D:\Crazywolf\Devops\Devops.mdf";
        private readonly string workFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "Temp", "Official");
        [TestMethod]
        public void TestZipPackage()
        {
            /*
            IMetrics metrics = new HP.TS.Devops.CentralConnect.Plugin.General.MetricsMockup();
            PluginResponse pluginResponse = metrics.GenerateMetrics(new MetricsRequest());
            System.Console.WriteLine(System.Text.Encoding.Default.GetString(pluginResponse.FileContent));
             * */
            Guid id = Guid.NewGuid();
            string currentWorkFolder = System.IO.Path.Combine(workFolder, id.ToString());
            if (!System.IO.Directory.Exists(currentWorkFolder))
            {
                System.IO.Directory.CreateDirectory(currentWorkFolder);
            }
            //this.GenerateFiles(metricsRequest, currentWorkFolder);
            //HP.TS.Devops.CentralConnect.Plugin.Toolkits.Zip.GZip.Compress(@"D:\AAA", @"D:\", id.ToString());
            string ZipFileToCreate = @"D:\" + id.ToString();
            string DirectoryToZip = @"D:\AAA";
            using (ZipFile zip = new ZipFile())
            {
                // note: this does not recurse directories! 
                String[] filenames = System.IO.Directory.GetFiles(DirectoryToZip);

                // This is just a sample, provided to illustrate the DotNetZip interface.  
                // This logic does not recurse through sub-directories.
                // If you are zipping up a directory, you may want to see the AddDirectory() method, 
                // which operates recursively. 
                foreach (String filename in filenames)
                {
                    Console.WriteLine("Adding {0}...", filename);
                    ZipEntry e = zip.AddFile(filename);
                    e.Comment = "Added by Cheeso's CreateZip utility.";
                }

                zip.Comment = String.Format("This zip archive was created by the CreateZip example application on machine '{0}'",
                   System.Net.Dns.GetHostName());

                zip.Save(ZipFileToCreate);
            }
        }

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
