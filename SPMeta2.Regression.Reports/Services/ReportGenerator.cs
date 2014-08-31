using SPMeta2.Regression.Reports.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Reports.Services
{
    public abstract class ReportGenerator
    {
        public abstract byte[] GenerateClassReport(TestClassReport report);
    }
}
