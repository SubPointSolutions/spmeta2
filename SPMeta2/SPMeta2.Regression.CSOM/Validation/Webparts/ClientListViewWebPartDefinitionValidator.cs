using System;
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

            var pageItem = listItemModelHost.HostListItem;
            var context = pageItem.Context;


            WithWithExistingWebPart(pageItem, definition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);

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

                    context.ExecuteQuery();

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

                    context.ExecuteQuery();

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
