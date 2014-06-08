using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace HP.TS.Devops.Core
{
    public class Logger : HP.TS.Devops.Core.Action
    {
        private string LogClass { get; set; }
        public Logger(string connectString,string logClass)
            : base(connectString)
        {
            this.LogClass = logClass;
        }

        public enum LogType
        {
            ERROR = 1,
            WARN = 2,
            INFO = 3,
            DEBUG = 4
        }

        public void Write(LogType logType, string logInformation)
        {
            Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, "SP_WriteLog",this.LogClass, Enum.GetName(typeof(LogType), logType), logInformation);
        }

        public void Write(string logInformation)
        {
            this.Write(LogType.INFO, logInformation);
        }
        public void Write(object logObject)
        {
            this.Write(LogType.INFO, "", logObject);
        }
        public void Write(string logInformation ,object logObject)
        {
            this.Write(LogType.INFO, logInformation,logObject);
        }
        public void Write(LogType logType, string logInformation,object logObject)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            this.Write(LogType.INFO, string.Format("{0}-{1}", logInformation, jsonSerializer.Serialize(logObject)));
        }
    }
}
