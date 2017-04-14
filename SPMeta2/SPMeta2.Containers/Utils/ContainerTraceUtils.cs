using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Containers.Utils
{
    public static class ContainerTraceUtils
    {
        #region static

        static ContainerTraceUtils()
        {
        }

        #endregion

        #region methods

        public static void WriteLine(string message)
        {
            var m2logService = ServiceContainer.Instance.GetService<TraceServiceBase>();
            m2logService.Information(0, message);
        }

        #endregion
    }
}
