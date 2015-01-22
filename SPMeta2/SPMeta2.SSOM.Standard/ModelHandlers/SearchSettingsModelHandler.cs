using System;
using System.Linq;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Audience;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class SearchSettingsModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SearchSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SearchSettingsDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, siteModelHost, definition);
        }

        private void DeployDefinition(object modelHost, SiteModelHost siteModelHost, SearchSettingsDefinition definition)
        {
            var site = siteModelHost.HostSite;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = site,
                ObjectType = typeof(Audience),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            ProcessObject(site, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = site,
                ObjectType = typeof(Audience),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        private void ProcessObject(SPSite site, SearchSettingsDefinition definition)
        {

        }

        #endregion
    }
}
