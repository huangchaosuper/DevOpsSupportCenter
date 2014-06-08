using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.Core
{
    public class CoreException:Exception
    {
        public enum ExceptionType { Application = 600, Business = 601, SQL = 602, Validation = 603 }
        private ExceptionType Type { get; set; }
        
        public CoreException(string message, Exception innerException)
            : this(ExceptionType.Application,message, innerException)
        {
        }

        public CoreException(ExceptionType exceptionType, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Type = exceptionType;
        }

        public CoreException(string message)
            : this(ExceptionType.Application,message)
        {
        }

        public CoreException(ExceptionType exceptionType, string message)
            : base(message)
        {
            this.Type = exceptionType;
        }

        public int Code
        {
            get { return (int)this.Type; }
        }
        public override string Message
        {
            get { return string.Format("{0}-{1}", Enum.GetName(typeof(ExceptionType), this.Type), base.Message); }
        }
    }
}
