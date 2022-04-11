﻿using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;


namespace SPMeta2.Regression.SSOM.Validation
{
    public class ContentTypeFieldLinkDefinitionValidator : ContentTypeFieldLinkModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelhost = modelHost.WithAssertAndCast<ContentTypeModelHost>("model", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var spModel = typedModelhost.HostContentType;

            var spObject = FindExistingFieldLink(spModel, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                       .ShouldNotBeNull(spObject);

            //.ShouldBeEqual(m => m.FieldId, o => o.Id);

            if (!string.IsNullOrEmpty(definition.FieldInternalName))
                assert.ShouldBeEqual(m => m.FieldInternalName, o => o.Name);
            else
                assert.SkipProperty(m => m.FieldInternalName, "FieldInternalName is NULL or empty. Skipping.");

            if (definition.FieldId.HasGuidValue())
                assert.ShouldBeEqual(m => m.FieldId, o => o.Id);
            else
                assert.SkipProperty(m => m.FieldId, "FieldId is NULL. Skipping.");


            if (!string.IsNullOrEmpty(definition.DisplayName))
                assert.ShouldBeEqual(m => m.DisplayName, o => o.DisplayName);
            else
                assert.SkipProperty(m => m.DisplayName, "DisplayName is NULL or empty. Skipping.");

            if (definition.Required.HasValue)
                assert.ShouldBeEqual(m => m.Required, o => o.Required);
            else
                assert.SkipProperty(m => m.Required, "Required is NULL. Skipping.");

            if (definition.Hidden.HasValue)
                assert.ShouldBeEqual(m => m.Hidden, o => o.Hidden);
            else
                assert.SkipProperty(m => m.Hidden, "Hidden is NULL. Skipping.");
        }
    }
}
