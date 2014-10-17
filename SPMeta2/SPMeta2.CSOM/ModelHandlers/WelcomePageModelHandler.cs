using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
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
            context.ExecuteQuery();

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

            folder.WelcomePage = welcomePgaeModel.Url;

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

            folder.Update();

            context.ExecuteQuery();
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
                return (modelHost as FolderModelHost).CurrentLibraryFolder;
            }

            throw new ArgumentException("modelHost shoue be web/list/folder model host");
        }

        #endregion
    }
}
