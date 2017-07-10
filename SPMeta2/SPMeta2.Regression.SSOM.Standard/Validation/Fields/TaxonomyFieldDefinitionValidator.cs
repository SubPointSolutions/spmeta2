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

        protected SPSite ExtractSite(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb.Site;

            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList.ParentWeb.Site;

            throw new ArgumentException("modelHost");
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<SSOMModelHostBase>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyFieldDefinition>("model", value => value.RequireNotNull());

            var site = ExtractSite(typedModelHost);
            var spObject = GetField(modelHost, definition) as TaxonomyField;

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.IsMulti, o => o.AllowMultipleValues);

            if (definition.CreateValuesInEditForm.HasValue)
                assert.ShouldBeEqual(m => m.CreateValuesInEditForm, o => o.CreateValuesInEditForm);
            else
                assert.SkipProperty(m => m.CreateValuesInEditForm, "CreateValuesInEditForm is null. Skipping property.");

            if (definition.Open.HasValue)
                assert.ShouldBeEqual(m => m.Open, o => o.Open);
            else
                assert.SkipProperty(m => m.Open, "Open is null. Skipping property.");

            if (definition.IsPathRendered.HasValue)
                assert.ShouldBeEqual(m => m.IsPathRendered, o => o.IsPathRendered);
            else
                assert.SkipProperty(m => m.IsPathRendered, "IsPathRendered is null. Skipping property.");

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

            if (definition.UseDefaultSiteCollectionTermStore == true)
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
                assert.SkipProperty(m => m.UseDefaultSiteCollectionTermStore, "UseDefaultSiteCollectionTermStore is null. Skipping property.");
            }

            // is site collectiongroup
            if (definition.IsSiteCollectionGroup.HasValue && definition.IsSiteCollectionGroup.Value)
            {
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(site, definition);

                Group group = null;

                // cause binding might be only by group AND (termset || term)
                var termSet = TaxonomyFieldModelHandler.LookupTermSet(site, termStore, definition);
                var term = TaxonomyFieldModelHandler.LookupTerm(site, termStore, definition);

                if (termSet != null)
                    group = termSet.Group;
                else if (term != null)
                    group = term.TermSet.Group;

                var isValid = group.IsSiteCollectionGroup;

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IsSiteCollectionGroup);

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
                assert.SkipProperty(m => m.IsSiteCollectionGroup, "IsSiteCollectionGroup is null. Skipping property.");
            }

            // term group
            if (definition.TermGroupId.HasValue)
            {
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(site, definition);

                Group group = null;

                // cause binding might be only by group AND (termset || term)
                var termSet = TaxonomyFieldModelHandler.LookupTermSet(site, termStore, definition);
                var term = TaxonomyFieldModelHandler.LookupTerm(site, termStore, definition);

                if (termSet != null)
                    group = termSet.Group;
                else if (term != null)
                    group = term.TermSet.Group;

                var isValid = group.Id == definition.TermGroupId;

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.TermGroupId);

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
                assert.SkipProperty(m => m.TermGroupId, "TermGroupId is null. Skipping property.");
            }

            if (!string.IsNullOrEmpty(definition.TermGroupName))
            {
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(site, definition);

                Group group = null;

                // cause binding might be only by group AND (termset || term)
                var termSet = TaxonomyFieldModelHandler.LookupTermSet(site, termStore, definition);
                var term = TaxonomyFieldModelHandler.LookupTerm(site, termStore, definition);

                if (termSet != null)
                    group = termSet.Group;
                else if (term != null)
                    group = term.TermSet.Group;

                var isValid = group.Name == definition.TermGroupName;

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.TermGroupName);

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
                assert.SkipProperty(m => m.TermGroupName, "TermGroupName is null. Skipping property.");
            }

            // term set
            if (definition.TermSetId.HasValue)
                assert.ShouldBeEqual(m => m.TermSetId, o => o.TermSetId);
            else
                assert.SkipProperty(m => m.TermSetId, "TermSetId is null. Skipping property.");

            if (!string.IsNullOrEmpty(definition.TermSetName))
            {
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(site, definition);
                var termSet = TaxonomyFieldModelHandler.LookupTermSet(site, termStore, definition);

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
                var term = TaxonomyFieldModelHandler.LookupTerm(site, termStore, definition);

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

            if (definition.UserCreated.HasValue)
                assert.ShouldBeEqual(m => m.UserCreated, o => o.UserCreated);
            else
                assert.SkipProperty(m => m.UserCreated, "UserCreated is null. Skipping property.");
        }
    }
}
