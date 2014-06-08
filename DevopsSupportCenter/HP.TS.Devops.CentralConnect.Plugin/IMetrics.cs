using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HP.TS.Devops.CentralConnect.Plugin
{
    [Guid("C11774EB-CBBF-473D-AFB2-B481946C1E5A")]
    public interface IMetrics
    {
        [DispId(1)]
        PluginResponse GenerateMetrics(MetricsRequest metricsRequest);
        [DispId(2)]
        List<EnhancementParameter> GetEnhancementParameters();
    }
}
