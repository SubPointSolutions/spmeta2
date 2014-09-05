using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SPMeta2.Regression.Reports.Data
{
    public class TestReportNode
    {
        public TestReportNode()
        {
        }

        public string Title { get; set; }

        public object Tag { get; set; }

        [XmlIgnore]
        public object SourceValue { get; set; }

        [XmlIgnore]
        public object DestValue { get; set; }

        private List<TestReportNodeProperty> _props = new List<TestReportNodeProperty>();

        public List<TestReportNodeProperty> Properties
        {
            get { return _props; }
            set
            {
                _props = value;
            }
        }

        private List<TestReportNode> _childItems = new List<TestReportNode>();

        public List<TestReportNode> ChildItems
        {
            get { return _childItems; }
            set
            {
                _childItems = value;
            }
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


    //public class TypedTestReportNode<TSource, TDest> : TestReportNode
    //{
    //    [XmlIgnore]
    //    public TSource SourceTypedValue { get; set; }

    //    [XmlIgnore]
    //    public TDest DestTypedValue { get; set; }


    //}
}
