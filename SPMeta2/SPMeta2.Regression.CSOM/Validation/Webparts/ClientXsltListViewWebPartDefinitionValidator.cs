using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHandlers.Webparts;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHandlers.Fields;

namespace SPMeta2.Regression.CSOM.Validation.Webparts
{
    public class ClientXsltListViewWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        internal class ListBindContext
        {
            public View OriginalView { get; set; }
            public View TargetView { get; set; }

            public Guid? OriginalViewId { get; set; }
            public Guid? TargetViewId { get; set; }

            public Guid ListId { get; set; }
            public string TitleUrl { get; set; }
        }

        private ListBindContext LookupBindContext(ListItemModelHost listItemModelHost, XsltListViewWebPartDefinition wpModel)
        {
            var webId = default(Guid);

            return LookupBindContext(listItemModelHost, wpModel, out webId);
        }

        private ListBindContext LookupBindContext(ListItemModelHost listItemModelHost, XsltListViewWebPartDefinition wpModel,
             out Guid webId)
        {
            var result = new ListBindContext
            {

            };

            var web = listItemModelHost.HostWeb;
            var context = listItemModelHost.HostWeb.Context;

            var list = LookupList(listItemModelHost, wpModel, out webId);

            View view = null;

            if (wpModel.ViewId.HasValue && wpModel.ViewId != default(Guid))
                view = list.Views.GetById(wpModel.ViewId.Value);
            else if (!string.IsNullOrEmpty(wpModel.ViewName))
                view = list.Views.GetByTitle(wpModel.ViewName);
            else if (!string.IsNullOrEmpty(wpModel.ViewUrl))
            {
                var views = list.Views;

                context.Load(views, v => v.Include(r => r.ServerRelativeUrl));
                context.ExecuteQueryWithTrace();

                view = views.ToArray()
                            .FirstOrDefault(v => v.ServerRelativeUrl.ToUpper().EndsWith(wpModel.ViewUrl.ToUpper()));
            }

            context.Load(list, l => l.Id);
            context.Load(list, l => l.DefaultViewUrl);
            context.Load(list, l => l.Title);
            context.Load(list, l => l.DefaultView);

            if (view != null)
            {
                context.Load(view);
                context.ExecuteQueryWithTrace();

                result.OriginalView = list.DefaultView;
                result.OriginalViewId = list.DefaultView.Id;

                result.TargetView = view;
                result.TargetViewId = view.Id;

                result.TitleUrl = view.ServerRelativeUrl;
            }
            else
            {
                context.ExecuteQueryWithTrace();
            }

            result.ListId = list.Id;

            if (wpModel.TitleUrl == null)
            {
                if (string.IsNullOrEmpty(result.TitleUrl))
                    result.TitleUrl = list.DefaultViewUrl;
            }

            return result;
        }

        public override Type TargetType
        {
            get { return typeof(XsltListViewWebPartDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<XsltListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var web = listItemModelHost.HostWeb;
            var context = web.Context;

            var pageItem = listItemModelHost.HostListItem;

            WithExistingWebPart(listItemModelHost.HostFile, definition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);

                var typedDefinition = definition;

                if (typedDefinition.WebId.HasGuidValue())
                {
                    // TODO
                }
                else
                {
                    assert.SkipProperty(m => m.WebId, "WebId is NULL. Skipping.");
                }

                if (!string.IsNullOrEmpty(typedDefinition.WebUrl))
                {
                           var webId = default(Guid);
                           var bindContext = LookupBindContext(listItemModelHost, typedDefinition, out webId);
                    // web id should be the same

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.WebUrl);
                        var isValid = webId == new  Guid(CurrentWebPartXml.GetWebId());

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

                // list
                if (!string.IsNullOrEmpty(definition.ListTitle))
                {
                    var listId = new Guid(CurrentWebPartXml.GetListId());
                    var list = LookupList(listItemModelHost, definition);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ListTitle);
                        var isValid = list.Id == listId;

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
                    assert.SkipProperty(m => m.ListTitle, "ListTitle is null or empty. Skipping.");
                }

                if (!string.IsNullOrEmpty(definition.ListUrl))
                {
                    var listId = new Guid(CurrentWebPartXml.GetListId());
                    var list = LookupList(listItemModelHost, definition);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ListUrl);
                        var isValid = list.Id == listId;

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
                    assert.SkipProperty(m => m.ListUrl, "ListUrl is null or empty. Skipping.");
                }

                if (definition.ListId.HasGuidValue())
                {
                    var listId = new Guid(CurrentWebPartXml.GetListId());
                    var list = LookupList(listItemModelHost, definition);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ListId);
                        var isValid = list.Id == listId;

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
                    assert.SkipProperty(m => m.ListId, "ListId is null or empty. Skipping.");
                }

