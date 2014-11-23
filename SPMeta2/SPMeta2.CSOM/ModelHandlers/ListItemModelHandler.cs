using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

using SPMeta2.ModelHosts;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListItemModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ListItemDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModeHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            var list = listModeHost.HostList;

            DeployInternall(list, listItemModel);
        }

        private void DeployInternall(List list, ListItemDefinition listItemModel)
        {
            if (IsDocumentLibray(list))
            {
                TraceService.Error((int)LogEventId.ModelProvisionCoreCall, "Please use ModuleFileDefinition to deploy files to the document libraries. Throwing SPMeta2NotImplementedException");

                throw new SPMeta2NotImplementedException("Please use ModuleFileDefinition to deploy files to the document libraries");
            }

            ListItem currentItem = null;

            InvokeOnModelEvent<ListItemDefinition, ListItem>(currentItem, ModelEventType.OnUpdating);
            currentItem = EnsureListItem(list, listItemModel);
            InvokeOnModelEvent<ListItemDefinition, ListItem>(currentItem, ModelEventType.OnUpdated);
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var listModeHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            var list = listModeHost.HostList;

            var item = EnsureListItem(list, listItemModel);
            var context = list.Context;

            if (childModelType == typeof(ListItemFieldValueDefinition))
            {
                // naaaaah, just gonna get a new one list item
                // keep it simple and safe, really really really safe with all that SharePoint stuff...
                var currentItem = list.GetItemById(item.Id);

                context.Load(currentItem);
                context.ExecuteQueryWithTrace();

                var listItemPropertyHost = new ListItemFieldValueModelHost
                {
                    CurrentItem = currentItem
                };

                action(listItemPropertyHost);

                currentItem.Update();

                context.ExecuteQueryWithTrace();
            }
        }

        protected ListItem GetListItem(List list, ListItemDefinition definition)
        {
            var items = list.GetItems(CamlQuery.CreateAllItemsQuery());

            var context = list.Context;

            context.Load(items);
            context.ExecuteQueryWithTrace();

            // BIG TODO, don't tell me, I know that
            return items.FirstOrDefault(i => i[BuiltInInternalFieldNames.Title] != null && (i[BuiltInInternalFieldNames.Title].ToString() == definition.Title));
        }

        private ListItem EnsureListItem(List list, ListItemDefinition listItemModel)
        {
            var context = list.Context;
            var currentItem = GetListItem(list, listItemModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentItem,
                ObjectType = typeof(ListItem),
                ObjectDefinition = listItemModel,
                ModelHost = list
            });

            if (currentItem == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list item");

                var newItem = list.AddItem(new ListItemCreationInformation());

                newItem[BuiltInInternalFieldNames.Title] = listItemModel.Title;

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newItem,
                    ObjectType = typeof(ListItem),
                    ObjectDefinition = listItemModel,
                    ModelHost = list
                });

                newItem.Update();

                context.ExecuteQueryWithTrace();

                return newItem;
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list item");

                currentItem[BuiltInInternalFieldNames.Title] = listItemModel.Title;

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentItem,
                    ObjectType = typeof(ListItem),
                    ObjectDefinition = listItemModel,
                    ModelHost = list
                });

                currentItem.Update();

                context.ExecuteQueryWithTrace();

                return currentItem;
            }
        }

        private bool IsDocumentLibray(List list)
        {
            // TODO
            return false;
        }

        #endregion
    }
}
