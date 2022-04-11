using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHandlers.Fields;
using SPMeta2.CSOM.ModelHandlers.Webparts;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Webparts
{
    public class ClientListViewWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(ListViewWebPartDefinition); }
        }

        private List LookupList(ListItemModelHost listItemModelHost, ListViewWebPartDefinition wpModel)
        {
            var web = listItemModelHost.HostWeb;
            var context = listItemModelHost.HostWeb.Context;

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

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var context = listItemModelHost.HostClientContext;

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
                    // TODO
                }
                else
                {
                    assert.SkipProperty(m => m.WebUrl, "WebUrl is NULL. Skipping.");
                }

                // list
                if (!string.IsNullOrEmpty(definition.ListTitle))
                {
                    var listId = new Guid(CurrentWebPartXml.GetListViewWebPartProperty("ListId"));
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
                    var listId = new Guid(CurrentWebPartXml.GetListViewWebPartProperty("ListId"));
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
                    var listId = new Guid(CurrentWebPartXml.GetListViewWebPartProperty("ListId"));
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

                if (definition.ViewId.HasGuidValue())
                {
                    var list = LookupList(listItemModelHost, definition);
                    var bindContext = LookupBindContext(listItemModelHost, definition);

                    var viewBindingXml = XDocument.Parse(CurrentWebPartXml.GetListViewWebPartProperty("ListViewXml"));
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

                    var viewBindingXml = XDocument.Parse(CurrentWebPartXml.GetListViewWebPartProperty("ListViewXml"));
                    var viewId = new Guid(viewBindingXml.Root.GetAttributeValue("Name"));

                    var bindedView = list.Views.GetById(viewId);
                    var targetView = list.Views.GetByTitle(definition.ViewName);

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

                    var viewBindingXml = XDocument.Parse(CurrentWebPartXml.GetListViewWebPartProperty("ListViewXml"));
                    var viewId = new Guid(viewBindingXml.Root.GetAttributeValue("Name"));

                    var bindedView = list.Views.GetById(viewId);
                    var targetView = list.Views.GetByTitle(definition.ViewName);

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
                                            .GetTargetWeb(this.CurrentClientContext.Site, 
                                            typedDefinition.WebUrl, 
                                            typedDefinition.WebId,
                                            modelHost);
                        }

                        var list = XsltListViewWebPartModelHandler.LookupList(targetWeb,
                                        typedDefinition.ListUrl,
                                        typedDefinition.ListTitle,
                                        typedDefinition.WebId);

                        var xmlDefinition = ConvertUtils.ToString(
                            CurrentWebPartXml.Root.Descendants(
                                ((XNamespace)"http://schemas.microsoft.com/WebPart/v2/ListView") + "ListViewXml")
                            .First().Value);

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

        private ClientXsltListViewWebPartDefinitionValidator.ListBindContext
            LookupBindContext(ListItemModelHost listItemModelHost, ListViewWebPartDefinition wpModel)
        {
            var result = new ClientXsltListViewWebPartDefinitionValidator.ListBindContext
            {

            };

            var web = listItemModelHost.HostWeb;
            var context = listItemModelHost.HostWeb.Context;

            var list = LookupList(listItemModelHost, wpModel);

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

    }
}
