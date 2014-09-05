using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Reports.Data
{
    public class TestReportNodeProperty
    {
        public string SourcePropName { get; set; }
        public object SourcePropValue { get; set; }
        public string SourcePropType { get; set; }

        public string DestPropName { get; set; }
        public object DestPropValue { get; set; }
        public string DestPropType { get; set; }

        public string Comment { get; set; }
    }
}
