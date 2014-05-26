using System;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.Extensions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListViewModelHandler : ModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ListViewDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<SPList>("modelHost", value => value.RequireNotNull());
            var listViewModel = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

            ProcessView(list, listViewModel);
        }

        protected void ProcessView(SPList targetList, ListViewDefinition viewModel)
        {
            var currentView = targetList.Views.FindByName(viewModel.Title);

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

            currentView.Update();
        }

        #endregion
    }
}
