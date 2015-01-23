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
            //var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SearchSettingsDefinition>("model", value => value.RequireNotNull());

            if (modelHost is SiteModelHost)
            {
                DeployDefinition(modelHost, (modelHost as SiteModelHost).HostSite.RootWeb, definition);
            }
            else if (modelHost is WebModelHost)
            {
                DeployDefinition(modelHost, (modelHost as WebModelHost).HostWeb, definition);
            }
        }

        private void DeployDefinition(object modelHost, SPWeb web, SearchSettingsDefinition definition)
        {
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            ProcessObject(web, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            web.Update();
        }

        private void ProcessObject(SPWeb web, SearchSettingsDefinition definition)
        {
            // web.AllProperties["SRCH_ENH_FTR_URL_SITE"] = string.
        }

        #endregion
    }
}