                // list view
                if (definition.ViewId.HasGuidValue())
                {
                    var list = LookupList(listItemModelHost, definition);
                    var bindContext = LookupBindContext(listItemModelHost, definition);

                    var viewBindingXml = XDocument.Parse(CurrentWebPartXml.GetProperty("XmlDefinition"));
                    var viewId = new Guid(viewBindingXml.Root.GetAttributeValue("Name"));

                    var bindedView = list.Views.GetById(viewId);
                    var targetView = list.Views.GetById(definition.ViewId.Value);

                    context.Load(bindedView, l => l.ViewFields, l => l.ViewQuery, l => l.RowLimit);
                    context.Load(targetView, l => l.ViewFields, l => l.ViewQuery, l => l.RowLimit);

                    context.ExecuteQueryWithTrace();

                    var isValid = false;

                    // these are two different views, just CAML and field count
                    isValid = (bindedView.ViewFields.Count == targetView.ViewFields.Count)
                              && (bindedView.ViewQuery == targetView.ViewQuery)
                              && (bindedView.RowLimit == targetView.RowLimit);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ViewId);

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
                    var list = LookupList(listItemModelHost, definition);
                    var bindContext = LookupBindContext(listItemModelHost, definition);

                    var viewBindingXml = XDocument.Parse(CurrentWebPartXml.GetProperty("XmlDefinition"));
                    var viewId = new Guid(viewBindingXml.Root.GetAttributeValue("Name"));

                    var bindedView = list.Views.GetById(viewId);
                    var targetView = list.Views.GetByTitle(definition.ViewName);

                    context.Load(bindedView, l => l.BaseViewId, l => l.ViewFields, l => l.ViewQuery, l => l.RowLimit, l => l.JSLink);
                    context.Load(targetView, l => l.BaseViewId, l => l.ViewFields, l => l.ViewQuery, l => l.RowLimit, l => l.JSLink);

                    context.ExecuteQueryWithTrace();

                    var isValid = false;

