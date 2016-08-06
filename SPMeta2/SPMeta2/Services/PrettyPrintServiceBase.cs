using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;

namespace SPMeta2.Services
{
    public abstract class PrettyPrintServiceBase
    {
        #region methods
        public abstract string ToPrettyPrint(ModelNode modelNode);
        #endregion
    }
}
