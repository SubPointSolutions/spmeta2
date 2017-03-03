using SPMeta2.Regression.Service;
using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.Utils
{
    public static class RegressionUtils
    {
        #region static

        static RegressionUtils()
        {
            // replace and redirect all global trace for the regression
            ServiceContainer.Instance.ReplaceService(typeof(TraceServiceBase), new RegressionTraceService());
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
