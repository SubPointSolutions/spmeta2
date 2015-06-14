using System;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHandlers.Webparts;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Webparts
{
    public class PageViewerWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(PageViewerWebPartDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);


            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PageViewerWebPartDefinition>("model", value => value.RequireNotNull());

            var pageItem = listItemModelHost.HostListItem;

            WithWithExistingWebPart(pageItem, definition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);

                if (!string.IsNullOrEmpty(definition.ContentLink))
                {
                    var value = CurrentWebPartXml.GetPageViewerWebPartProperty("ContentLink");

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ContentLink);
                        var isValid = definition.ContentLink == value;

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
                    assert.SkipProperty(m => m.ContentLink, "ContentLink is null or empty. Skipping.");
                }

                if (!string.IsNullOrEmpty(definition.SourceType))
                {
                    var value = CurrentWebPartXml.GetPageViewerWebPartProperty("SourceType");

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.SourceType);
                        var isValid = definition.SourceType == value;

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
                    assert.SkipProperty(m => m.SourceType, "SourceType is null or empty. Skipping.");
                }
            });
        }
    }
}
