using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;

namespace SPMeta2.Services
{
    public abstract class ModelPrintServiceBase
    {
        #region methods
        public abstract string PrintModel(ModelNode modelNode);

        #endregion
    }
}
