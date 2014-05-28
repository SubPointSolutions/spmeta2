using System.Runtime.Remoting.Lifetime;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Utils;

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

                var newContext = new FolderModelHost
                {
                    CurrentList = folderModelHost.CurrentList,
                    CurrentLibraryFolder = currentFolder,
                    CurrentWeb = folderModelHost.CurrentWeb
                    //Folder = currentFolder,
                    //List = folderModelHost.CurrentList,
                    //Web = folderModelHost.CurrentWeb
                };

                action(newContext);
            }
            else if (folderModelHost.CurrentList != null && folderModelHost.CurrentList.BaseType != BaseType.DocumentLibrary)
            {
                var currentListItem = EnsureListFolder(folderModelHost, folderModel);

                var newContext = new FolderModelHost
                {
                    CurrentList = folderModelHost.CurrentList,
                    CurrentListItem = currentListItem,
                    CurrentWeb = folderModelHost.CurrentWeb
                };

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

            if (folderModelHost.CurrentList != null && folderModelHost.CurrentList.BaseType == BaseType.DocumentLibrary)
                EnsureLibraryFolder(folderModelHost, folderModel);
            else if (folderModelHost.CurrentList != null && folderModelHost.CurrentList.BaseType != BaseType.DocumentLibrary)
                EnsureListFolder(folderModelHost, folderModel);
        }

        private ListItem EnsureListFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
        {
            var list = folderModelHost.CurrentList;
            var context = list.Context;

            var currentFolderItem = folderModelHost.CurrentListItem;

            context.Load(list, l => l.RootFolder);
            context.Load(list, l => l.ParentWeb);

            if (folderModelHost.CurrentListItem != null)
            {
                context.Load(folderModelHost.CurrentListItem, l => l.Folder);
            }

            context.ExecuteQuery();

            var serverRelativeUrl = folderModelHost.CurrentListItem == null
                                                ? list.RootFolder.ServerRelativeUrl
                                                : folderModelHost.CurrentListItem.Folder.ServerRelativeUrl;

            var currentUrl = serverRelativeUrl + "/" + folderModel.Name;
            var currentFolder = folderModelHost.CurrentList.ParentWeb.GetFolderByServerRelativeUrl(currentUrl);

            var doesFolderExist = false;

            try
            {
                context.ExecuteQuery();
                doesFolderExist = true;
            }
            catch (ServerException e)
            {
                if (e.ServerErrorTypeName == "System.IO.FileNotFoundException")
                    doesFolderExist = false;
            }

            context.ExecuteQuery();

            if (!doesFolderExist)
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
            }
            else
            {
                context.Load(currentFolder, f => f.ListItemAllFields);
                context.ExecuteQuery();

                currentFolderItem = currentFolder.ListItemAllFields;
            }

            return currentFolderItem;
        }

        private Folder EnsureLibraryFolder(FolderModelHost folderModelHost, FolderDefinition folderModel)
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

            if (currentFolder == null)
                currentFolder = parentFolder.Folders.Add(folderModel.Name);

            context.ExecuteQuery();

            return currentFolder;
        }

        #endregion
    }
}
