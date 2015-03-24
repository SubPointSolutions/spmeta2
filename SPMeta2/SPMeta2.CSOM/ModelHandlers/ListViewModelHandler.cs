using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListViewModelHandler : CSOMModelHandlerBase
    {
        #region properties

        #endregion

        #region methods

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
            var list = modelHost.WithAssertAndCast<List>("modelHost", value => value.RequireNotNull());
            var listViewModel = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

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

                if (listViewModel.Fields != null && listViewModel.Fields.Count() > 0)
                    newView.ViewFields = listViewModel.Fields.ToArray();

                currentView = list.Views.Add(newView);


                if (!string.IsNullOrEmpty(listViewModel.ContentTypeName))
                    currentView.ContentTypeId = LookupListContentTypeByName(list, listViewModel.ContentTypeName);

                if (!string.IsNullOrEmpty(listViewModel.ContentTypeId))
                    currentView.ContentTypeId = LookupListContentTypeById(list, listViewModel.ContentTypeId);

                currentView.JSLink = listViewModel.JSLink;

                if (listViewModel.DefaultViewForContentType.HasValue)
                    currentView.DefaultViewForContentType = listViewModel.DefaultViewForContentType.Value;

                currentView.Hidden = listViewModel.Hidden;

                currentView.Title = listViewModel.Title;
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

                currentView.Hidden = listViewModel.Hidden;

                currentView.RowLimit = (uint)listViewModel.RowLimit;
                currentView.DefaultView = listViewModel.IsDefault;
                currentView.Paged = listViewModel.IsPaged;

                if (!string.IsNullOrEmpty(listViewModel.Query))
                    currentView.ViewQuery = listViewModel.Query;

                if (listViewModel.Fields != null && listViewModel.Fields.Count() > 0)
                {
                    currentView.ViewFields.RemoveAll();

                    foreach (var f in listViewModel.Fields)
                        currentView.ViewFields.Add(f);
                }

                if (!string.IsNullOrEmpty(listViewModel.ContentTypeName))
                    currentView.ContentTypeId = LookupListContentTypeByName(list, listViewModel.ContentTypeName);

                if (!string.IsNullOrEmpty(listViewModel.ContentTypeId))
                    currentView.ContentTypeId = LookupListContentTypeById(list, listViewModel.ContentTypeId);

                if (!string.IsNullOrEmpty(listViewModel.JSLink))
                    currentView.JSLink = listViewModel.JSLink;

                if (listViewModel.DefaultViewForContentType.HasValue)
                    currentView.DefaultViewForContentType = listViewModel.DefaultViewForContentType.Value;

                currentView.Title = listViewModel.Title;
            }

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

        protected ContentTypeId LookupListContentTypeByName(List targetList, string name)
        {
            if (!targetList.IsPropertyAvailable("ContentTypes"))
            {
                targetList.Context.Load(targetList, l => l.ContentTypes);
                targetList.Context.ExecuteQuery();
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
                if (System.String.Compare(view.Title, listViewTitle, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return view;
                }
            }

            return null;
        }

        #endregion

        public override Type TargetType
        {
            get { return typeof(ListViewDefinition); }
        }
    }
}
