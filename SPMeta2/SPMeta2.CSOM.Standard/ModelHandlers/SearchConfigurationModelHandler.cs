using System;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Administration;
using Microsoft.SharePoint.Client.Search.Portability;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class SearchConfigurationModelHandler : CSOMModelHandlerBase
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

        protected string GetCurrentSearchConfiguration(Site site)
        {
            var context = site.Context;

            var conf = new SearchConfigurationPortability(context);
            var owner = new SearchObjectOwner(context, SearchObjectLevel.SPSite);

            var result = conf.ExportSearchConfiguration(owner);

            context.ExecuteQueryWithTrace();

            return result.Value;
        }

        private void DeploySearchConfiguration(object modelHost, Site site, SearchConfigurationDefinition definition)
        {
            var context = site.Context;

            var conf = new SearchConfigurationPortability(context);
            var owner = new SearchObjectOwner(context, SearchObjectLevel.SPSite);

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

            context.ExecuteQueryWithTrace();
        }

        #endregion
    }
}
