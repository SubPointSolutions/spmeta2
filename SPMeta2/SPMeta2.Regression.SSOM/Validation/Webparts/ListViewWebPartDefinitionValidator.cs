using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.SSOM.ModelHandlers.Webparts;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Webparts
{
    public class ListViewWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        public ListViewWebPartDefinitionValidator()
        {
            DisableTitleUrlValidation = true;
        }


        #region properties

        public override Type TargetType
        {
            get { return typeof(ListViewWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);


            // web specific validation
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListViewWebPartDefinition>("model", value => value.RequireNotNull());

            WebPartExtensions.WithExistingWebPart(host.HostFile, definition, (spWebPartManager, spObject) =>
            {
                var web = spWebPartManager.Web;
                var typedObject = spObject as ListViewWebPart;

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
                        var dstView = targetList.Views[new Guid(typedObject.ViewGuid)];

                        var srcProp = s.GetExpressionValue(m => m.ViewId);
                        var dstProp = d.GetExpressionValue(o => o.ViewGuid);

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
                        var dstView = targetList.Views[new Guid(typedObject.ViewGuid)];

                        var srcProp = s.GetExpressionValue(m => m.ViewName);
                        var dstProp = d.GetExpressionValue(o => o.ViewGuid);

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
                      
                        var dstView = targetList.Views[new Guid(typedObject.ViewGuid)];

                        var srcProp = s.GetExpressionValue(m => m.ViewUrl);
                        var dstProp = d.GetExpressionValue(o => o.ViewGuid);

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

                //  title link
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


                // skip it, it will be part of the .Toolbar validation
                assert.SkipProperty(m => m.ToolbarShowAlways, "");

                if (!string.IsNullOrEmpty(definition.Toolbar))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var list = XsltListViewWebPartModelHandler.GetTargetList(targetWeb,
                                        typedDefinition.ListTitle,
                                        typedDefinition.ListUrl,
                                        typedDefinition.ListId);

                        var targetView = list.Views[Guid.Parse((spObject as ListViewWebPart).ViewGuid)];
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
