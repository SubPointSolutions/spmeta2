using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Regression.Reports.Data;

namespace SPMeta2.Regression.Reports.Services
{
    public class DefaultReportService : ReportService
    {
        #region methods



        #endregion
        public override TypedTestReportNode<TSource, TDest> NotifyReportItem<TSource, TDest>(object tag, TSource source, TDest dest)
        {
            var reportItem = new TypedTestReportNode<TSource, TDest>
            {
                Tag = tag,

                SourceValue = source,
                DestValue = dest,

                SourceTypedValue = source,
                DestTypedValue = dest
            };

            InvokeOnReportNodeAdded(null, reportItem);

            return reportItem;
        }
    }

   
}
