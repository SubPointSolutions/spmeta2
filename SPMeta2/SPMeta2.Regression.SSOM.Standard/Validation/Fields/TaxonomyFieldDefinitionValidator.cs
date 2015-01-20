using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.Fields;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Fields
{
    public class TaxonomyFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(TaxonomyFieldDefinition); }
        }

        protected override void CustomFieldTypeValidation(AssertPair<FieldDefinition, SPField> assert, SPField spObject, FieldDefinition definition)
        {
            var typedObject = spObject as TaxonomyField;
            var typedDefinition = definition as TaxonomyFieldDefinition;

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.FieldType);
                var isValid = typedDefinition.IsMulti
                    ? typedObject.TypeAsString == "TaxonomyFieldTypeMulti"
                    : typedObject.TypeAsString == "TaxonomyFieldType";

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyFieldDefinition>("model", value => value.RequireNotNull());

            var site = typedModelHost.HostSite;
            var spObject = GetField(modelHost, definition) as TaxonomyField;


            var assert = ServiceFactory.AssertService
                            .NewAssert(definition, spObject)
                                  .ShouldNotBeNull(spObject)
                                  .ShouldBeEqual(m => m.IsMulti, o => o.AllowMultipleValues);

            // SSP
            if (definition.SspId.HasValue)
                assert.ShouldBeEqual(m => m.SspId, o => o.SspId);
            else
                assert.SkipProperty(m => m.SspId, "SspId is null. Skipping property.");

            if (!string.IsNullOrEmpty(definition.SspName))
            {
                // TODO   
            }
            else
            {
                assert.SkipProperty(m => m.SspName, "SspName is null. Skipping property.");
            }

            if (definition.UseDefaultSiteCollectionTermStore.HasValue)
            {
                var taxSession = new TaxonomySession(site);
                var termStore = taxSession.DefaultSiteCollectionTermStore;

                var isValid = termStore.Id == spObject.SspId;

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.UseDefaultSiteCollectionTermStore);

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
                assert.SkipProperty(m => m.TermSetName, "UseDefaultSiteCollectionTermStore is null. Skipping property.");
            }

            // term set
            if (definition.TermSetId.HasValue)
                assert.ShouldBeEqual(m => m.TermSetId, o => o.TermSetId);
            else
                assert.SkipProperty(m => m.TermSetId, "TermSetId is null. Skipping property.");

            if (!string.IsNullOrEmpty(definition.TermSetName))
            {
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(site, definition);
                var termSet = TaxonomyFieldModelHandler.LookupTermSet(termStore, definition);

                var isValid = spObject.TermSetId == termSet.Id;

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.TermSetName);

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
                assert.SkipProperty(m => m.TermSetName, "TermSetName is null. Skipping property.");
            }

            /// term

            if (definition.TermId.HasValue)
                assert.ShouldBeEqual(m => m.TermId, o => o.AnchorId);
            else
                assert.SkipProperty(m => m.TermId, "TermId is null. Skipping property.");

            if (!string.IsNullOrEmpty(definition.TermName))
            {
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(site, definition);
                var term = TaxonomyFieldModelHandler.LookupTerm(termStore, definition);

                var isValid = spObject.AnchorId == term.Id;

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.TermName);

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
                assert.SkipProperty(m => m.TermName, "TermName is null. Skipping property.");
            }

            // etc
            assert.SkipProperty(m => m.TermLCID, "TermLCID. Skipping property.");
            assert.SkipProperty(m => m.TermSetLCID, "TermSetLCID. Skipping property.");

        }
    }
}
