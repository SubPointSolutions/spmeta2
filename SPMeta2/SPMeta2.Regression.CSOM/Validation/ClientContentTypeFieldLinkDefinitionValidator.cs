using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientContentTypeFieldLinkDefinitionValidator : ContentTypeFieldLinkModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var modelHostContext = modelHost.WithAssertAndCast<ModelHostContext>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var site = modelHostContext.Site;
            var contentType = modelHostContext.ContentType;

            var context = site.Context;

            var spObject = FindExistingFieldLink(contentType, definition);

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);

            if (!string.IsNullOrEmpty(definition.FieldInternalName))
                assert.ShouldBeEqual(m => m.FieldInternalName, o => o.Name);
            else
                assert.SkipProperty(m => m.FieldInternalName, "FieldInternalName is NULL or empty. Skipping.");

            if (definition.FieldId.HasGuidValue())
                assert.ShouldBeEqual(m => m.FieldId, o => o.Id);
            else
                assert.SkipProperty(m => m.FieldId, "FieldId is NULL. Skipping.");

            if (!string.IsNullOrEmpty(definition.DisplayName))
            {
                // DisplayName is notsupported by CSOM yet
                // https://officespdev.uservoice.com/forums/224641-general/suggestions/7024931-enhance-fieldlink-class-with-additional-properties
                // assert.ShouldBeEqual(m => m.DisplayName, o => o.DisplayName);

                assert.SkipProperty(m => m.DisplayName, "DisplayName is not supported by CSOM yet. Skipping.");
            }
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
