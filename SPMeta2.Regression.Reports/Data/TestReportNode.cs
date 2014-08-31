using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Reports.Data
{
    public class TestReportNode
    {
        public TestReportNode()
        {
        }

        public string Title { get; set; }

        public object Tag { get; set; }

        public object SourceValue { get; set; }
        public object DestValue { get; set; }

        private List<TestReportNodeProperty> _props = new List<TestReportNodeProperty>();

        public IEnumerable<TestReportNodeProperty> Properties
        {
            get { return _props; }
        }

        private List<TestReportNode> _childItems = new List<TestReportNode>();

        public IEnumerable<TestReportNode> ChildItems
        {
            get { return _childItems; }
        }

        public void AddProperty(TestReportNodeProperty property)
        {
            _props.Add(property);
        }

        public void AddChildItem(TestReportNode item)
        {
            _childItems.Add(item);
        }
    }

    public class TypedTestReportNode<TSource, TDest> : TestReportNode
    {
        public TSource SourceTypedValue { get; set; }
        public TDest DestTypedValue { get; set; }

        
    }
}
