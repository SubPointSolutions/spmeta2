using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Regression.Reports.Data;
using SPMeta2.Regression.Reports.Assert;

namespace SPMeta2.Regression.Reports.Services
{
    public class TestReportNodeResult<TSource, TDest>
    {
        public TestReportNode TestReportNode { get; set; }
        public AssertPair<TSource, TDest> Assert { get; set; }
    }

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

        public TestReportNodeResult<TSource, TDest> NotifyReportItem<TSource, TDest>(object tag, TSource source, TDest dest)
        {
            var reportItem = new TestReportNode
            {
                Tag = tag,

                SourceValue = source,
                DestValue = dest,
            };

            InvokeOnReportNodeAdded(null, reportItem);

            var nodeResult = new TestReportNodeResult<TSource, TDest>
            {
                TestReportNode = reportItem,
                Assert = new AssertPair<TSource, TDest>(source, dest)
            };

            nodeResult.Assert.OnValidateProperty += (e, a) =>
            {
                reportItem.Properties.Add(new TestReportNodeProperty
                {
                    SourcePropName = a.SrcPropertyName,
                    SourcePropValue = a.SrcPropertyValue,
                    SourcePropType = a.SrcPropertyType,

                    DestPropName = a.DstPropertyName,
                    DestPropValue = a.DstPropertyValue,
                    DestPropType = a.DstPropertyType
                });

            };

          


            return nodeResult;
        }

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
