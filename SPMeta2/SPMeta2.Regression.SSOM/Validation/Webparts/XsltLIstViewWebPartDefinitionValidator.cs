using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Webparts
{
    public class XsltListViewWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(XsltListViewWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);


            // web specific validation
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<XsltListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var item = host.PageListItem;

            WebPartExtensions.WithExistingWebPart(item, definition, (spWebPartManager, spObject) =>
            {
                var web = spWebPartManager.Web;
                var typedObject = spObject as XsltListViewWebPart;

                var assert = ServiceFactory.AssertService
                    .NewAssert(definition, typedObject)
                    .ShouldNotBeNull(typedObject);

                var targetList = web.Lists[typedObject.ListId];

                var hasList = !string.IsNullOrEmpty(definition.ListTitle) ||
                              !string.IsNullOrEmpty(definition.ListUrl) ||
                              definition.ListId.HasValue;
                var hasView = !string.IsNullOrEmpty(definition.ViewName) ||
                              definition.ViewId.HasValue; ;

                // list
                if (!string.IsNullOrEmpty(definition.ListTitle))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ListTitle);
                        var dstProp = d.GetExpressionValue(o => o.ListId);

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = null,
                            IsValid = targetList.ID == (Guid)dstProp.Value
                        };
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.ListTitle, "ListTitle is null or empty. Skipping.");
                }

                if (!string.IsNullOrEmpty(definition.ListUrl))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ListUrl);
                        var dstProp = d.GetExpressionValue(o => o.ListId);

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = null,
                            IsValid = targetList.ID == (Guid)dstProp.Value
                        };
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.ListUrl, "ListUrl is null or empty. Skipping.");
                }

                if (definition.ListId.HasValue && definition.ListId != default(Guid))
                {
                    assert.ShouldBeEqual(m => m.ListId, o => o.ListId);
                }
                else
                {
                    assert.SkipProperty(m => m.ListId, "ListId is null or empty. Skipping.");
                }

                // view
                if (definition.ViewId.HasValue)
                {

                }
                else
                {
                    assert.SkipProperty(m => m.ViewId, "ViewId is null or empty. Skipping.");
                }

                if (!string.IsNullOrEmpty(definition.ViewName))
                {

                }
                else
                {
                    assert.SkipProperty(m => m.ViewName, "ViewName is null or empty. Skipping.");
                }

                // JSLink
                assert.ShouldBeEqual(m => m.JSLink, o => o.JSLink);

                // title link
                if (definition.TitleUrl == null)
                {
                    // list?
                    if (hasView)
                    {

                    }
                    else if (hasList)
                    {
                        assert.ShouldBeEqual((p, s, d) =>
                        {
                            var srcProp = s.GetExpressionValue(m => m.TitleUrl);

                            return new PropertyValidationResult
                            {
                                Tag = p.Tag,
                                Src = srcProp,
                                Dst = null,
                                IsValid = targetList.DefaultViewUrl.StartsWith(typedObject.TitleUrl)
                            };
                        });
                    }
                }
                else
                {
                    assert.ShouldBeEqual(m => m.TitleUrl, o => o.TitleUrl);
                }
            });
        }

        #endregion
    }
}
