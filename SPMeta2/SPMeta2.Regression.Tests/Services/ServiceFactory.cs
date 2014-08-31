using SPMeta2.Regression.Reports.Services;

namespace SPMeta2.Regression.Tests.Services
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
