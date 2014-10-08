using System;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy
{
    public class TaxonomyTermStoreModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TaxonomyStoreDefinition); }
        }

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("model", value => value.RequireNotNull());
            var termStoreModel = model.WithAssertAndCast<TaxonomyStoreDefinition>("model", value => value.RequireNotNull());

            var termStore = FindTermStore(siteModelHost, termStoreModel);

            action(new TermStoreModelHost
            {
                HostTermStore = termStore
            });

            termStore.CommitAll();
        }

        protected TermStore FindTermStore(SiteModelHost siteModelHost, TaxonomyStoreDefinition termStoreModel)
        {
            var site = siteModelHost.HostSite;

            var session = TaxonomySession.GetTaxonomySession(siteModelHost.HostClientContext);
            var client = session.Context;

            TermStore termStore = null;

            if (termStoreModel.Id.HasValue && termStoreModel.Id != default(Guid))
                termStore = session.TermStores.GetById(termStoreModel.Id.Value);
            else if (!string.IsNullOrEmpty(termStoreModel.Name))
                termStore = session.TermStores.GetByName(termStoreModel.Name);

            if (termStore != null)
            {
                client.Load(termStore, s => s.Id);
                client.Load(termStore, s => s.Name);

                client.ExecuteQuery();
            }

            return termStore;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("model", value => value.RequireNotNull());
            var termStoreModel = model.WithAssertAndCast<TaxonomyStoreDefinition>("model", value => value.RequireNotNull());

            var termStore = FindTermStore(siteModelHost, termStoreModel);

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
