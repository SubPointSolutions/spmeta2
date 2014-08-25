using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.ModelHandlers.Base;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListPropertyModelHandler : PropertyModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ListPropertyDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var propertyModel = model.WithAssertAndCast<ListPropertyDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            DeployProperty(modelHost, list.RootFolder.Properties, propertyModel);
        }

        #endregion
    }
}
