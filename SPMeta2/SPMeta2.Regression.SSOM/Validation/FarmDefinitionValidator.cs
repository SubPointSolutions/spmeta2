using SPMeta2.Definitions.Base;
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

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<FarmDefinition>("model", value => value.RequireNotNull());

            var spObject = farmModelHost.HostFarm;

            ServiceFactory.AssertService
                       .NewAssert(definition, spObject)
                       .ShouldNotBeNull(spObject);
        }

        #endregion
    }
}
