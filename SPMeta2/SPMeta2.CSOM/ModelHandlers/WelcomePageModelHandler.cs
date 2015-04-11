using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class WelcomePageModelHandler : CSOMModelHandlerBase
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

        private void DeployWelcomePage(object modelHost, DefinitionBase model, Folder folder, WelcomePageDefinition welcomePgaeModel)
        {
            var context = folder.Context;

            context.Load(folder);
            context.ExecuteQueryWithTrace();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = folder,
                ObjectType = typeof(Folder),
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
                ObjectType = typeof(Folder),
                ObjectDefinition = welcomePgaeModel,
                ModelHost = modelHost
            });


            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling folder.Update()");
            folder.Update();

            context.ExecuteQueryWithTrace();
        }

        private static string ProcessWelcomeUrl(string welcomeUrl)
        {
            return UrlUtility.RemoveStartingSlash(welcomeUrl);
        }

        protected Folder ExtractFolderFromModelHost(object modelHost)
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

                return folderModelHost.CurrentListItem.Folder;
            }

            TraceService.ErrorFormat((int)LogEventId.ModelProvisionCoreCall, "Unsupported model host of type: [{0}]. Throwing SPMeta2UnsupportedModelHostException",
                    new[]
                    {
                        modelHost
                    });

            throw new SPMeta2UnsupportedModelHostException("modelHost should be web/list/folder model host");
        }

        #endregion
    }
}
