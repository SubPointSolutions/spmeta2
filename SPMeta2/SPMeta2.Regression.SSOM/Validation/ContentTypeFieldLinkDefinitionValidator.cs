using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;


namespace SPMeta2.Regression.SSOM.Validation
{
    public class ContentTypeFieldLinkDefinitionValidator : ContentTypeFieldLinkModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var spModel = modelHost.WithAssertAndCast<SPContentType>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var spObject = spModel.FieldLinks[definition.FieldId];

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldBeEqual(m => m.FieldId, o => o.Id);

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
