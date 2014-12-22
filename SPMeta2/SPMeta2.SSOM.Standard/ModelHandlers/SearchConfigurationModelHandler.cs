using System;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.Office.Server.Search.Portability;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class SearchConfigurationModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SearchConfigurationDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var site = webModelHost.HostSite;

            var definition = model.WithAssertAndCast<SearchConfigurationDefinition>("model", value => value.RequireNotNull());

            DeploySearchConfiguration(modelHost, site, definition);
        }

        protected string GetCurrentSearchConfiguration(SPSite site)
        {
            var owner = new SearchObjectOwner(SearchObjectLevel.SPSite, site.RootWeb);
            return new SearchConfigurationPortability(site).ExportSearchConfiguration(owner);
        }

        private void DeploySearchConfiguration(object modelHost, SPSite site, SearchConfigurationDefinition definition)
        {
            var conf = new SearchConfigurationPortability(site);
            var owner = new SearchObjectOwner(SearchObjectLevel.SPSite, site.RootWeb);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = conf,
                ObjectType = typeof(SearchConfigurationPortability),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            conf.ImportSearchConfiguration(owner, definition.SearchConfiguration);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = conf,
                ObjectType = typeof(SearchConfigurationPortability),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
