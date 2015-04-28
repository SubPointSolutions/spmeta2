using System;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHandlers.Webparts;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Webparts
{
    public class ClientContentEditorWebpartDefinitionValidator : WebPartDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(ContentEditorWebPartDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentEditorWebPartDefinition>("model", value => value.RequireNotNull());

            var pageItem = listItemModelHost.HostListItem;

            WithWithExistingWebPart(pageItem, definition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);


                if (!string.IsNullOrEmpty(definition.Content))
                {
                    var value = CurrentWebPartXml.GetContentEditorWebPartProperty("Content");

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.Content);
                        var isValid = definition.Content == value;

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
                    assert.SkipProperty(m => m.Content, "Content is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.ContentLink))
                {
                    var value = CurrentWebPartXml.GetContentEditorWebPartProperty("ContentLink");
                    var defValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                    {
                        Context = listItemModelHost.HostClientContext,
                        Value = definition.ContentLink
                    }).Value;

                    var isValid = defValue.ToUpper() == value.ToUpper();

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ContentLink);

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
                    assert.SkipProperty(m => m.ContentLink, "ContentLink is null or empty. Skipping.");

            });
        }
    }
}
