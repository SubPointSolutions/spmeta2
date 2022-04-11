﻿using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
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

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("model", value => value.RequireNotNull());
            var termStoreModel = model.WithAssertAndCast<TaxonomyTermStoreDefinition>("model", value => value.RequireNotNull());

            var termStore = FindTermStore(siteModelHost, termStoreModel);

            var termStoreModelHost = ModelHostBase.Inherit<TermStoreModelHost>(siteModelHost, context =>
            {
                context.HostTermStore = termStore;
            });

            action(termStoreModelHost);

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling termStore.CommitAll()");

            if (termStoreModelHost.ShouldUpdateHost)
            {
                termStore.CommitAll();
                termStore.Context.ExecuteQueryWithTrace();
            }
        }

        private static Dictionary<string, TermStore> _storeCache = new Dictionary<string, TermStore>();
        private static readonly object _storeCacheLock = new object();

        internal static TermStore FindTermStore(SiteModelHost siteModelHost,
            string termStoreName,
            Guid? termStoreId,
            bool? useDefaultSiteCollectionTermStore)
        {
            return FindTermStore(siteModelHost.HostSite, termStoreName, termStoreId, useDefaultSiteCollectionTermStore);
        }

        internal static TermStore FindTermStore(Site site,
            string termStoreName,
            Guid? termStoreId,
            bool? useDefaultSiteCollectionTermStore)
        {
            var clientContext = site.Context;
            TermStore termStore = null;

            lock (_storeCacheLock)
            {
                var key = string.Format("{0}-{1}-{2}",
                    clientContext.GetHashCode(),
                    clientContext.GetHashCode(),
                    string.Format("{0}-{1}-{2}", termStoreName, termStoreId, useDefaultSiteCollectionTermStore))
                        .ToLower();

                if (!_storeCache.ContainsKey(key))
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "First call to TermStore with cache key: [{0}]", key);

                    var session = TaxonomySession.GetTaxonomySession(clientContext);
                    var client = clientContext;

                    if (useDefaultSiteCollectionTermStore == true)
                    {
                        TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Resolving Term Store as useDefaultSiteCollectionTermStore");

                        termStore = session.GetDefaultSiteCollectionTermStore();
                    }
                    else
                    {
                        if (termStoreId.HasValue && termStoreId != default(Guid))
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving Term Store by ID: [{0}]", termStoreId.Value);
                            termStore = session.TermStores.GetById(termStoreId.Value);
                        }
                        else if (!string.IsNullOrEmpty(termStoreName))
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving Term Store by Name: [{0}]", termStoreName);
                            termStore = session.TermStores.GetByName(termStoreName);
                        }
                    }

                    if (termStore != null)
                    {
                        client.Load(termStore, s => s.Id);
                        client.Load(termStore, s => s.Name);

                        client.ExecuteQueryWithTrace();
                    }


                    _storeCache.Add(key, termStore);
                }
                else
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving term store from internal cache with cache key: [{0}]", key);
                }

                return _storeCache[key];
            }
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
