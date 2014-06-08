using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.Core
{
    public class Action
    {
        protected string ConnectString { get; set; }
        protected Logger Logger { get { return new Logger(this.ConnectString, this.GetType().FullName); } }
        public Action(string connectString)
        {
            this.ConnectString = connectString;
        }
    }
}