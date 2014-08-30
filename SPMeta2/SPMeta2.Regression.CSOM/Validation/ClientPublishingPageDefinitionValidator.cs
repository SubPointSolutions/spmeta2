using System;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientPublishingPageDefinitionValidator : PublishingPageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var publishingPageModel = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            ValidatePublishingPage(modelHost, folder, publishingPageModel);
        }

        private void ValidatePublishingPage(object modelHost, Folder list, PublishingPageDefinition publishingPageModel)
        {
            // TODO
        }

        #endregion
    }
}
