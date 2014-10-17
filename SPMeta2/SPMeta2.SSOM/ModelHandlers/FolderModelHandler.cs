using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FolderModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(FolderDefinition); }
        }

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var folderModel = model.WithAssertAndCast<FolderDefinition>("model", value => value.RequireNotNull());

            if (folderModelHost.CurrentLibrary != null)
            {
                var currentFolder = EnsureLibraryFolder(folderModelHost, folderModel);

                var newContext = new FolderModelHost
                {
                    CurrentLibrary = folderModelHost.CurrentLibrary,
                    CurrentLibraryFolder = currentFolder
                };

                action(newContext);
            }
            else if (folderModelHost.CurrentList != null)
            {
                var currentListItem = EnsureListFolder(folderModelHost, folderModel);

                var newContext = new FolderModelHost
                {
                    CurrentList = folderModelHost.CurrentList,
                    CurrentListItem = currentListItem
                };

                action(newContext);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var folderModel = model.WithAssertAndCast<FolderDefinition>("model", value => value.RequireNotNull());

            if (folderModelHost.CurrentLibrary != null)
                EnsureLibraryFolder(folderModelHost, folderModel);
            else if (folderModelHost.CurrentList != null)
                EnsureListFolder(folderModelHost, folderModel);
            else
            {
                throw new ArgumentException("Model host is not supported.");
            }
        }

        protected SPFolder GetListFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            var list = folderModelHost.CurrentList;
            var currentFolderItem = folderModelHost.CurrentListItem;

            var serverRelativeUrl = folderModelHost.CurrentListItem == null
                                                ? list.RootFolder.ServerRelativeUrl
                                                : folderModelHost.CurrentListItem.Folder.ServerRelativeUrl;

            var currentUrl = serverRelativeUrl + "/" + folderModel.Name;
            return folderModelHost.CurrentList.ParentWeb.GetFolder(currentUrl);
        }


        protected SPFolder GetLibraryFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            var parentFolder = folderModelHost.CurrentLibraryFolder;

            // dirty stuff, needs to be rewritten
            return parentFolder
                                   .SubFolders
                                   .OfType<SPFolder>()
                                   .FirstOrDefault(f => f.Name == folderModel.Name);
        }

        private SPListItem EnsureListFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            var list = folderModelHost.CurrentList;
            var currentFolderItem = folderModelHost.CurrentListItem;

            var serverRelativeUrl = folderModelHost.CurrentListItem == null
                                                ? list.RootFolder.ServerRelativeUrl
                                                : folderModelHost.CurrentListItem.Folder.ServerRelativeUrl;

            var currentUrl = serverRelativeUrl + "/" + folderModel.Name;
            var currentFolder = GetListFolder(folderModelHost, folderModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFolder == null || !currentFolder.Exists ? null : currentFolder,
                ObjectType = typeof(SPFolder),
                ObjectDefinition = folderModel,
                ModelHost = folderModelHost
            });

            if (!currentFolder.Exists)
            {
                currentFolderItem = list.AddItem(serverRelativeUrl, SPFileSystemObjectType.Folder);

                currentFolderItem[SPBuiltInFieldId.Title] = folderModel.Name;
                currentFolderItem.Update();

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentFolderItem.Folder,
                    ObjectType = typeof(SPFolder),
                    ObjectDefinition = folderModel,
                    ModelHost = folderModelHost
                });
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentFolder,
                    ObjectType = typeof(SPFolder),
                    ObjectDefinition = folderModel,
                    ModelHost = folderModelHost
                });

                currentFolder.Update();

                currentFolderItem = currentFolder.Item;
            }

            return currentFolderItem;
        }

        private SPFolder EnsureLibraryFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            var parentFolder = folderModelHost.CurrentLibraryFolder;

            // dirty stuff, needs to be rewritten
            var currentFolder = GetLibraryFolder(folderModelHost, folderModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFolder == null || !currentFolder.Exists ? null : currentFolder,
                ObjectType = typeof(SPFolder),
                ObjectDefinition = folderModel,
                ModelHost = folderModelHost
            });

            if (currentFolder == null || !currentFolder.Exists)
            {
                currentFolder = parentFolder.SubFolders.Add(folderModel.Name);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentFolder,
                    ObjectType = typeof(SPFolder),
                    ObjectDefinition = folderModel,
                    ModelHost = folderModelHost
                });
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentFolder,
                    ObjectType = typeof(SPFolder),
                    ObjectDefinition = folderModel,
                    ModelHost = folderModelHost
                });

                currentFolder.Update();
            }

            return currentFolder;
        }

        #endregion
    }
}
