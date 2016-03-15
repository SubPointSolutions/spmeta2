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
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            if (modelHost is ListModelHost)
            {
                var list = (modelHost as ListModelHost).HostList;
                var rootFolder = (modelHost as ListModelHost).HostList.RootFolder;

                if (!rootFolder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    rootFolder.Context.Load(rootFolder, f => f.ServerRelativeUrl);
                    rootFolder.Context.ExecuteQueryWithTrace();
                }

                DeployInternall(list, rootFolder, listItemModel);
            }
            else if (modelHost is FolderModelHost)
            {
#if NET35
                throw new SPMeta2NotImplementedException("Not implemented for SP2010 - https://github.com/SubPointSolutions/spmeta2/issues/767");
#endif

#if !NET35

                // suppose it is a list, it must be
                var list = (modelHost as FolderModelHost).CurrentList;
                var rootFolder = (modelHost as FolderModelHost).CurrentListItem.Folder;

                if (!rootFolder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    rootFolder.Context.Load(rootFolder, f => f.ServerRelativeUrl);
                    rootFolder.Context.ExecuteQueryWithTrace();
                }

                DeployInternall(list, rootFolder, listItemModel);

#endif
            }
            else
            {
                throw new SPMeta2UnsupportedModelHostException("modeHost should be either ListModelHost or FolderModelHost");
            }
        }

        private void DeployInternall(List list, Folder folder, ListItemDefinition listItemModel)
        {
            if (IsDocumentLibray(list))
            {
                TraceService.Error((int)LogEventId.ModelProvisionCoreCall, "Please use ModuleFileDefinition to deploy files to the document libraries. Throwing SPMeta2NotImplementedException");

                throw new SPMeta2NotImplementedException("Please use ModuleFileDefinition to deploy files to the document libraries");
            }

            EnsureListItem(list, folder, listItemModel);
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


            var listModeHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            List list = null;
            Folder rootFolder = null;

            if (modelHost is ListModelHost)
            {
                list = (modelHost as ListModelHost).HostList;
                rootFolder = (modelHost as ListModelHost).HostList.RootFolder;

                if (!rootFolder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    rootFolder.Context.Load(rootFolder, f => f.ServerRelativeUrl);
                    rootFolder.Context.ExecuteQueryWithTrace();
                }
            }
            else if (modelHost is FolderModelHost)
            {
#if NET35
                throw new SPMeta2NotImplementedException("Not implemented for SP2010 - https://github.com/SubPointSolutions/spmeta2/issues/767");
#endif

#if !NET35
                list = (modelHost as FolderModelHost).CurrentList;
                rootFolder = (modelHost as FolderModelHost).CurrentListItem.Folder;

                if (!rootFolder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    rootFolder.Context.Load(rootFolder, f => f.ServerRelativeUrl);
                    rootFolder.Context.ExecuteQueryWithTrace();
                }

#endif
            }

            var item = EnsureListItem(list, rootFolder, listItemModel);
            var context = list.Context;

            // naaaaah, just gonna get a new one list item
            // keep it simple and safe, really really really safe with all that SharePoint stuff...
            // var currentItem = list.GetItemById(item.Id);

            //context.Load(currentItem);
            //context.ExecuteQueryWithTrace();

            if (childModelType == typeof(ListItemFieldValueDefinition)
                || childModelType == typeof(ListItemFieldValuesDefinition))
            {
                var listItemPropertyHost = new ListItemModelHost
                {
                    HostListItem = item
                };

                action(listItemPropertyHost);
            }
            else
            {
                action(item);
            }

            item.Update();
            context.ExecuteQueryWithTrace();
        }

        protected virtual ListItem FindListItem(List list, Folder folder, ListItemDefinition definition)
        {
            var context = list.Context;

            var items = list.GetItems(new CamlQuery
            {
                FolderServerRelativeUrl = folder.ServerRelativeUrl,
                ViewXml = string.Format(@"<View>
                                          <Query>
                                             <Where>
                                                 <Eq>
                                                     <FieldRef Name='Title'/>
                                                     <Value Type='Text'>{0}</Value>
                                                 </Eq>
                                                </Where>
                                            </Query>
                                         </View>", definition.Title)
            });

            context.Load(items);
            context.ExecuteQueryWithTrace();

            if (items.Count > 0)
                return items[0];

            return null;
        }

        private ListItem EnsureListItem(List list, Folder folder, ListItemDefinition listItemModel)
        {
            var context = list.Context;
            var currentItem = FindListItem(list, folder, listItemModel);

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

                var newItem = list.AddItem(new ListItemCreationInformation
                {
                    FolderUrl = folder.ServerRelativeUrl,
                    UnderlyingObjectType = FileSystemObjectType.File,
                    LeafName = null
                });

                MapListItemProperties(newItem, listItemModel);

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

                MapListItemProperties(currentItem, listItemModel);

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

        protected virtual void MapListItemProperties(ListItem currentItem, ListItemDefinition definition)
        {
            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                currentItem[BuiltInInternalFieldNames.ContentTypeId] = definition.ContentTypeId;
            }
            else if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                currentItem[BuiltInInternalFieldNames.ContentTypeId] = ContentTypeLookupService
                                            .LookupContentTypeByName(currentItem.ParentList, definition.ContentTypeName)
                                            .Id.ToString();
            }

            FieldLookupService.EnsureDefaultValues(currentItem, definition.DefaultValues);


            currentItem[BuiltInInternalFieldNames.Title] = definition.Title;

            FieldLookupService.EnsureValues(currentItem, definition.Values, true);
        }

        private bool IsDocumentLibray(List list)
        {
            // TODO
            return false;
        }

        #endregion
    }
}
