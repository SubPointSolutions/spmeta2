using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class InformationRightsManagementSettingsModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(InformationRightsManagementSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<InformationRightsManagementSettingsDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, listModelHost, definition);
        }

        private void DeployDefinition(object modelHost, ListModelHost siteModelHost, InformationRightsManagementSettingsDefinition definition)
        {
            var currentSettings = GetCurrentInformationRightsManagementSettings(siteModelHost.HostList);


            currentSettings.Update();
        }

        protected SPInformationRightsManagementSettings GetCurrentInformationRightsManagementSettings(SPList list)
        {
            return list.InformationRightsManagementSettings;
        }

        #endregion
    }
}
