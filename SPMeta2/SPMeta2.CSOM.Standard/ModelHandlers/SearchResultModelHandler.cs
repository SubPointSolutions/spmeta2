using System;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Administration;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class SearchResultModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SearchResultDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var site = webModelHost.HostSite;

            var definition = model.WithAssertAndCast<SearchResultDefinition>("model", value => value.RequireNotNull());

            throw new SPMeta2NotImplementedException("Search Result Source provision is not supported by CSOM yet.");
        }

        #endregion
    }
}