                    // these are two different views, just CAML and field count
                    isValid = (bindedView.ViewFields.Count == targetView.ViewFields.Count)
                              && (bindedView.ViewQuery == targetView.ViewQuery)
                              && (bindedView.RowLimit == targetView.RowLimit)
                              && (bindedView.JSLink == targetView.JSLink)
                              && (bindedView.BaseViewId == targetView.BaseViewId);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ViewName);

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
                    var list = LookupList(listItemModelHost, definition);
                    var bindContext = LookupBindContext(listItemModelHost, definition);

                    var viewBindingXml = XDocument.Parse(CurrentWebPartXml.GetProperty("XmlDefinition"));
                    var viewId = new Guid(viewBindingXml.Root.GetAttributeValue("Name"));

                    var bindedView = list.Views.GetById(viewId);

                    var views = list.Views;

                    context.Load(views, v => v.Include(r => r.ServerRelativeUrl));
                    context.ExecuteQueryWithTrace();

                    var targetView = views.ToArray()
                                .FirstOrDefault(v => v.ServerRelativeUrl.ToUpper().EndsWith(definition.ViewUrl.ToUpper()));

                    context.Load(bindedView, l => l.BaseViewId, l => l.ViewFields, l => l.ViewQuery, l => l.RowLimit, l => l.JSLink);
                    context.Load(targetView, l => l.BaseViewId, l => l.ViewFields, l => l.ViewQuery, l => l.RowLimit, l => l.JSLink);

                    context.ExecuteQueryWithTrace();

                    var isValid = false;

                    // these are two different views, just CAML and field count
                    isValid = (bindedView.ViewFields.Count == targetView.ViewFields.Count)
                              && (bindedView.ViewQuery == targetView.ViewQuery)
                              && (bindedView.RowLimit == targetView.RowLimit)
                              && (bindedView.JSLink == targetView.JSLink)
                              && (bindedView.BaseViewId == targetView.BaseViewId);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ViewUrl);

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
                    assert.SkipProperty(m => m.ViewUrl, "ViewUrl is null or empty. Skipping.");
                }

                // jslink
                if (!string.IsNullOrEmpty(definition.JSLink))
                {
                    var jsLinkValue = CurrentWebPartXml.GetJSLink();

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.JSLink);
                        var isValid = definition.JSLink.ToUpper() == jsLinkValue.ToUpper();

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
                    assert.SkipProperty(m => m.JSLink, "JSLink is null or empty. Skipping.");
                }

                // the rest
                if (definition.CacheXslTimeOut.HasValue)
                {
                    var value = ConvertUtils.ToInt(CurrentWebPartXml.GetProperty("CacheXslTimeOut"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.CacheXslTimeOut);
                        var isValid = definition.CacheXslTimeOut == value;

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
                    assert.SkipProperty(m => m.CacheXslTimeOut, "CacheXslTimeOut is null or empty. Skipping.");

                if (definition.CacheXslStorage.HasValue)
                {
                    var value = ConvertUtils.ToBool(CurrentWebPartXml.GetProperty("CacheXslStorage"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.CacheXslStorage);
                        var isValid = definition.CacheXslStorage == value;

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
                    assert.SkipProperty(m => m.CacheXslStorage, "CacheXslStorage is null or empty. Skipping.");

                if (definition.ShowTimelineIfAvailable.HasValue)
                {
                    var value = ConvertUtils.ToBool(CurrentWebPartXml.GetProperty("ShowTimelineIfAvailable"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ShowTimelineIfAvailable);
                        var isValid = definition.ShowTimelineIfAvailable == value;

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
                    assert.SkipProperty(m => m.ShowTimelineIfAvailable, "ShowTimelineIfAvailable is null or empty. Skipping.");

                if (definition.InplaceSearchEnabled.HasValue)
                {
                    var value = ConvertUtils.ToBool(CurrentWebPartXml.GetProperty("InplaceSearchEnabled"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.InplaceSearchEnabled);
                        var isValid = definition.InplaceSearchEnabled == value;

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
                    assert.SkipProperty(m => m.InplaceSearchEnabled, "InplaceSearchEnabled is null or empty. Skipping.");


                if (definition.DisableSaveAsNewViewButton.HasValue)
                {
                    var value = ConvertUtils.ToBool(CurrentWebPartXml.GetProperty("DisableSaveAsNewViewButton"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.DisableSaveAsNewViewButton);
                        var isValid = definition.DisableSaveAsNewViewButton == value;

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
                    assert.SkipProperty(m => m.DisableSaveAsNewViewButton, "DisableSaveAsNewViewButton is null or empty. Skipping.");


                if (definition.DisableColumnFiltering.HasValue)
                {
                    var value = ConvertUtils.ToBool(CurrentWebPartXml.GetProperty("DisableColumnFiltering"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.DisableColumnFiltering);
                        var isValid = definition.DisableColumnFiltering == value;

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
                    assert.SkipProperty(m => m.DisableColumnFiltering, "DisableColumnFiltering is null or empty. Skipping.");

                if (definition.DisableViewSelectorMenu.HasValue)
                {
                    var value = ConvertUtils.ToBool(CurrentWebPartXml.GetProperty("DisableViewSelectorMenu"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.DisableViewSelectorMenu);
                        var isValid = definition.DisableViewSelectorMenu == value;

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
                    assert.SkipProperty(m => m.DisableViewSelectorMenu, "DisableViewSelectorMenu is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.BaseXsltHashKey))
                {
                    var value = ConvertUtils.ToString(CurrentWebPartXml.GetProperty("BaseXsltHashKey"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.BaseXsltHashKey);
                        var isValid = definition.BaseXsltHashKey == value;

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
                    assert.SkipProperty(m => m.BaseXsltHashKey, "BaseXsltHashKey is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.GhostedXslLink))
                {
                    var value = ConvertUtils.ToString(CurrentWebPartXml.GetProperty("GhostedXslLink"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.GhostedXslLink);
                        var isValid = definition.GhostedXslLink == value;

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
                    assert.SkipProperty(m => m.GhostedXslLink, "GhostedXslLink is null or empty. Skipping.");

                // xsl
                if (!string.IsNullOrEmpty(definition.Xsl))
                {
                    var value = ConvertUtils.ToString(CurrentWebPartXml.GetProperty("Xsl"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.Xsl);
                        var isValid = value
                                        .Replace("\r", string.Empty)
                                        .Replace("\n", string.Empty) ==
                                     definition.Xsl
                                                .Replace("\r", string.Empty)
                                                .Replace("\n", string.Empty);

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
                    assert.SkipProperty(m => m.Xsl, "Xsl is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.XslLink))
                {
                    var value = ConvertUtils.ToString(CurrentWebPartXml.GetProperty("XslLink"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.XslLink);
                        var isValid = definition.XslLink == value;

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
                    assert.SkipProperty(m => m.XslLink, "XslLink is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.GhostedXslLink))
                {
                    var value = ConvertUtils.ToString(CurrentWebPartXml.GetProperty("GhostedXslLink"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.GhostedXslLink);
                        var isValid = definition.GhostedXslLink == value;

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
                    assert.SkipProperty(m => m.GhostedXslLink, "GhostedXslLink is null or empty. Skipping.");

                // xml
                if (!string.IsNullOrEmpty(definition.XmlDefinition))
                {
                    var value = ConvertUtils.ToString(CurrentWebPartXml.GetProperty("XmlDefinition"));
                    var destXmlAttrs = XDocument.Parse(value).Root.Attributes();

                    assert.ShouldBeEqual((p, s, d) =>
                    {
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
                    assert.SkipProperty(m => m.XmlDefinition, "XmlDefinition is null or empty. Skipping.");

                if (!string.IsNullOrEmpty(definition.XmlDefinitionLink))
                {
                    var value = ConvertUtils.ToString(CurrentWebPartXml.GetProperty("XmlDefinitionLink"));

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.XmlDefinitionLink);
                        var isValid = definition.XmlDefinitionLink == value;

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
                    assert.SkipProperty(m => m.XmlDefinitionLink, "XmlDefinitionLink is null or empty. Skipping.");



                // skip it, it will be part of the .Toolbar validation
                assert.SkipProperty(m => m.ToolbarShowAlways, "");

                if (!string.IsNullOrEmpty(definition.Toolbar))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var targetWeb = listItemModelHost.HostWeb;

                        if (typedDefinition.WebId.HasGuidValue() || !string.IsNullOrEmpty(typedDefinition.WebUrl))
                        {
                            targetWeb = new LookupFieldModelHandler()
                                            .GetTargetWeb(this.CurrentClientContext.Site, typedDefinition.WebUrl, typedDefinition.WebId);
                        }

                        var list = XsltListViewWebPartModelHandler.LookupList(targetWeb,
                                        typedDefinition.ListUrl,
                                        typedDefinition.ListTitle,
                                        typedDefinition.WebId);

                        var xmlDefinition = ConvertUtils.ToString(CurrentWebPartXml.GetProperty("XmlDefinition"));
                        var xmlDefinitionDoc = XDocument.Parse(xmlDefinition);

                        var viewId = new Guid(xmlDefinitionDoc.Root.GetAttributeValue("Name"));

                        var hiddenView = list.Views.GetById(viewId);
                        context.Load(hiddenView, v => v.HtmlSchemaXml);
                        context.ExecuteQueryWithTrace();

                        var htmlSchemaXml = XDocument.Parse(hiddenView.HtmlSchemaXml);

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

        private List LookupList(ListItemModelHost listItemModelHost, XsltListViewWebPartDefinition wpModel)
        {
            var webId = default(Guid);

            return LookupList(listItemModelHost, wpModel, out webId);
        }

        private List LookupList(ListItemModelHost listItemModelHost, 
            XsltListViewWebPartDefinition wpModel
            , out Guid webId)
        {
            var web = listItemModelHost.HostWeb;
            var context = web.Context;

            if (wpModel.WebId.HasGuidValue() || !string.IsNullOrEmpty(wpModel.WebUrl))
            {
                web = new LookupFieldModelHandler()
                                .GetTargetWeb(listItemModelHost.HostClientContext.Site,
                                        wpModel.WebUrl, wpModel.WebId);

                webId = web.Id;
            }
            else
            {
                context.Load(web);
                context.Load(web, w => w.Id);

                context.ExecuteQueryWithTrace();


                webId = web.Id;
            }

            List list = null;

            if (!string.IsNullOrEmpty(wpModel.ListUrl))
            {
                list = web.QueryAndGetListByUrl(wpModel.ListUrl);
            }
            else if (!string.IsNullOrEmpty(wpModel.ListTitle))
            {
                list = web.Lists.GetByTitle(wpModel.ListTitle);
            }
            else if (wpModel.ListId != default(Guid))
            {
                list = web.Lists.GetById(wpModel.ListId.Value);
            }
            else
            {
                throw new SPMeta2Exception("ListUrl, ListTitle or ListId should be defined.");
            }

            return list;
        }
    }
}
