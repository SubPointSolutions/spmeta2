using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Common
{
    public class EventHooks
    {
        #region properties

        public bool OnProvisioned { get; set; }
        public bool OnProvisioning { get; set; }

        public ModelNode ModelNode { get; set; }

        public string Tag { get; set; }

        #endregion
    }
}
