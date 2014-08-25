using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;
using Microsoft.SharePoint.Administration;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebApplicationModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebApplicationDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var webApplicationDefinition = model.WithAssertAndCast<WebApplicationDefinition>("model", value => value.RequireNotNull());

            DeployWebApplication(farmModelHost, farmModelHost.HostFarm, webApplicationDefinition);
        }

        private void DeployWebApplication(FarmModelHost farmModelHost, SPFarm farm, WebApplicationDefinition webApplicationDefinition)
        {

        }

        #endregion

    }
}
