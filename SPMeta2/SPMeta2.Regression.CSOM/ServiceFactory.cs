using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Regression.Reports.Services;

namespace SPMeta2.Regression.CSOM
{
    internal static class ServiceFactory
    {
        #region static

        static ServiceFactory()
        {
            ReportService = new DefaultReportService();
        }

        #endregion

        #region properties

        public static ReportService ReportService { get; set; }

        #endregion
    }
}
