using System;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
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

        protected void ProcessView(object modelHost, SPList targetList, ListViewDefinition viewModel)
        {
            var currentView = targetList.Views.FindByName(viewModel.Title);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentView,
                ObjectType = typeof(SPView),
                ObjectDefinition = viewModel,
                ModelHost = modelHost
            });

            if (currentView == null)
            {
                var viewFields = new StringCollection();
                viewFields.AddRange(viewModel.Fields.ToArray());

                // TODO, handle personal view creation
                currentView = targetList.Views.Add(viewModel.Title, viewFields,
                            viewModel.Query,
                            (uint)viewModel.RowLimit,
                            viewModel.IsPaged,
                            viewModel.IsDefault);
            }

            // viewModel.InvokeOnDeployingModelEvents<ListViewDefinition, SPView>(currentView);

            // if any fields specified, overwrite
            if (viewModel.Fields.Any())
            {
                currentView.ViewFields.DeleteAll();

                foreach (var viewField in viewModel.Fields)
                    currentView.ViewFields.Add(viewField);
            }

            // viewModel.InvokeOnModelUpdatedEvents<ListViewDefinition, SPView>(currentView);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentView,
                ObjectType = typeof(SPView),
                ObjectDefinition = viewModel,
                ModelHost = modelHost
            });

            currentView.Update();
        }

        #endregion
    }
}
