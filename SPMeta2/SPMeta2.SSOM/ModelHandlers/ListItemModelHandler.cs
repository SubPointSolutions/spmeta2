using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListItemModelHandler : SSOMModelHandlerBase
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
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            DeployInternall(list, listItemModel);
        }

        private void DeployInternall(SPList list, ListItemDefinition listItemModel)
        {
            if (IsDocumentLibray(list))
            {
                throw new NotImplementedException("Please use ModuleFileDefinition to deploy files to the document libraries");
            }

            EnsureListItem(list, listItemModel);
        }

        protected SPListItem GetListItem(SPList list, ListItemDefinition listItemModel)
        {
            // TODO, lazy to query
            // BIG TODO, don't tell me, I know that

            return list.Items
                            .OfType<SPListItem>()
                            .FirstOrDefault(i => i.Title == listItemModel.Title);
        }

        private SPListItem EnsureListItem(SPList list, ListItemDefinition listItemModel)
        {
            var currentItem = GetListItem(list, listItemModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentItem,
                ObjectType = typeof(SPListItem),
                ObjectDefinition = listItemModel,
                ModelHost = list
            });

            if (currentItem == null)
            {
                var newItem = list.AddItem();

                newItem["Title"] = listItemModel.Title;

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newItem,
                    ObjectType = typeof(SPListItem),
                    ObjectDefinition = listItemModel,
                    ModelHost = list
                });

                newItem.Update();

                return newItem;
            }
            else
            {
                currentItem["Title"] = listItemModel.Title;

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentItem,
                    ObjectType = typeof(SPListItem),
                    ObjectDefinition = listItemModel,
                    ModelHost = list
                });

                currentItem.Update();

                return currentItem;
            }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var item = EnsureListItem(list, listItemModel);

            if (childModelType == typeof(ListItemFieldValueDefinition))
            {
                // naaaaah, just gonna get a new one list item
                // keep it simple and safe, really really really safe with all that SharePoint stuff...
                var currentItem = list.GetItemById(item.ID);

                action(currentItem);

                currentItem.Update();
            }
        }


        private bool IsDocumentLibray(SPList list)
        {
            // TODO

            return false;
        }

        #endregion
    }
}
