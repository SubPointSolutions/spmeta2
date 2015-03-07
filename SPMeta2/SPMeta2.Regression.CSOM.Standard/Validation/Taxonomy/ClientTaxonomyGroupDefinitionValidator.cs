using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Taxonomy
{
    public class TaxonomyGroupDefinitionValidator : TaxonomyGroupModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var termStoreModelHost = modelHost.WithAssertAndCast<TermStoreModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyTermGroupDefinition>("model", value => value.RequireNotNull());

            var spObject = FindGroup(termStoreModelHost, definition);

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);

            if (definition.IsSiteCollectionGroup)
            {
                assert.SkipProperty(m => m.Name, "IsSiteCollectionGroup is TRUE. Skipping Name property validation.");

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IsSiteCollectionGroup);
                    var group = FindSiteCollectionGroup(termStoreModelHost, definition);

                    var isValid = group.IsSiteCollectionGroup;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                
            }
            else
            {
                assert.SkipProperty(m => m.IsSiteCollectionGroup, "IsSiteCollectionGroup is false. Skipping property.");
                assert.ShouldBeEqual(m => m.Name, o => o.Name);
            }

            if (definition.Id.HasValue)
            {
                assert.ShouldBeEqual(m => m.Id, o => o.Id);
            }
            else
            {
                assert.SkipProperty(m => m.Id, "Id is null. Skipping property.");
            }
        }
    }
}
