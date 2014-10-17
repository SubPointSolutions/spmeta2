using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy
{
    public class TaxonomyTermStoreModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TaxonomyTermStoreDefinition); }
        }

        #endregion

        #region methods

        protected TermStore FindTermStore(SiteModelHost siteModelHost, TaxonomyTermStoreDefinition termStoreModel)
        {
            return TaxonomyTermStoreModelHandler.FindTermStore(siteModelHost,
                termStoreModel.Name,
                termStoreModel.Id,
               termStoreModel.UseDefaultSiteCollectionTermStore);
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("model", value => value.RequireNotNull());
            var termStoreModel = model.WithAssertAndCast<TaxonomyTermStoreDefinition>("model", value => value.RequireNotNull());

            var termStore = FindTermStore(siteModelHost, termStoreModel);

            action(new TermStoreModelHost
            {
                HostTermStore = termStore
            });

            termStore.CommitAll();
        }

        private static Dictionary<string, TermStore> _storeCache = new Dictionary<string, TermStore>();
        private static readonly object _storeCacheLock = new object();

        internal static TermStore FindTermStore(SiteModelHost siteModelHost,
            string termStoreName,
            Guid? termStoreId,
            bool? useDefaultSiteCollectionTermStore)
        {
            var site = siteModelHost.HostSite;

            TermStore termStore = null;

            lock (_storeCacheLock)
            {
                var key = string.Format("{0}-{1}-{2}",
                    siteModelHost.HostClientContext.GetHashCode(),
                    siteModelHost.HostClientContext.Site.GetHashCode(),
                    string.Format("{0}-{1}-{2}", termStoreName, termStoreId, useDefaultSiteCollectionTermStore))
                        .ToLower();

                if (!_storeCache.ContainsKey(key))
                {
                    var session = TaxonomySession.GetTaxonomySession(siteModelHost.HostClientContext);
                    var client = siteModelHost.HostClientContext;

                    if (useDefaultSiteCollectionTermStore == true)
                    {
                        termStore = session.GetDefaultSiteCollectionTermStore();
                    }
                    else
                    {
                        if (termStoreId.HasValue && termStoreId != default(Guid))
                            termStore = session.TermStores.GetById(termStoreId.Value);
                        else if (!string.IsNullOrEmpty(termStoreName))
                            termStore = session.TermStores.GetByName(termStoreName);
                    }

                    if (termStore != null)
                    {
                        client.Load(termStore, s => s.Id);
                        client.Load(termStore, s => s.Name);

                        client.ExecuteQuery();
                    }

                    _storeCache.Add(key, termStore);
                }


                return _storeCache[key];
            }

            return termStore;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("model", value => value.RequireNotNull());
            var termStoreModel = model.WithAssertAndCast<TaxonomyTermStoreDefinition>("model", value => value.RequireNotNull());

            var termStore = TaxonomyTermStoreModelHandler.FindTermStore(siteModelHost,
              termStoreModel.Name,
              termStoreModel.Id,
             termStoreModel.UseDefaultSiteCollectionTermStore);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = termStore,
                ObjectType = typeof(TermStore),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = termStore,
                ObjectType = typeof(TermStore),
                ObjectDefinition = model,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
