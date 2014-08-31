using System.Collections.Generic;

namespace SPMeta2.Regression.Reports.Data
{
    public class TestClassReport
    {
        public string ClassName { get; set; }

        private readonly List<TestReport> _items = new List<TestReport>();

        public IEnumerable<TestReport> Items
        {
            get
            {
                return _items;
            }
        }

        public void AddTestReportItem(TestReport item)
        {
            _items.Add(item);
        }
    }
}
