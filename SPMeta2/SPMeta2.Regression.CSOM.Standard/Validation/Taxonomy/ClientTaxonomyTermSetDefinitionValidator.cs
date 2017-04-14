using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Taxonomy
{
    public class ClientTaxonomyTermSetDefinitionValidator : TaxonomyTermSetModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var termGroupModelHost = modelHost.WithAssertAndCast<TermGroupModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyTermSetDefinition>("model", value => value.RequireNotNull());

            var spObject = FindTermSet(termGroupModelHost.HostGroup, definition);
            var context = termGroupModelHost.HostClientContext;

            if (spObject == null && IsSharePointOnlineContext(context))
            {
                TryRetryService.TryWithRetry(() =>
                {
                    spObject = FindTermSet(termGroupModelHost.HostGroup, definition);
                    return spObject != null;
                });
            }

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Name, o => o.Name);
            //.ShouldBeEqual(m => m.IsAvailableForTagging, o => o.IsAvailableForTagging)
            //.ShouldBeEqual(m => m.IsOpenForTermCreation, o => o.IsOpenForTermCreation);

            assert.SkipProperty(m => m.LCID, "LCID is not accessible from OM. Should be alright while provision.");

            if (definition.IsAvailableForTagging.HasValue)
                assert.ShouldBeEqual(m => m.IsAvailableForTagging, o => o.IsAvailableForTagging);
            else
                assert.SkipProperty(m => m.IsAvailableForTagging, "IsAvailableForTagging is null. Skipping property.");

            if (definition.IsOpenForTermCreation.HasValue)
                assert.ShouldBeEqual(m => m.IsOpenForTermCreation, o => o.IsOpenForTermCreation);
            else
                assert.SkipProperty(m => m.IsOpenForTermCreation, "IsOpenForTermCreation is null. Skipping property.");

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description, "Description is null. Skipping property.");

            if (definition.Id.HasValue)
            {
                assert.ShouldBeEqual(m => m.Id, o => o.Id);
            }
            else
            {
                assert.SkipProperty(m => m.Id, "Id is null. Skipping property.");
            }


            if (!string.IsNullOrEmpty(definition.CustomSortOrder))
                assert.ShouldBeEqual(m => m.CustomSortOrder, o => o.CustomSortOrder);
            else
                assert.SkipProperty(m => m.CustomSortOrder);


            if (!string.IsNullOrEmpty(definition.Contact))
                assert.ShouldBeEqual(m => m.Contact, o => o.Contact);
            else
                assert.SkipProperty(m => m.Contact);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.CustomProperties);

                var isValid = true;

                // missed props, or too much
                // should be equal on the first provision
                if (s.CustomProperties.Count != d.CustomProperties.Count)
                {
                    isValid = false;
                }

                // per prop
                foreach (var customProp in s.CustomProperties)
                {
                    if (!d.CustomProperties.ContainsKey(customProp.Name))
                    {
                        isValid = false;
                        break;
                    }

                    if (d.CustomProperties[customProp.Name] != customProp.Value)
                    {
                        isValid = false;
                        break;
                    }
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    // Dst = dstProp,
                    IsValid = isValid
                };
            });
        }
    }
}
