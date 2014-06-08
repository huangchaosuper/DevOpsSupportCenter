using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HP.TS.Devops.CentralConnect;

namespace SolutionTest
{
    [TestClass]
    public class CentralConnectTest
    {
        private string ConnectString = @"Data Source=(LocalDB)\v11.0;Integrated Security=SSPI;AttachDbFileName =D:\Crazywolf\Devops\Devops.mdf";
        [TestMethod]
        public void TestCreateAndSendMetrics()
        {
            ClientAction clientAction = new ClientAction(ConnectString);
            MetricsEnhancement metricsEnhancement = new MetricsEnhancement();
            metricsEnhancement.ClientName = "HCTestClient";
            metricsEnhancement.Domain = "local";
            metricsEnhancement.Environment = "FUT1";
            metricsEnhancement.Type = "General";
            System.Console.WriteLine(clientAction.CreateAndSendMetrics(metricsEnhancement).ToString());
        }
    }
}
