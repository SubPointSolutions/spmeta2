using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

                if (definition.CacheXslStorage.HasValue)
                    assert.ShouldBeEqual(m => m.CacheXslStorage, o => o.CacheXslStorage);
                else
                    assert.SkipProperty(m => m.CacheXslStorage, "CacheXslStorage is null or empty.");

                if (definition.CacheXslTimeOut.HasValue)
                    assert.ShouldBeEqual(m => m.CacheXslTimeOut, o => o.CacheXslTimeOut);
                else
                    assert.SkipProperty(m => m.CacheXslTimeOut, "CacheXslTimeOut is null or empty.");

                if (definition.ShowTimelineIfAvailable.HasValue)
                    assert.ShouldBeEqual(m => m.ShowTimelineIfAvailable, o => o.ShowTimelineIfAvailable);
                else
                    assert.SkipProperty(m => m.ShowTimelineIfAvailable, "ShowTimelineIfAvailable is null or empty.");

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
                    // web part gonna have hidden view
                    // so validation is a bit tricky, done by other properties

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcView = targetList.Views[s.ViewId.Value];
                        var dstView = typedObject.View;

                        var srcProp = s.GetExpressionValue(m => m.ViewId);
                        var dstProp = d.GetExpressionValue(o => o.View);

                        var isValid = srcView.ViewFields.Count == dstView.ViewFields.Count
                                      && srcView.Query == dstView.Query;

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
                    assert.SkipProperty(m => m.ViewId, "ViewId is null or empty. Skipping.");
                }

                if (!string.IsNullOrEmpty(definition.ViewName))
                {
                    // web part gonna have hidden view
                    // so validation is a bit tricky, done by other properties

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcView = targetList.Views[s.ViewName];
                        var dstView = typedObject.View;

                        var srcProp = s.GetExpressionValue(m => m.ViewName);
                        var dstProp = d.GetExpressionValue(o => o.View);

                        var isValid = srcView.ViewFields.Count == dstView.ViewFields.Count
                                      && srcView.Query == dstView.Query;

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
                    assert.SkipProperty(m => m.ViewName, "ViewName is null or empty. Skipping.");
                }

                // JSLink
                assert.ShouldBeEqual(m => m.JSLink, o => o.JSLink);

                if (definition.InplaceSearchEnabled.HasValue)
                    assert.ShouldBeEqual(m => m.InplaceSearchEnabled, o => o.InplaceSearchEnabled);
                else
                    assert.SkipProperty(m => m.InplaceSearchEnabled, "InplaceSearchEnabled is null or empty.");

                if (definition.DisableSaveAsNewViewButton.HasValue)
                    assert.ShouldBeEqual(m => m.DisableSaveAsNewViewButton, o => o.DisableSaveAsNewViewButton);
                else
                    assert.SkipProperty(m => m.DisableSaveAsNewViewButton, "DisableSaveAsNewViewButton is null or empty.");

                if (definition.DisableColumnFiltering.HasValue)
                    assert.ShouldBeEqual(m => m.DisableColumnFiltering, o => o.DisableColumnFiltering);
                else
                    assert.SkipProperty(m => m.DisableColumnFiltering, "DisableColumnFiltering is null or empty.");

                if (definition.DisableViewSelectorMenu.HasValue)
                    assert.ShouldBeEqual(m => m.DisableViewSelectorMenu, o => o.DisableViewSelectorMenu);
                else
                    assert.SkipProperty(m => m.DisableViewSelectorMenu, "DisableViewSelectorMenu is null or empty.");

                // title link
                if (string.IsNullOrEmpty(definition.TitleUrl))
                {
                    // list?
                    if (hasView)
                    {
                        assert.ShouldBeEqual((p, s, d) =>
                        {
                            var srcProp = s.GetExpressionValue(m => m.TitleUrl);
                            var srcView = string.IsNullOrEmpty(s.ViewName) ?
                                targetList.Views[s.ViewId.Value] :
                                targetList.Views[s.ViewName];

                            return new PropertyValidationResult
                            {
                                Tag = p.Tag,
                                Src = srcProp,
                                Dst = null,
                                IsValid = srcView.ServerRelativeUrl.StartsWith(typedObject.TitleUrl)
                            };
                        });
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

                if (!string.IsNullOrEmpty(definition.XslLink))
                    assert.ShouldBeEqual(m => m.XslLink, o => o.XslLink);
                else
                    assert.SkipProperty(m => m.XslLink);

                if (!string.IsNullOrEmpty(definition.XmlDefinitionLink))
                    assert.ShouldBeEqual(m => m.XmlDefinitionLink, o => o.XmlDefinitionLink);
                else
                    assert.SkipProperty(m => m.XmlDefinitionLink);

                if (!string.IsNullOrEmpty(definition.GhostedXslLink))
                    assert.ShouldBeEqual(m => m.GhostedXslLink, o => o.GhostedXslLink);
                else
                    assert.SkipProperty(m => m.GhostedXslLink);

                if (!string.IsNullOrEmpty(definition.BaseXsltHashKey))
                    assert.ShouldBeEqual(m => m.BaseXsltHashKey, o => o.BaseXsltHashKey);
                else
                    assert.SkipProperty(m => m.BaseXsltHashKey);


                if (!string.IsNullOrEmpty(definition.XmlDefinition))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.XmlDefinition);

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = null,
                            IsValid = d.XmlDefinition.Contains("BaseViewID=\"2\"")
                        };
                    });
                }
                else
                    assert.SkipProperty(m => m.XmlDefinition);

                if (!string.IsNullOrEmpty(definition.Xsl))
                    assert.ShouldBeEqual(m => m.Xsl, o => o.Xsl);
                else
                    assert.SkipProperty(m => m.Xsl);

            });
        }

        #endregion
    }
}
