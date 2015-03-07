using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Taxonomy
{
    public class ClientTaxonomyTermLabelDefinitionValidator : TaxonomyTermLabelModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var termModelHost = modelHost.WithAssertAndCast<TermModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyTermLabelDefinition>("model", value => value.RequireNotNull());

            var spObject = FindLabelInTerm(termModelHost.HostTerm, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .ShouldBeEqual(m => m.Name, o => o.Value)
                                 .ShouldBeEqual(m => m.LCID, o => o.Language)
                                 .ShouldBeEqual(m => m.IsDefault, o => o.IsDefaultForLanguage);
        }
    }
}
