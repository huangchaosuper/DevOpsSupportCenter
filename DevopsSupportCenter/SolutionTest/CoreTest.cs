using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HP.TS.Devops.Core;

namespace SolutionTest
{
    [TestClass]
    public class CoreTest
    {
        private string ConnectString = @"Data Source=(LocalDB)\v11.0;Integrated Security=SSPI;AttachDbFileName =D:\Crazywolf\Devops\Devops.mdf";
        
        [TestMethod]
        public void TestEncrypt()
        {
            System.Console.WriteLine(Hash.Encrypt("Administrator", "111111"));
            System.Console.WriteLine(Hash.Encrypt("Administrator", "Welcome-2014"));
            System.Console.WriteLine(Hash.Encrypt("Administrator", "Welcome-2015"));
        }
        [TestMethod]
        public void TestLog()
        {
            Logger logger = new Logger(ConnectString, "TestLog");
            logger.Write(Logger.LogType.INFO, "this is a test");
        }
    }
}
