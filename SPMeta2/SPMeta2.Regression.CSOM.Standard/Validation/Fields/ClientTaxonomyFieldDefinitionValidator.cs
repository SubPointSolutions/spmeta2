﻿using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.Fields;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.Exceptions;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Fields
{
    public class ClientTaxonomyFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        #region constructors

        public ClientTaxonomyFieldDefinitionValidator()
        {
            //SkipRequredPropValidation = true;
            //SkipDescriptionPropValidation = true;
        }

        #endregion

        #region properties

        public override Type TargetType
        {
            get
            {
                return typeof(TaxonomyFieldDefinition);
            }
        }

        #endregion

        #region methods

        protected override void CustomFieldTypeValidation(AssertPair<FieldDefinition, Field> assert, Field spObject, FieldDefinition definition)
        {
            var typedObject = spObject.Context.CastTo<TaxonomyField>(spObject);
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

            var typedModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyFieldDefinition>("model", value => value.RequireNotNull());

            var context = typedModelHost.HostClientContext;

            var site = typedModelHost.HostSite;
            var spObjectTmp = GetField(modelHost, definition);
            var spObject = spObjectTmp.Context.CastTo<TaxonomyField>(spObjectTmp);


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

            if (definition.UseDefaultSiteCollectionTermStore.HasValue &&
                definition.UseDefaultSiteCollectionTermStore.Value)
            {
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(context, definition);

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

            if (!string.IsNullOrEmpty(definition.SspName))
            {
                // TODO   
            }
            else
            {
                assert.SkipProperty(m => m.SspName, "SspName is null. Skipping property.");
            }

            // is site collectiongroup
            if (definition.IsSiteCollectionGroup.HasValue && definition.IsSiteCollectionGroup.Value)
            {
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(context, definition);

                TermGroup group = null;

                // cause binding might be only by group AND (termset || term)
                var termSet = TaxonomyFieldModelHandler.LookupTermSet(context, context.Site, termStore, definition);
                var term = TaxonomyFieldModelHandler.LookupTerm(context, termStore, termSet, definition);

                if (termSet != null)
                    group = termSet.Group;
                else if (term != null)
                    group = term.TermSet.Group;

                context.Load(group);
                context.ExecuteQuery();

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
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(context, definition);

                TermGroup group = null;

                // cause binding might be only by group AND (termset || term)
                var termSet = TaxonomyFieldModelHandler.LookupTermSet(context,context.Site, termStore, definition);
                var term = TaxonomyFieldModelHandler.LookupTerm(context, termStore, termSet, definition);

                if (termSet != null)
                    group = termSet.Group;
                else if (term != null)
                    group = term.TermSet.Group;

                context.Load(group);
                context.ExecuteQuery();

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
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(context, definition);

                TermGroup group = null;

                // cause binding might be only by group AND (termset || term)
                var termSet = TaxonomyFieldModelHandler.LookupTermSet(context,context.Site, termStore, definition);
                var term = TaxonomyFieldModelHandler.LookupTerm(context, termStore, termSet, definition);

                if (termSet != null)
                    group = termSet.Group;
                else if (term != null)
                    group = term.TermSet.Group;

                context.Load(group);
                context.ExecuteQuery();

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
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(context, definition);
                var termSet = TaxonomyFieldModelHandler.LookupTermSet(context, termStore, definition);

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

            if (definition.TermId.HasValue)
                assert.ShouldBeEqual(m => m.TermId, o => o.AnchorId);
            else
                assert.SkipProperty(m => m.TermId, "TermId is null. Skipping property.");

            if (!string.IsNullOrEmpty(definition.TermName))
            {
                var termStore = TaxonomyFieldModelHandler.LookupTermStore(context, definition);
                var termSet = TaxonomyFieldModelHandler.LookupTermSet(context, termStore, definition);
                var term = TaxonomyFieldModelHandler.LookupTerm(context, termStore, termSet, definition);

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

        #endregion
    }
}
