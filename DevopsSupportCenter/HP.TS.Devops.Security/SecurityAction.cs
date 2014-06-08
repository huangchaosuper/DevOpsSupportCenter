using HP.TS.Devops.Core;
using HP.TS.Devops.Dictionary;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.Security
{
    public class SecurityAction : HP.TS.Devops.Core.Action
    {
        private string DefaultPassword = "Welcome-"+DateTime.Now.Year.ToString();
        public SecurityAction(string connectString)
            : base(connectString)
        { }
        public Guid RetrieveTokenByPassword(string username, string password,string type)
        {
            password = HP.TS.Devops.Core.Hash.Encrypt(username,password);
            Guid token = Guid.Empty;
            if (type == "Administrator")
            {
                SqlParameter[] ParamList =new SqlParameter[] { 
                    new SqlParameter("@UserName", username), 
                    new SqlParameter("@Password", password), 
                    new SqlParameter("@Token", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output },
                    new SqlParameter("@Return",System.Data.SqlDbType.Int){ Direction = System.Data.ParameterDirection.ReturnValue }
                };
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, System.Data.CommandType.StoredProcedure, "SP_RetrieveAdministratorTokenByPassword", ParamList);
                if (Int32.Parse(ParamList[3].Value.ToString()) > 0)
                {
                    token = Guid.Parse(ParamList[2].Value.ToString());
                }
            }
            else if (type == "ProjectUser")
            {
                SqlParameter[] ParamList = new SqlParameter[] { 
                    new SqlParameter("@UserName", username), 
                    new SqlParameter("@Password", password), 
                    new SqlParameter("@Token", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output },
                    new SqlParameter("@Return",System.Data.SqlDbType.Int){ Direction = System.Data.ParameterDirection.ReturnValue }
                };
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, System.Data.CommandType.StoredProcedure, "SP_RetrieveProjectUserTokenByPassword", ParamList);
                if (Int32.Parse(ParamList[3].Value.ToString()) > 0)
                {
                    token = Guid.Parse(ParamList[2].Value.ToString());
                }
            }
            return token;
            
        }
        public int AddProjectUser(string username)
        {
            string password = HP.TS.Devops.Core.Hash.Encrypt(username,this.DefaultPassword);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_CreateProjectUser", username, password);
        }
        public int RemoveProjectUser(string username)
        {
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_RemoveProjectUserByUserName", username);
        }
        public int ChangeProjectUserPassword(string username, string currectpassword, string newpassword)
        {
            currectpassword = HP.TS.Devops.Core.Hash.Encrypt(username,currectpassword);
            newpassword = HP.TS.Devops.Core.Hash.Encrypt(username,newpassword);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_ChangeProjectUserPassword", username, currectpassword, newpassword);
        }
        public int ResetProjectUserPassword(string username)
        {
            string password = HP.TS.Devops.Core.Hash.Encrypt(username, this.DefaultPassword);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_ResetProjectUserPassword", username, password);
        }
        public int ChangeAdministratorPassword(string username, string currectpassword,string newpassword)
        {
            currectpassword = HP.TS.Devops.Core.Hash.Encrypt(username,currectpassword);
            newpassword = HP.TS.Devops.Core.Hash.Encrypt(username,newpassword);
            return Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_ChangeAdministratorPassword", username, currectpassword, newpassword);
        }
        public static SecurityCode CheckAccess(string connectString, SecurityRequest securityRequest,out string message)
        {
            SecurityCode securityCode = CheckAccess(connectString, securityRequest, new System.Diagnostics.StackFrame(1).GetMethod());
            message = Enum.GetName(typeof(SecurityCode), securityCode);
            return securityCode;
        }
        public static SecurityCode CheckAccess(string connectString, SecurityRequest securityRequest)
        {
            return CheckAccess(connectString, securityRequest, new System.Diagnostics.StackFrame(1).GetMethod());
        }
        public static SecurityCode CheckAccess(string connectString,SecurityRequest securityRequest, System.Reflection.MethodBase methodBase)
        {
            new HP.TS.Devops.Core.Logger(connectString, "HP.TS.Devops.Security.CheckAccess").Write(string.Format("ID={0},Type={1},Token={2},Method={3}", securityRequest.Id, securityRequest.Type, Guid.Parse(securityRequest.Token), methodBase.Name));
            int checkUserAccess = CheckUserAccess(connectString, securityRequest);//-1, 0 ,1
            if (checkUserAccess == -1)
            {
                return SecurityCode.Timeout;//timeout
            }
            else if (checkUserAccess == 0)
            {
                return SecurityCode.TokenIssue;//token error
            }
            else if (checkUserAccess == 1)
            {
                int checkMethodAccess = CheckMethodAccess(connectString, securityRequest, methodBase);//0, 1
                if (checkMethodAccess == 0)
                {
                    return SecurityCode.MethodAccessIssue;//method no access
                }
                else if (checkMethodAccess == 1)
                {
                    return SecurityCode.Success;//success
                }
                else
                {
                    return SecurityCode.UnknownRuleMapIssue;//unknown rulemap error
                }
            }
            else
            {
                return SecurityCode.UnknownUserAccessIssue;//unknown user error
            }
        }
        private static int CheckUserAccess(string connectString, SecurityRequest securityRequest)
        {
            int rtnCode = 0;
            if (securityRequest.Type == "Administrator")
            {
                SqlParameter[] ParamList = new SqlParameter[] { 
                    new SqlParameter("@UserName", securityRequest.Id), 
                    new SqlParameter("@Token", Guid.Parse(securityRequest.Token)), 
                    new SqlParameter("@Code", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output }
                };
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(connectString, System.Data.CommandType.StoredProcedure, "SP_CheckAdministratorAccessByToken", ParamList);
                rtnCode = Int32.Parse(ParamList[2].Value.ToString());
            }
            else if (securityRequest.Type == "ProjectUser")
            {
                SqlParameter[] ParamList = new SqlParameter[] { 
                    new SqlParameter("@UserName", securityRequest.Id), 
                    new SqlParameter("@Token", Guid.Parse(securityRequest.Token)), 
                    new SqlParameter("@Code", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output }
                };
                Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(connectString, System.Data.CommandType.StoredProcedure, "SP_CheckProjectUserAccessByToken", ParamList);
                rtnCode = Int32.Parse(ParamList[2].Value.ToString());
            }

            return rtnCode;
        }
        private static int CheckMethodAccess(string connectString, SecurityRequest securityRequest, System.Reflection.MethodBase methodBase)
        {
            SqlParameter[] ParamList = new SqlParameter[] { 
                    new SqlParameter("@Rule", securityRequest.Type), 
                    new SqlParameter("@Map", methodBase.Name), 
                    new SqlParameter("@Return",System.Data.SqlDbType.Int){ Direction = System.Data.ParameterDirection.ReturnValue }
                };
            Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(connectString, System.Data.CommandType.StoredProcedure, "SP_CheckAccess", ParamList);
            return Int32.Parse(ParamList[2].Value.ToString());
        }
    }
}
