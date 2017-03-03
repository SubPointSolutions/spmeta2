using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Models;
using SPMeta2.Utils;

namespace SPMeta2.Services.Impl
{
    public class DefaultIncrementalModelPrettyPrintService : DefaultModelPrettyPrintService
    {
        #region methods

        protected override string GetCurrentIntent(ModelNode modelNode, string currentIndent)
        {
            bool? shouldDeploy = false;

            var incrementalRequireSelfProcessingValue = modelNode.NonPersistentPropertyBag
                .FirstOrDefault(p => p.Name == "_sys.IncrementalRequireSelfProcessingValue");

            if (incrementalRequireSelfProcessingValue != null)
                shouldDeploy = ConvertUtils.ToBoolWithDefault(incrementalRequireSelfProcessingValue.Value, false);

            if (shouldDeploy.Value)
                return string.Format("[+] {0}", currentIndent);

            return string.Format("[-] {0}", currentIndent);
        }

        #endregion
    }
}
