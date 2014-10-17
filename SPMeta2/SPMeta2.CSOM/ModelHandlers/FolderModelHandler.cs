using System.Runtime.Remoting.Lifetime;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Utils;

using SPMeta2.ModelHosts;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class FolderModelHandler : CSOMModelHandlerBase
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

            if (folderModelHost.CurrentList != null && folderModelHost.CurrentList.BaseType == BaseType.DocumentLibrary)
            {
                var currentFolder = EnsureLibraryFolder(folderModelHost, folderModel);

                var newContext = ModelHostBase.Inherit<FolderModelHost>(folderModelHost, c =>
                {
                    c.CurrentList = folderModelHost.CurrentList;
                    c.CurrentLibraryFolder = currentFolder;
                    c.CurrentWeb = folderModelHost.CurrentWeb;
                });

                action(newContext);
            }
            else if (folderModelHost.CurrentList != null && folderModelHost.CurrentList.BaseType != BaseType.DocumentLibrary)
            {
                var currentListItem = EnsureListFolder(folderModelHost, folderModel);

                var newContext = ModelHostBase.Inherit<FolderModelHost>(folderModelHost, c =>
                {
                    c.CurrentList = folderModelHost.CurrentList;
                    c.CurrentListItem = currentListItem;
                    c.CurrentWeb = folderModelHost.CurrentWeb;
                });

                action(newContext);
            }
            else
            {
                throw new NotImplementedException("model host is unklnown");
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var folderModel = model.WithAssertAndCast<FolderDefinition>("model", value => value.RequireNotNull());

            if (ShouldDeployLibraryFolder(folderModelHost))
                EnsureLibraryFolder(folderModelHost, folderModel);
            else if (ShouldDeployListFolder(folderModelHost))
                EnsureListFolder(folderModelHost, folderModel);
        }

        protected bool ShouldDeployListFolder(FolderModelHost folderModelHost)
        {
            return folderModelHost.CurrentList != null &&
                   folderModelHost.CurrentList.BaseType != BaseType.DocumentLibrary;
        }

        protected bool ShouldDeployLibraryFolder(FolderModelHost folderModelHost)
        {
            return folderModelHost.CurrentList != null &&
                   folderModelHost.CurrentList.BaseType == BaseType.DocumentLibrary;
        }

        protected Folder GetListFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            var tmp = string.Empty;
            var result = GetListFolder(folderModelHost, folderModel, out tmp);

            return result;
        }

        private Folder GetListFolder(FolderModelHost folderModelHost, FolderDefinition folderModel,
            out string serverRelativeUrl)
        {
            var list = folderModelHost.CurrentList;
            var context = list.Context;

            context.Load(list, l => l.RootFolder);
            context.Load(list, l => l.ParentWeb);

            if (folderModelHost.CurrentListItem != null)
            {
                context.Load(folderModelHost.CurrentListItem, l => l.Folder);
            }

            context.ExecuteQuery();

            serverRelativeUrl = folderModelHost.CurrentListItem == null
                                                ? list.RootFolder.ServerRelativeUrl
                                                : folderModelHost.CurrentListItem.Folder.ServerRelativeUrl;

            var currentUrl = serverRelativeUrl + "/" + folderModel.Name;
            var currentFolder = folderModelHost.CurrentList.ParentWeb.GetFolderByServerRelativeUrl(currentUrl);

            var doesFolderExist = false;

            try
            {
                context.Load(currentFolder, f => f.Name);
                context.ExecuteQuery();

                doesFolderExist = true;
            }
            catch (ServerException e)
            {
                if (e.ServerErrorTypeName == "System.IO.FileNotFoundException")
                    doesFolderExist = false;
            }

            if (doesFolderExist)
                return currentFolder;

            return null;
        }

        private ListItem EnsureListFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            string serverRelativeUrl = string.Empty;

            var list = folderModelHost.CurrentList;
            var context = list.Context;


            var currentFolder = GetListFolder(folderModelHost, folderModel, out serverRelativeUrl);
            var currentFolderItem = folderModelHost.CurrentListItem;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFolder,
                ObjectType = typeof(Folder),
                ObjectDefinition = folderModel,
                ModelHost = folderModelHost
            });

            context.ExecuteQuery();

            if (currentFolder == null)
            {
                currentFolderItem = list.AddItem(new ListItemCreationInformation
                {
                    FolderUrl = serverRelativeUrl,
                    LeafName = folderModel.Name,
                    UnderlyingObjectType = FileSystemObjectType.Folder
                });

                currentFolderItem["Title"] = folderModel.Name;
                currentFolderItem.Update();

                context.ExecuteQuery();



                context.Load(currentFolderItem.Folder);
                context.ExecuteQuery();

                currentFolder = currentFolderItem.Folder;
            }
            else
            {
                context.Load(currentFolder, f => f.ListItemAllFields);
                context.Load(currentFolder, f => f.Name);
                context.ExecuteQuery();

                currentFolderItem = currentFolder.ListItemAllFields;
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentFolder,
                ObjectType = typeof(Folder),
                ObjectDefinition = folderModel,
                ModelHost = folderModelHost
            });

            return currentFolderItem;
        }

        protected Folder GetLibraryFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            var parentFolder = folderModelHost.CurrentLibraryFolder;
            var context = parentFolder.Context;

            context.Load(parentFolder, f => f.Folders);
            context.ExecuteQuery();

            // dirty stuff, needs to be rewritten
            var currentFolder = parentFolder
                                   .Folders
                                   .OfType<Folder>()
                                   .FirstOrDefault(f => f.Name == folderModel.Name);

            if (currentFolder != null)
            {
                context.Load(currentFolder, f => f.Name);
                context.ExecuteQuery();
            }

            return currentFolder;
        }

        private Folder EnsureLibraryFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            var parentFolder = folderModelHost.CurrentLibraryFolder;
            var context = parentFolder.Context;

            var currentFolder = GetLibraryFolder(folderModelHost, folderModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFolder,
                ObjectType = typeof(Folder),
                ObjectDefinition = folderModel,
                ModelHost = folderModelHost
            });

            if (currentFolder == null)
            {
                currentFolder = parentFolder.Folders.Add(folderModel.Name);
                context.ExecuteQuery();
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentFolder,
                ObjectType = typeof(Folder),
                ObjectDefinition = folderModel,
                ModelHost = folderModelHost
            });

            currentFolder.Update();
            context.ExecuteQuery();

            return currentFolder;
        }

        #endregion
    }
}
