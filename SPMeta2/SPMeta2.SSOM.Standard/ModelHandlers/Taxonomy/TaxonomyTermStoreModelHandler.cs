using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Taxonomy;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Taxonomy
{
    public class TaxonomyTermStoreModelHandler : SSOMModelHandlerBase
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
            var siteModelHost = model.WithAssertAndCast<SiteModelHost>("model", value => value.RequireNotNull());
            var termStoreModel = model.WithAssertAndCast<TaxonomyStoreDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;

            var session = new TaxonomySession(site);
            TermStore termStore = null;


            if (termStoreModel.Id.HasValue)
                termStore = session.TermStores[termStoreModel.Id.Value];
            else if (!string.IsNullOrEmpty(termStoreModel.Name))
                termStore = session.TermStores[termStoreModel.Name];

            action(new TermStoreModelHost
            {
                HostTermStore = termStore
            });
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var site = model.WithAssertAndCast<SiteModelHost>("model", value => value.RequireNotNull());
            var termStoreModel = model.WithAssertAndCast<TaxonomyStoreDefinition>("model", value => value.RequireNotNull());

        }

        #endregion
    }
}
