using System;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Administration;
using Microsoft.SharePoint.Client.Search.Portability;
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

        private void DeploySearchConfiguration(object modelHost, Site site, SearchConfigurationDefinition definition)
        {
            var context = site.Context;

            var conf = new SearchConfigurationPortability(context);
            var owner = new SearchObjectOwner(context, SearchObjectLevel.SPWeb);

            conf.ImportSearchConfiguration(owner, definition.SearchConfiguration);

            context.ExecuteQuery();
        }

        #endregion
    }
}
