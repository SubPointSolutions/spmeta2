using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Taxonomy
{
    public class ClientTaxonomyStoreDefinitionValidator : TaxonomyTermStoreModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyTermStoreDefinition>("model", value => value.RequireNotNull());

            var spObject = FindTermStore(siteModelHost, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject);

            // TODO

        }
    }
}
