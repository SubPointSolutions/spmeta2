using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Assertion
{
    public class PropertyValidationResult
    {
        public object Tag { get; set; }

        public PropResult Src { get; set; }
        public PropResult Dst { get; set; }

        public bool IsValid { get; set; }

        public string Message { get; set; }
    }
}
