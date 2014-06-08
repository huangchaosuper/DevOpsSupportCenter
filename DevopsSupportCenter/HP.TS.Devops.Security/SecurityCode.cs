using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.Security
{
    public enum SecurityCode
    {
        Success = 0,
        Timeout = 1,
        TokenIssue = 2,
        MethodAccessIssue = 3,
        UnknownRuleMapIssue = 4,
        UnknownUserAccessIssue = 5
    }
}
