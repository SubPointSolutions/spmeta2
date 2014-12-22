using System;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.Office.Server.Search.Administration.Query;
using Microsoft.Office.Server.Search.Portability;
using Microsoft.Office.Server.Search.Query.Rules;
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

        protected Source GetCurrentSource(Microsoft.SharePoint.SPSite site, SearchResultDefinition definition)
        {
            FederationManager federationManager = null;
            SearchObjectOwner searchOwner = null;

            return GetCurrentSource(site, definition, out federationManager, out searchOwner);
        }

        protected Source GetCurrentSource(Microsoft.SharePoint.SPSite site, SearchResultDefinition definition,
            out FederationManager federationManager,
            out SearchObjectOwner searchOwner)
        {
            var context = SPServiceContext.GetContext(site);
            var searchAppProxy = context.GetDefaultProxy(typeof(SearchServiceApplicationProxy)) as SearchServiceApplicationProxy;

            federationManager = new FederationManager(searchAppProxy);
            searchOwner = new SearchObjectOwner(SearchObjectLevel.SPSite, site.RootWeb);

            return federationManager.GetSourceByName(definition.Name, searchOwner);
        }

        protected SearchProvider GetProviderByName(FederationManager federationManager, string providerName)
        {
            return federationManager.ListProviders()[providerName];
        }

        protected Source GetDefaultSource(FederationManager federationManager, SearchObjectOwner owner)
        {
            return federationManager.GetDefaultSource(owner);
        }

        private void DeploySearchResult(object modelHost, Microsoft.SharePoint.SPSite site, SearchResultDefinition definition)
        {
            FederationManager federationManager = null;
            SearchObjectOwner searchOwner = null;

            var currentSource = GetCurrentSource(site, definition, out federationManager, out searchOwner);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentSource,
                ObjectType = typeof(Source),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (currentSource == null)
            {
                currentSource = federationManager.CreateSource(searchOwner);
                currentSource.Name = definition.Name;

                if (definition.ProviderId.HasValue)
                    currentSource.ProviderId = definition.ProviderId.Value;
                else
                    currentSource.ProviderId = GetProviderByName(federationManager, definition.ProviderName).Id;
            }

            currentSource.Description = definition.Description ?? string.Empty;
            currentSource.CreateQueryTransform(new QueryTransformProperties(), definition.Query);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentSource,
                ObjectType = typeof(Source),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            currentSource.Commit();

            if (definition.IsDefault)
                federationManager.UpdateDefaultSource(currentSource.Id, searchOwner);
        }

        #endregion
    }
}
