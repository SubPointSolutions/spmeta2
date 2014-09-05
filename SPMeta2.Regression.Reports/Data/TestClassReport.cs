using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SPMeta2.Regression.Reports.Data
{
    public class TestClassReport
    {
        public string ClassName { get; set; }

        private  List<TestReport> _items = new List<TestReport>();

        public List<TestReport> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        public void AddTestReportItem(TestReport item)
        {
            _items.Add(item);
        }
    }
}
