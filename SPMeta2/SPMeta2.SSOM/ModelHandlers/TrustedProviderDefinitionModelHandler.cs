using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using Microsoft.SharePoint.Administration;
using System.Security;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class TrustedAccessProviderModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TrustedAccessProviderDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TrustedAccessProviderDefinition>("model", value => value.RequireNotNull());

            DeployAftifact(farmModelHost, farmModelHost.HostFarm, definition);
        }

        private void DeployAftifact(FarmModelHost farmModelHost, SPFarm spFarm, TrustedAccessProviderDefinition definition)
        {
            // TODO

            throw new NotImplementedException();
        }

        #endregion

    }
}
