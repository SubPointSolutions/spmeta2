using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
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
            var folder = ExtractFoleerFromMOdelHost(modelHost);
            var welcomePgaeModel = model.WithAssertAndCast<WelcomePageDefinition>("model", value => value.RequireNotNull());

            DeployWelcomePage(model, folder, welcomePgaeModel);
        }

        private void DeployWelcomePage(DefinitionBase model, SPFolder folder, WelcomePageDefinition welcomePgaeModel)
        {
            folder.WelcomePage = welcomePgaeModel.Url;
            folder.Update();
        }

        protected SPFolder ExtractFoleerFromMOdelHost(object modelHost)
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
