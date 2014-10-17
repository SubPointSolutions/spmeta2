using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListViewModelHandler : CSOMModelHandlerBase
    {
        #region properties

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<List>("modelHost", value => value.RequireNotNull());
            var listViewModel = model.WithAssertAndCast<ListViewDefinition>("model", value => value.RequireNotNull());

            var currentView = FindViewByTitle(list.Views, listViewModel.Title);

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
                var newView = new ViewCreationInformation
                {
                    Title = listViewModel.Title,
                    RowLimit = (uint)listViewModel.RowLimit,
                    SetAsDefaultView = listViewModel.IsDefault,
                    Paged = listViewModel.IsPaged
                };

                if (!string.IsNullOrEmpty(listViewModel.Query))
                    newView.Query = listViewModel.Query;

                if (listViewModel.Fields != null && listViewModel.Fields.Count() > 0)
                    newView.ViewFields = listViewModel.Fields.ToArray();

                currentView = list.Views.Add(newView);
            }
            else
            {
                currentView.Title = listViewModel.Title;
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

            currentView.Update();
            list.Context.ExecuteQuery();
        }

        protected View FindViewByTitle(IEnumerable<View> viewCollection, string listViewTitle)
        {
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
