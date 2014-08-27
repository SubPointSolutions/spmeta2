using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FarmDefinitionValidator : FarmModelHandler
    {
        #region methods

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var farmModel = model.WithAssertAndCast<FarmDefinition>("model", value => value.RequireNotNull());
        }

        #endregion
    }
}
