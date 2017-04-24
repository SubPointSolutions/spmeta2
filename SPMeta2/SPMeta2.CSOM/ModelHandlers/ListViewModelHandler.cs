using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.SharePoint.Client;

using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListViewModelHandler : CSOMModelHandlerBase
    {
        #region constructors

        public ListViewModelHandler()
        {
            ListViewScopeTypesConvertService = ServiceContainer.Instance.GetService<ListViewScopeTypesConvertService>();
        }

        #endregion

        #region methods

        public ListViewScopeTypesConvertService ListViewScopeTypesConvertService { get; set; }

        #endregion

        #region methods

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());

            var web = listModelHost.HostWeb;
            var list = listModelHost.HostList;

            var listViewDefinition = model as ListViewDefinition;
            var context = web.Context;

            if (typeof(WebPartDefinitionBase).IsAssignableFrom(childModelType)
                                || childModelType == typeof(DeleteWebPartsDefinition))
            {
                var targetView = FindView(list, listViewDefinition);
                string serverRelativeFileUrl;

                Folder targetFolder = null;

                if (list.BaseType == BaseType.DocumentLibrary)
                {
                    targetFolder = FolderModelHandler.GetLibraryFolder(list.RootFolder, "Forms");
                }

                if (targetView != null)
                    serverRelativeFileUrl = targetView.ServerRelativeUrl;
                else
                {


                    context.Load(list.RootFolder);
                    context.ExecuteQueryWithTrace();

                    //  maybe forms files?
                    // they aren't views, but files

                    if (list.BaseType == BaseType.DocumentLibrary)
                    {
                        serverRelativeFileUrl = UrlUtility.CombineUrl(new[]
                        {
                            list.RootFolder.ServerRelativeUrl, 
                            "Forms",
                            listViewDefinition.Url
                        });
                    }
                    else
                    {
                        serverRelativeFileUrl = UrlUtility.CombineUrl(new[]
                        {
                            list.RootFolder.ServerRelativeUrl, 
                            listViewDefinition.Url
                        });
                    }
                }

                var file = web.GetFileByServerRelativeUrl(serverRelativeFileUrl);
                context.Load(file);
                context.ExecuteQueryWithTrace();



                var listItemHost = ModelHostBase.Inherit<ListItemModelHost>(listModelHost, itemHost =>
                {
                    itemHost.HostFolder = targetFolder;
                    //itemHost.HostListItem = folderModelHost.CurrentListItem;
                    itemHost.HostFile = file;

                    itemHost.HostList = list;
                });

                action(listItemHost);
            }
            else
            {
                action(listModelHost);
            }
        }

        protected string GetSafeViewUrl(string url)
        {
            return Regex.Replace(url, ".aspx", string.Empty, RegexOptions.IgnoreCase);
        }


        protected View FindView(List list, ListViewDefinition listViewModel)
        {
            // lookup by title
            var currentView = FindViewByTitle(list.Views, listViewModel.Title);

            // lookup by URL match
            if (currentView == null && !string.IsNullOrEmpty(listViewModel.Url))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving view by URL: [{0}]", listViewModel.Url);

                var safeUrl = listViewModel.Url.ToUpper();

                foreach (var view in list.Views)
                {
                    if (view.ServerRelativeUrl.ToUpper().EndsWith(safeUrl))
                    {
                        return view;
                    }
                }

                return null;
            }

            return currentView;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listMOdelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listViewModel = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

            var list = listMOdelHost.HostList;

            var currentView = FindView(list, listViewModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentView,
                ObjectType = typeof(View),
                ObjectDefinition = listViewModel,
                ModelHost = modelHost
            });

            if (currentView == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list view");

                var newView = new ViewCreationInformation
                {
                    Title = string.IsNullOrEmpty(listViewModel.Url) ? listViewModel.Title : GetSafeViewUrl(listViewModel.Url),
                    RowLimit = (uint)listViewModel.RowLimit,
                    SetAsDefaultView = listViewModel.IsDefault,
                    Paged = listViewModel.IsPaged
                };

                if (!string.IsNullOrEmpty(listViewModel.Query))
                    newView.Query = listViewModel.Query;

                if (listViewModel.Fields != null && listViewModel.Fields.Any())
                    newView.ViewFields = listViewModel.Fields.ToArray();

                if (!string.IsNullOrEmpty(listViewModel.Type))
                {
                    newView.ViewTypeKind = (ViewType)Enum.Parse(typeof(ViewType),
                        string.IsNullOrEmpty(listViewModel.Type) ? BuiltInViewType.Html : listViewModel.Type);
                }

                // nasty hack

                // The provision of calendars is not working properly #935
                // https://github.com/SubPointSolutions/spmeta2/issues/935
                if (listViewModel.Types.Count() > 0)
                {
                    ViewType? finalType = null;

                    foreach (var type in listViewModel.Types)
                    {
                        var tmpViewType = (ViewType)Enum.Parse(typeof(ViewType), type);

                        if (finalType == null)
                            finalType = tmpViewType;
                        else
                            finalType = finalType | tmpViewType;
                    }

                    newView.ViewTypeKind = finalType.Value;
                }

                currentView = list.Views.Add(newView);

                MapListViewProperties(list, currentView, listViewModel);

                currentView.Update();

                list.Context.ExecuteQueryWithTrace();
                currentView = FindView(list, listViewModel);

                list.Context.Load(currentView);
                list.Context.ExecuteQueryWithTrace();

            }
            else
            {
                list.Context.Load(currentView);
                list.Context.ExecuteQueryWithTrace();

                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list view");
                MapListViewProperties(list, currentView, listViewModel);
            }

            ProcessLocalization(currentView, listViewModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentView,
                ObjectType = typeof(View),
                ObjectDefinition = listViewModel,
                ModelHost = modelHost
            });

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling currentView.Update()");
            currentView.Update();

            list.Context.ExecuteQueryWithTrace();
        }

        public virtual void MapListViewProperties(List list, View listView, ListViewDefinition definition)
        {
            if (definition.RowLimit > 0)
                listView.RowLimit = (uint)definition.RowLimit;

            if (definition.MobileDefaultView.HasValue)
                listView.MobileDefaultView = definition.MobileDefaultView.Value;

            listView.DefaultView = definition.IsDefault;
            listView.Paged = definition.IsPaged;

            if (definition.IncludeRootFolder.HasValue)
                listView.IncludeRootFolder = definition.IncludeRootFolder.Value;

            if (!string.IsNullOrEmpty(definition.Query))
                listView.ViewQuery = definition.Query;

            if (definition.Fields != null && definition.Fields.Any())
            {
                listView.ViewFields.RemoveAll();

                foreach (var f in definition.Fields)
                    listView.ViewFields.Add(f);
            }

            if (!string.IsNullOrEmpty(definition.ViewData))
                listView.ViewData = definition.ViewData;

            if (!string.IsNullOrEmpty(definition.ContentTypeName))
                listView.ContentTypeId = LookupListContentTypeByName(list, definition.ContentTypeName);

            if (!string.IsNullOrEmpty(definition.ContentTypeId))
                listView.ContentTypeId = LookupListContentTypeById(list, definition.ContentTypeId);

#if !NET35
            if (!string.IsNullOrEmpty(definition.JSLink))
                listView.JSLink = definition.JSLink;
#endif

            if (definition.DefaultViewForContentType.HasValue)
                listView.DefaultViewForContentType = definition.DefaultViewForContentType.Value;

            // There is no value in setting Aggregations if AggregationsStatus is not to "On"
            if (!string.IsNullOrEmpty(definition.AggregationsStatus) && definition.AggregationsStatus == "On")
            {
                if (!string.IsNullOrEmpty(definition.Aggregations))
                    listView.Aggregations = definition.Aggregations;

                listView.AggregationsStatus = definition.AggregationsStatus;
            }

            listView.Hidden = definition.Hidden;

            if (!string.IsNullOrEmpty(definition.Scope))
            {
                var scopeValue = ListViewScopeTypesConvertService.NormilizeValueToCSOMType(definition.Scope);

                listView.Scope = (ViewScope)Enum.Parse(
                    typeof(ViewScope), scopeValue);
            }

            listView.Title = definition.Title;
        }

        protected ContentTypeId LookupListContentTypeByName(List targetList, string name)
        {
            if (!targetList.IsPropertyAvailable("ContentTypes"))
            {
                targetList.Context.Load(targetList, l => l.ContentTypes);
                targetList.Context.ExecuteQueryWithTrace();
            }

            var targetContentType = targetList.ContentTypes.FindByName(name);

            if (targetContentType == null)
                throw new SPMeta2Exception(string.Format("Cannot find content type by name ['{0}'] in list: [{1}]",
                    name, targetList.Title));

            return targetContentType.Id;
        }

        protected ContentTypeId LookupListContentTypeById(List targetList, string contentTypeId)
        {
            var context = targetList.Context;

            // lookup list content type?
            var result = targetList.ContentTypes.GetById(contentTypeId);
            context.ExecuteQueryWithTrace();

            if (result.ServerObjectIsNull == true)
            {
                result = targetList.ParentWeb.ContentTypes.GetById(contentTypeId);
                context.ExecuteQueryWithTrace();
            }

            // lookup site content type (BuiltInContentTypeId.RootOfList)

            if (result.ServerObjectIsNull == true)
                throw new SPMeta2Exception(string.Format("Cannot find content type by id ['{0}'] in list: [{1}]",
                    contentTypeId, targetList.Title));

            context.Load(result);
            context.ExecuteQueryWithTrace();

            return result.Id;
        }

        protected View FindViewByTitle(IEnumerable<View> viewCollection, string listViewTitle)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving view by Title: [{0}]", listViewTitle);

            foreach (var view in viewCollection)
            {
                if (String.Compare(view.Title, listViewTitle, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return view;
                }
            }

            return null;
        }

        #endregion
        protected virtual void ProcessLocalization(View obj, ListViewDefinition definition)
        {
            ProcessGenericLocalization(obj, new Dictionary<string, List<ValueForUICulture>>
            {
                { "TitleResource", definition.TitleResource }
            });
        }

        public override Type TargetType
        {
            get { return typeof(ListViewDefinition); }
        }
    }
}
