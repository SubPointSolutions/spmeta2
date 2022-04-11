using SPMeta2.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Diagnostic
{
    public static class SPMeta2Diagnostic
    {
        public static DiagnosticInfo GetDiagnosticInfo()
        {
            var service = ServiceContainer.Instance.GetService<DefaultDiagnosticInfoService>();

            return service.GetDiagnosticInfo();
        }
    }
}
