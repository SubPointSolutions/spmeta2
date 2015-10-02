﻿using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WelcomePageModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WelcomePageDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folder = ExtractFolderFromModelHost(modelHost);
            var welcomePgaeModel = model.WithAssertAndCast<WelcomePageDefinition>("model", value => value.RequireNotNull());

            DeployWelcomePage(modelHost, model, folder, welcomePgaeModel);
        }

        private void DeployWelcomePage(object modelHost, DefinitionBase model, SPFolder folder, WelcomePageDefinition welcomePgaeModel)
        {
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = folder,
                ObjectType = typeof(SPFolder),
                ObjectDefinition = welcomePgaeModel,
                ModelHost = modelHost
            });

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Changing welcome page to: [{0}]", welcomePgaeModel.Url);
            
            // https://github.com/SubPointSolutions/spmeta2/issues/431
            folder.WelcomePage = UrlUtility.RemoveStartingSlash(welcomePgaeModel.Url);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = folder,
                ObjectType = typeof(SPFolder),
                ObjectDefinition = welcomePgaeModel,
                ModelHost = modelHost
            });

            folder.Update();
        }

        protected SPFolder ExtractFolderFromModelHost(object modelHost)
        {
            if (modelHost is WebModelHost)
            {
                return (modelHost as WebModelHost).HostWeb.RootFolder;
            }
            else if (modelHost is ListModelHost)
            {
                return (modelHost as ListModelHost).HostList.RootFolder;
            }
            else if (modelHost is FolderModelHost)
            {
                var folderModelHost = (modelHost as FolderModelHost);

                if (folderModelHost.CurrentLibraryFolder != null)
                    return folderModelHost.CurrentLibraryFolder;

                folderModelHost.ShouldUpdateHost = false;

                return folderModelHost.CurrentListItem.Folder;
            }

            throw new ArgumentException("modelHost shoue be web/list/folder model host");
        }

        #endregion
    }
}
