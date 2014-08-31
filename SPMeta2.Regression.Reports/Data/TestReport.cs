using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Reports.Data
{
    public class TestReport
    {
        public string TestName { get; set; }

        #region properties

        private readonly List<TestReportNode> _items = new List<TestReportNode>();

        public IEnumerable<TestReportNode> Items
        {
            get
            {
                return _items;
            }
        }

        #endregion

        public void AddReportItem(TestReportNode item)
        {
            _items.Add(item);
        }

        //public ReportItem LookupItemByTag(object tag)
        //{
        //    return _items.FirstOrDefault(i => i.Tag == tag);
        //}

    }

}
