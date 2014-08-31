using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Regression.Reports.Data;

namespace SPMeta2.Regression.Reports.Services
{
    public abstract class ReportService
    {
        #region contructors

        #endregion

        #region methods

        public TestReport AddTestReport(string testName)
        {
            var report = new TestReport
            {
                TestName = testName
            };

            return report;
        }

        #endregion

        #region static

        public abstract TypedTestReportNode<TSource, TDest> NotifyReportItem<TSource, TDest>(object tag, TSource source, TDest dest);

        #endregion

        protected static void InvokeOnReportNodeAdded(object sender, TestReportNode item)
        {
            if (OnReportItemAdded != null)
            {
                OnReportItemAdded(sender, new OnTestReportNodeAddedEventArgs
                {
                    Node = item
                });
            }
        }

        public static EventHandler<OnTestReportNodeAddedEventArgs> OnReportItemAdded;
    }

    public class OnTestReportNodeAddedEventArgs : EventArgs
    {
        public TestReportNode Node { get; set; }
    }
}
