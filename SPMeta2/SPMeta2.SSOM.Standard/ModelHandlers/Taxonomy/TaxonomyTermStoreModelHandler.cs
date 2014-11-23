using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHosts;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Taxonomy
{
    public class TaxonomyTermStoreModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TaxonomyTermStoreDefinition); }
        }

        #endregion

        #region methods

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

        protected TermStore FindTermStore(SiteModelHost siteModelHost, TaxonomyTermStoreDefinition termStoreModel)
        {
            var site = siteModelHost.HostSite;

            var session = new TaxonomySession(site);
            TermStore termStore = null;

            if (termStoreModel.UseDefaultSiteCollectionTermStore == true)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Resolving Term Store as useDefaultSiteCollectionTermStore");
                termStore = session.DefaultSiteCollectionTermStore;
            }
            else if (termStoreModel.Id.HasValue && termStoreModel.Id != default(Guid))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving Term Store by ID: [{0}]", termStoreModel.Id);
                termStore = session.TermStores[termStoreModel.Id.Value];
            }
            else if (!string.IsNullOrEmpty(termStoreModel.Name))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving Term Store by Name: [{0}]", termStoreModel.Name);
                termStore = session.TermStores[termStoreModel.Name];
            }


            return termStore;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("model", value => value.RequireNotNull());
            var termStoreModel = model.WithAssertAndCast<TaxonomyTermStoreDefinition>("model", value => value.RequireNotNull());

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
