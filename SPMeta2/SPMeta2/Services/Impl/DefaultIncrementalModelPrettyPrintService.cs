using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Models;

namespace SPMeta2.Services.Impl
{
    public class DefaultIncrementalModelPrettyPrintService : DefaultModelPrettyPrintService
    {
        #region methods

        protected override string GetCurrentIntent(ModelNode modelNode, string currentIndent)
        {
            var shouldDeploy = modelNode.Options.RequireSelfProcessing;

            if (shouldDeploy)
                return string.Format("[+] {0}", currentIndent);

            return string.Format("[-] {0}", currentIndent);
        }

        #endregion
    }
}
