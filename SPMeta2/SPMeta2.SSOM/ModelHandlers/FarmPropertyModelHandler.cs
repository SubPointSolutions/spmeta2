using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.SSOM.ModelHandlers.Base;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FarmPropertyModelHandler : PropertyModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(FarmPropertyDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var propertyModel = model.WithAssertAndCast<FarmPropertyDefinition>("model", value => value.RequireNotNull());

            var farm = farmModelHost.HostFarm;

            DeployProperty(modelHost, farm.Properties, propertyModel);
        }

        #endregion
    }
}
