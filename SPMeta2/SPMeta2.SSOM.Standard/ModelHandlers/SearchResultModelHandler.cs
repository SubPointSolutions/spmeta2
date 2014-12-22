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
    public class SearchResultModelHandler : SSOMModelHandlerBase
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

            DeploySearchResult(modelHost, site, definition);
        }

        private void DeploySearchResult(object modelHost, Microsoft.SharePoint.SPSite site, SearchResultDefinition definition)
        {
            //InvokeOnModelEvent(this, new ModelEventArgs
            //{
            //    CurrentModelNode = null,
            //    Model = null,
            //    EventType = ModelEventType.OnProvisioning,
            //    Object = ,
            //    ObjectType = typeof(SearchConfigurationPortability),
            //    ObjectDefinition = definition,
            //    ModelHost = modelHost
            //});
        }

        #endregion
    }
}
