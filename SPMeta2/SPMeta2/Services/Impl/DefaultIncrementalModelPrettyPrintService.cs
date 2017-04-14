using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Models;
using SPMeta2.Utils;
using SPMeta2.Extensions;
using SPMeta2.Common;

namespace SPMeta2.Services.Impl
{
    public class DefaultIncrementalModelPrettyPrintService : DefaultModelPrettyPrintService
    {
        #region methods

        protected override string GetCurrentIntent(ModelNode modelNode, string currentIndent)
        {
            var shouldDeployAsIncremental = modelNode.GetIncrementalRequireSelfProcessingValue();

            if (shouldDeployAsIncremental)
                return string.Format("[+] {0}", currentIndent);

            return string.Format("[-] {0}", currentIndent);
        }

        #endregion
    }
}
