using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.Extensions;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListViewModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ListViewDefinition); }
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

                // TODO, handle personal view creation
                currentView = targetList.Views.Add(
                            string.IsNullOrEmpty(listViewModel.Url) ? listViewModel.Title : GetSafeViewUrl(listViewModel.Url),
                            viewFields,
                            listViewModel.Query,
                            (uint)listViewModel.RowLimit,
                            listViewModel.IsPaged,
                            listViewModel.IsDefault);

                currentView.Title = listViewModel.Title;
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list view");
            }

            // viewModel.InvokeOnDeployingModelEvents<ListViewDefinition, SPView>(currentView);

            // if any fields specified, overwrite
            if (listViewModel.Fields.Any())
            {
                currentView.ViewFields.DeleteAll();

                foreach (var viewField in listViewModel.Fields)
                    currentView.ViewFields.Add(viewField);
            }

            currentView.Hidden = listViewModel.Hidden;
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

            // viewModel.InvokeOnModelUpdatedEvents<ListViewDefinition, SPView>(currentView);

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

        protected SPContentTypeId LookupListContentTypeByName(SPList targetList, string name)
        {
            var targetContentType = targetList.ContentTypes
                   .OfType<SPContentType>()
                   .FirstOrDefault(ct => ct.Name.ToUpper() == name.ToUpper());

            if (targetContentType == null)
                throw new SPMeta2Exception(string.Format("Cannot find content type by name ['{0}'] in list: [{1}]",
                    name, targetList.Title));

            return targetContentType.Id;
        }

        protected SPContentTypeId LookupListContentTypeById(SPList targetList, string contentTypeId)
        {
            return new SPContentTypeId(contentTypeId);
        }

        #endregion
    }
}
