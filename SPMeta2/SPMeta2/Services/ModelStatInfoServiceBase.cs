using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Diagnostic;

namespace SPMeta2.Services
{
    public class ModelStatInfoServiceOptions
    {
        #region properties

        public bool IncludeModelNodes { get; set; }

        #endregion
    }

    public abstract class ModelStatInfoServiceBase
    {
        public abstract ModelStatInfo GetModelStat(ModelNode modelNode);
        public abstract ModelStatInfo GetModelStat(ModelNode modelNode, ModelStatInfoServiceOptions options);

    }
}
