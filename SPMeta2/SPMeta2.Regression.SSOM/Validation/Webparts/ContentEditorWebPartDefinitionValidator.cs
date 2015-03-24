using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
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
    public class ContentEditorWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentEditorWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);

            // web specific validation
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentEditorWebPartDefinition>("model", value => value.RequireNotNull());

            var item = host.PageListItem;

            WebPartExtensions.WithExistingWebPart(item, definition, (spWebPartManager, spObject) =>
            {
                var web = spWebPartManager.Web;
                var typedObject = spObject as ContentEditorWebPart;

                var assert = ServiceFactory.AssertService
                    .NewAssert(definition, typedObject)
                    .ShouldNotBeNull(typedObject);

                if (!string.IsNullOrEmpty(definition.ContentLink))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ContentLink);
                        var dstProp = d.GetExpressionValue(o => o.ContentLink);

                        var srcUrl = srcProp.Value as string;
                        var dstUrl = dstProp.Value as string;

                        var isValid = false;

                        if (definition.ContentLink.Contains("~sitecollection"))
                        {
                            var siteCollectionUrl = web.Site.ServerRelativeUrl == "/" ? string.Empty : web.Site.ServerRelativeUrl;

                            isValid = srcUrl.Replace("~sitecollection", siteCollectionUrl) == dstUrl;
                        }
                        else if (definition.ContentLink.Contains("~site"))
                        {
                            var siteCollectionUrl = web.ServerRelativeUrl == "/" ? string.Empty : web.ServerRelativeUrl;

                            isValid = srcUrl.Replace("~site", siteCollectionUrl) == dstUrl;
                        }
                        else
                        {
                            isValid = srcUrl == dstUrl;
                        }

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = dstProp,
                            IsValid = isValid
                        };
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.ContentLink, "ContentLink is null or empty. Skipping.");
                }

                if (!string.IsNullOrEmpty(definition.Content))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.Content);
                        var dstProp = d.GetExpressionValue(o => o.Content);

                        var srcUrl = srcProp.Value as string;
                        var dstUrl = dstProp.Value as XmlElement;

                        var isValid = dstUrl.InnerText == srcUrl;

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = dstProp,
                            IsValid = isValid
                        };
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.Content, "Content is null or empty. Skipping.");
                }

            });

            // content editor specific validation
        }

        #endregion
    }
}
