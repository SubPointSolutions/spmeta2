﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
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
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            if (modelHost is ListModelHost)
            {
                var list = (modelHost as ListModelHost).HostList;
                var rootFolder = (modelHost as ListModelHost).HostList.RootFolder;

                DeployInternall(list, rootFolder, listItemModel);
            }
            else if (modelHost is FolderModelHost)
            {
                // suppose it is a list, ir must be
                var list = (modelHost as FolderModelHost).CurrentList;
                var rootFolder = (modelHost as FolderModelHost).CurrentListItem.Folder;

                DeployInternall(list, rootFolder, listItemModel);
            }
            else
            {
                throw new SPMeta2UnsupportedModelHostException("modeHost should be either ListModelHost or FolderModelHost");
            }
        }

        private void DeployInternall(SPList list, SPFolder folder, ListItemDefinition listItemModel)
        {
            if (IsDocumentLibray(list))
            {
                TraceService.Error((int)LogEventId.ModelProvisionCoreCall, "Please use ModuleFileDefinition to deploy files to the document libraries. Throwing SPMeta2NotImplementedException");

                throw new SPMeta2NotImplementedException("Please use ModuleFileDefinition to deploy files to the document libraries");
            }

            EnsureListItem(list, folder, listItemModel);
        }

        protected virtual SPListItem FindListItem(SPList list, SPFolder folder, ListItemDefinition listItemModel)
        {
            var items = list.GetItems(new SPQuery
            {
                Folder = folder,
                Query = string.Format(@"<Where>
                             <Eq>
                                 <FieldRef Name='Title'/>
                                 <Value Type='Text'>{0}</Value>
                             </Eq>
                            </Where>", listItemModel.Title)
            });

            if (items.Count > 0)
                return items[0];

            return null;
        }

        private SPListItem EnsureListItem(SPList list, SPFolder folder, ListItemDefinition listItemModel)
        {
            var currentItem = FindListItem(list, folder, listItemModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentItem,
                ObjectType = typeof(SPListItem),
                ObjectDefinition = listItemModel,
                ModelHost = folder
            });

            if (currentItem == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list item");

                var newItem = list.Items.Add(folder.ServerRelativeUrl, SPFileSystemObjectType.File, null);

                MapListItemProperties(newItem, listItemModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newItem,
                    ObjectType = typeof(SPListItem),
                    ObjectDefinition = listItemModel,
                    ModelHost = folder
                });

                newItem.Update();

                return newItem;
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list item");

                MapListItemProperties(currentItem, listItemModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentItem,
                    ObjectType = typeof(SPListItem),
                    ObjectDefinition = listItemModel,
                    ModelHost = folder
                });

                currentItem.Update();

                return currentItem;
            }
        }

        protected virtual void MapListItemProperties(SPListItem newItem, ListItemDefinition listItemModel)
        {
            newItem[BuiltInInternalFieldNames.Title] = listItemModel.Title;
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            SPListItem item = null;
            SPList list = null;

            if (modelHost is ListModelHost)
            {
                list = (modelHost as ListModelHost).HostList;
                var rootFolder = (modelHost as ListModelHost).HostList.RootFolder;

                item = EnsureListItem(list, rootFolder, listItemModel);
            }
            else if (modelHost is FolderModelHost)
            {
                // suppose it is a list, ir must be
                list = (modelHost as FolderModelHost).CurrentList;
                var rootFolder = (modelHost as FolderModelHost).CurrentListItem.Folder;

                item = EnsureListItem(list, rootFolder, listItemModel);
            }

            // naaaaah, just gonna get a new one list item
            // keep it simple and safe, really really really safe with all that SharePoint stuff...
            var currentItem = list.GetItemById(item.ID);

            action(currentItem);

            currentItem.Update();
        }


        private bool IsDocumentLibray(SPList list)
        {
            // TODO

            return false;
        }

        #endregion
    }
}
