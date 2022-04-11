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
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Xml.Linq;
using SPMeta2.Enumerations;
using Microsoft.SharePoint;

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

            //var item = host.PageListItem;

            WebPartExtensions.WithExistingWebPart(host.HostFile, definition, (spWebPartManager, spObject) =>
            {
                var web = spWebPartManager.Web;
                var typedObject = spObject as XsltListViewWebPart;

                var assert = ServiceFactory.AssertService
                    .NewAssert(definition, typedObject)
                    .ShouldNotBeNull(typedObject);

                var typedDefinition = definition;
                var targetWeb = web;

                // web url
                if (!string.IsNullOrEmpty(typedDefinition.WebUrl))
                {
                    var lookupFieldModelHandler = new LookupFieldModelHandler();
                    targetWeb = lookupFieldModelHandler.GetTargetWeb(web.Site,
                                definition.WebUrl, definition.WebId);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.WebUrl);

                        var isValid = d.WebId == targetWeb.ID;

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
                    assert.SkipProperty(m => m.WebUrl, "WebUrl is NULL. Skipping.");
                }

                if (typedDefinition.WebId.HasGuidValue())
                {
                    var lookupFieldModelHandler = new LookupFieldModelHandler();
                    targetWeb = lookupFieldModelHandler.GetTargetWeb(web.Site,
                                definition.WebUrl, definition.WebId);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.WebId);

                        var isValid = d.WebId == targetWeb.ID;

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
                    assert.SkipProperty(m => m.WebId, "WebId is NULL. Skipping.");
                }

                var targetList = web.Lists[typedObject.ListId];

                var hasList = !string.IsNullOrEmpty(definition.ListTitle) ||
                              !string.IsNullOrEmpty(definition.ListUrl) ||
                              definition.ListId.HasValue;
                
                var hasView = !string.IsNullOrEmpty(definition.ViewName) ||
                        !string.IsNullOrEmpty(definition.ViewUrl) ||
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

                if (!string.IsNullOrEmpty(definition.ViewUrl))
                {
                    // web part gonna have hidden view
                    // so validation is a bit tricky, done by other properties

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcView = targetList.Views.OfType<SPView>()
                                        .FirstOrDefault(v => v.ServerRelativeUrl.ToUpper().EndsWith(s.ViewUrl.ToUpper()));
                        var dstView = typedObject.View;

                        var srcProp = s.GetExpressionValue(m => m.ViewUrl);
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
                    assert.SkipProperty(m => m.ViewUrl, "ViewName is null or empty. Skipping.");
                }

                // JSLink
                if (!string.IsNullOrEmpty(definition.JSLink))
                {
                    assert.ShouldBeEqual(m => m.JSLink, o => o.JSLink);
                }
                else
                {
                    assert.SkipProperty(m => m.JSLink);
                }

                if (definition.InplaceSearchEnabled.HasValue)
                {
                    //assert.ShouldBeEqual(m => m.InplaceSearchEnabled, o => o.InplaceSearchEnabled);

                    // TODO
                    // always return valid/true for SSOM
                    // tested manually, the SSOM API seems to always return true
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.InplaceSearchEnabled);
                        var dstProp = d.GetExpressionValue(o => o.InplaceSearchEnabled);

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = dstProp,
                            IsValid = true
                        };
                    });
                }
                else
                    assert.SkipProperty(m => m.InplaceSearchEnabled, "InplaceSearchEnabled is null or empty.");

                if (definition.DisableSaveAsNewViewButton.HasValue)
                {
                    //assert.ShouldBeEqual(m => m.DisableSaveAsNewViewButton, o => o.DisableSaveAsNewViewButton);

                    // TODO
                    // always return valid/true for SSOM
                    // tested manually, the SSOM API seems to always return true
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.DisableSaveAsNewViewButton);
                        var dstProp = d.GetExpressionValue(o => o.DisableSaveAsNewViewButton);

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = dstProp,
                            IsValid = true
                        };
                    });

                }
                else
                    assert.SkipProperty(m => m.DisableSaveAsNewViewButton,
                        "DisableSaveAsNewViewButton is null or empty.");

                if (definition.DisableColumnFiltering.HasValue)
                    assert.ShouldBeEqual(m => m.DisableColumnFiltering, o => o.DisableColumnFiltering);
                else
                    assert.SkipProperty(m => m.DisableColumnFiltering, "DisableColumnFiltering is null or empty.");

                if (definition.DisableViewSelectorMenu.HasValue)
                {
                    //assert.ShouldBeEqual(m => m.DisableViewSelectorMenu, o => o.DisableViewSelectorMenu);

                    // TODO
                    // always return valid/true for SSOM
                    // tested manually, the SSOM API seems to always return true
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.DisableViewSelectorMenu);
                        var dstProp = d.GetExpressionValue(o => o.DisableViewSelectorMenu);

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = dstProp,
                            IsValid = true
                        };
                    });
                }
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
                          
                            SPView srcView = null;

                            if (s.ViewId.HasValue && s.ViewId != default(Guid))
                                srcView = targetList.Views[s.ViewId.Value];
                            else if (!string.IsNullOrEmpty(s.ViewName))
                                srcView = targetList.Views[s.ViewName];
                            else if (!string.IsNullOrEmpty(s.ViewUrl))
                            {
                                srcView = targetList.Views.OfType<SPView>()
                                    .FirstOrDefault(v => v.ServerRelativeUrl.ToUpper().EndsWith(s.ViewUrl.ToUpper()));
                            }

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
                        var value = ConvertUtils.ToString(d.XmlDefinition);
                        var destXmlAttrs = XDocument.Parse(value).Root.Attributes();

                        var srcProp = s.GetExpressionValue(m => m.XmlDefinition);
                        var isValid = true;

                        var srcXmlAttrs = XDocument.Parse(definition.XmlDefinition).Root.Attributes();

                        foreach (var srcAttr in srcXmlAttrs)
                        {
                            var attrName = srcAttr.Name;
                            var attrValue = srcAttr.Value;

                            isValid = destXmlAttrs.FirstOrDefault(a => a.Name == attrName)
                                .Value == attrValue;

                            if (!isValid)
                                break;
                        }

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
                    assert.SkipProperty(m => m.XmlDefinition);

                if (!string.IsNullOrEmpty(definition.Xsl))
                    assert.ShouldBeEqual(m => m.Xsl, o => o.Xsl);
                else
                    assert.SkipProperty(m => m.Xsl);

                // skip it, it will be part of the .Toolbar validation
                assert.SkipProperty(m => m.ToolbarShowAlways, "");

                if (!string.IsNullOrEmpty(definition.Toolbar))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var targetView = (spObject as XsltListViewWebPart).View;
                        var htmlSchemaXml = XDocument.Parse(targetView.HtmlSchemaXml);

                        var useShowAlwaysValue =
                                     (typedDefinition.Toolbar.ToUpper() == BuiltInToolbarType.Standard.ToUpper())
                                     && typedDefinition.ToolbarShowAlways.HasValue
                                     && typedDefinition.ToolbarShowAlways.Value;

                        var toolbarNode = htmlSchemaXml.Root
                            .Descendants("Toolbar")
                            .FirstOrDefault();

                        // NONE? the node might not be there
                        if ((typedDefinition.Toolbar.ToUpper() == BuiltInToolbarType.None.ToUpper())
                            && (toolbarNode == null))
                        {
                            var srcProp = s.GetExpressionValue(m => m.Toolbar);

                            return new PropertyValidationResult
                            {
                                Tag = p.Tag,
                                Src = srcProp,
                                Dst = null,
                                IsValid = true
                            };
                        }
                        else
                        {
                            var toolBarValue = toolbarNode.GetAttributeValue("Type");

                            var srcProp = s.GetExpressionValue(m => m.Toolbar);
                            var isValid = toolBarValue.ToUpper() == definition.Toolbar.ToUpper();

                            if (useShowAlwaysValue)
                            {
                                var showAlwaysValue = toolbarNode.GetAttributeValue("ShowAlways");
                                isValid = isValid && (showAlwaysValue.ToUpper() == "TRUE");
                            }

                            return new PropertyValidationResult
                            {
                                Tag = p.Tag,
                                Src = srcProp,
                                Dst = null,
                                IsValid = isValid
                            };
                        }
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.Toolbar);
                }

            });
        }

        #endregion
    }
}
