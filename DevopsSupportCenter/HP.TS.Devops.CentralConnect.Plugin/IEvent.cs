using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.CentralConnect.Plugin
{
    public interface IEvent
    {
        PluginResponse Generate(EventRequest eventRequest);
    }
}
