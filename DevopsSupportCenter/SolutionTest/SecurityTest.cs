using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using HP.TS.Devops.Security;

namespace SolutionTest
{
    [TestClass]
    public class SecurityTest
    {
        private string ConnectString = @"Data Source=(LocalDB)\v11.0;Integrated Security=SSPI;AttachDbFileName =D:\Crazywolf\Devops\Devops.mdf";
        [TestMethod]
        public void TestCheckAccess()
        {
            SecurityRequest securityRequest = new SecurityRequest() { Type = "Administrator" };
            HP.TS.Devops.Security.SecurityAction.CheckAccess(this.ConnectString, securityRequest, new StackFrame(1).GetMethod());
        }
        [TestMethod]
        public void TestRetrieveTokenByPassword()
        {
            SecurityAction securityAction = new SecurityAction(ConnectString);
            System.Console.WriteLine(securityAction.RetrieveTokenByPassword("CR59", "Welcome-2014", "ProjectUser"));
        }
        [TestMethod]
        public void TestAddProjectUser()
        {
            SecurityAction securityAction = new SecurityAction(ConnectString);
            System.Console.WriteLine(securityAction.AddProjectUser("CR59"));
        }
        [TestMethod]
        public void TestRemoveProjectUser()
        {
            SecurityAction securityAction = new SecurityAction(ConnectString);
            System.Console.WriteLine(securityAction.RemoveProjectUser("TEST"));
        }
        [TestMethod]
        public void TestChangeProjectUserPassword()
        {
            SecurityAction securityAction = new SecurityAction(ConnectString);
            System.Console.WriteLine(securityAction.ChangeProjectUserPassword("CR59", "Welcome-2014", "Welcome-2015"));
        }
        [TestMethod]
        public void TestResetProjectUserPassword()
        {
            SecurityAction securityAction = new SecurityAction(ConnectString);
            System.Console.WriteLine(securityAction.ResetProjectUserPassword("CR59"));
        }
        [TestMethod]
        public void TestChangeAdministratorPassword()
        {
            SecurityAction securityAction = new SecurityAction(ConnectString);
            System.Console.WriteLine(securityAction.ChangeAdministratorPassword("Administrator", "Welcome-2015", "Welcome-2014"));
        }
    }
}
