using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;

using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.SSOM.Extensions;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Enumerations;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListViewModelHandler : SSOMModelHandlerBase
    {
        #region constructors

        public ListViewModelHandler()
        {
            ListViewScopeTypesConvertService = ServiceContainer.Instance.GetService<ListViewScopeTypesConvertService>();
        }

        #endregion

        #region methods

        public ListViewScopeTypesConvertService ListViewScopeTypesConvertService { get; set; }

        public override Type TargetType
        {
            get { return typeof(ListViewDefinition); }
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var web = list.ParentWeb;

            if (typeof(WebPartDefinitionBase).IsAssignableFrom(childModelType))
            {
                var listViewDefinition = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());
                var currentView = FindView(list, listViewDefinition);

                string serverRelativeFileUrl;
                if (currentView != null)
                    serverRelativeFileUrl = currentView.ServerRelativeUrl;
                else
                {
                    //  maybe forms files?
                    // they aren't views, but files

                    if (list.BaseType == SPBaseType.DocumentLibrary)
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

                var targetFile = web.GetFile(serverRelativeFileUrl);

                using (var webPartManager = targetFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
                {
                    var webpartPageHost = new WebpartPageModelHost
                    {
                        HostFile = targetFile,
                        PageListItem = targetFile.Item,
                        SPLimitedWebPartManager = webPartManager
                    };

                    action(webpartPageHost);
                }
            }
            else
            {
                action(modelHost);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listViewModel = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            ProcessView(modelHost, list, listViewModel);
        }

        protected string GetSafeViewUrl(string url)
        {
            return Regex.Replace(url, ".aspx", string.Empty, RegexOptions.IgnoreCase);
        }

        protected SPView FindView(SPList targetList, ListViewDefinition listViewModel)
        {
            // lookup by title
            var currentView = targetList.Views.FindByName(listViewModel.Title);

            // lookup by URL match
            if (currentView == null && !string.IsNullOrEmpty(listViewModel.Url))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving view by URL: [{0}]", listViewModel.Url);

                var safeUrl = listViewModel.Url.ToUpper();
                currentView = targetList.Views.OfType<SPView>().FirstOrDefault(w => w.Url.ToUpper().EndsWith(safeUrl));
            }

            return currentView;
        }

        protected void ProcessView(object modelHost, SPList targetList, ListViewDefinition listViewModel)
        {
            var currentView = FindView(targetList, listViewModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentView,
                ObjectType = typeof(SPView),
                ObjectDefinition = listViewModel,
                ModelHost = modelHost
            });

            if (currentView == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list view");

                var viewFields = new StringCollection();
                viewFields.AddRange(listViewModel.Fields.ToArray());

                var isPersonalView = false;
                var viewType = (SPViewCollection.SPViewType)Enum.Parse(typeof(SPViewCollection.SPViewType),
                    string.IsNullOrEmpty(listViewModel.Type) ? BuiltInViewType.Html : listViewModel.Type);

                // TODO, handle personal view creation
                currentView = targetList.Views.Add(
                            string.IsNullOrEmpty(listViewModel.Url) ? listViewModel.Title : GetSafeViewUrl(listViewModel.Url),
                            viewFields,
                            listViewModel.Query,
                            (uint)listViewModel.RowLimit,
                            listViewModel.IsPaged,
                            listViewModel.IsDefault,
                            viewType,
                            isPersonalView);

                currentView.Title = listViewModel.Title;
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list view");
            }

            // viewModel.InvokeOnDeployingModelEvents<ListViewDefinition, SPView>(currentView);

            MapProperties(targetList, currentView, listViewModel);

            // viewModel.InvokeOnModelUpdatedEvents<ListViewDefinition, SPView>(currentView);

            ProcessLocalization(currentView, listViewModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentView,
                ObjectType = typeof(SPView),
                ObjectDefinition = listViewModel,
                ModelHost = modelHost
            });

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling currentView.Update()");
            currentView.Update();
        }

        private void MapProperties(SPList targetList, SPView currentView, ListViewDefinition listViewModel)
        {
            // if any fields specified, overwrite
            if (listViewModel.Fields.Any())
            {
                currentView.ViewFields.DeleteAll();

                foreach (var viewField in listViewModel.Fields)
                    currentView.ViewFields.Add(viewField);
            }

            if (!string.IsNullOrEmpty(listViewModel.ViewData))
                currentView.ViewData = listViewModel.ViewData;

            if (!string.IsNullOrEmpty(listViewModel.Scope))
            {
                var scopeValue = ListViewScopeTypesConvertService.NormilizeValueToSSOMType(listViewModel.Scope);

                currentView.Scope = (SPViewScope)Enum.Parse(
                    typeof(SPViewScope), scopeValue);
            }

            // There is no value in setting Aggregations if AggregationsStatus is not to "On"
            if (!string.IsNullOrEmpty(listViewModel.AggregationsStatus) && listViewModel.AggregationsStatus == "On")
            {
                currentView.AggregationsStatus = listViewModel.AggregationsStatus;

                if (!string.IsNullOrEmpty(listViewModel.Aggregations))
                    currentView.Aggregations = listViewModel.Aggregations;
            }

            currentView.Hidden = listViewModel.Hidden;
            if (listViewModel.InlineEdit.HasValue)
            {
                currentView.InlineEdit = listViewModel.InlineEdit.Value.ToString(CultureInfo.InvariantCulture);
            }

            currentView.Title = listViewModel.Title;

            currentView.RowLimit = (uint)listViewModel.RowLimit;
            currentView.DefaultView = listViewModel.IsDefault;
            currentView.Paged = listViewModel.IsPaged;

#if !NET35
            if (!string.IsNullOrEmpty(listViewModel.JSLink))
                currentView.JSLink = listViewModel.JSLink;
#endif

            if (!string.IsNullOrEmpty(listViewModel.Query))
                currentView.Query = listViewModel.Query;

            if (listViewModel.DefaultViewForContentType.HasValue)
                currentView.DefaultViewForContentType = listViewModel.DefaultViewForContentType.Value;

            if (!string.IsNullOrEmpty(listViewModel.ContentTypeName))
                currentView.ContentTypeId = LookupListContentTypeByName(targetList, listViewModel.ContentTypeName);

            if (!string.IsNullOrEmpty(listViewModel.ContentTypeId))
                currentView.ContentTypeId = LookupListContentTypeById(targetList, listViewModel.ContentTypeId);

            if (listViewModel.ViewStyleId.HasValue)
            {
                var viewStyle = targetList.ParentWeb.ViewStyles.StyleByID(listViewModel.ViewStyleId.Value);
                currentView.ApplyStyle(viewStyle);
            }

            if (listViewModel.TabularView.HasValue)
            {
                currentView.TabularView = listViewModel.TabularView.Value;
            }
        }

        protected SPContentTypeId LookupListContentTypeByName(SPList targetList, string name)
        {
            var targetContentType = targetList.ContentTypes
                   .OfType<SPContentType>()
                   .FirstOrDefault(ct => String.Equals(ct.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (targetContentType == null)
                throw new SPMeta2Exception(string.Format("Cannot find content type by name ['{0}'] in list: [{1}]",
                    name, targetList.Title));

            return targetContentType.Id;
        }

        protected SPContentTypeId LookupListContentTypeById(SPList targetList, string contentTypeId)
        {
            return new SPContentTypeId(contentTypeId);
        }

        protected virtual void ProcessLocalization(SPView obj, ListViewDefinition definition)
        {

            if (definition.TitleResource.Any())
            {
#if !NET35
                foreach (var locValue in definition.TitleResource)
                    LocalizationService.ProcessUserResource(obj, obj.TitleResource, locValue);
#endif
            }

        }

        #endregion
    }
}
