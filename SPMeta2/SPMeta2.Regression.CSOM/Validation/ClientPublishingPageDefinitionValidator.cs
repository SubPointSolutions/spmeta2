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
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var publishingPageModel = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            ValidatePublishingPage(modelHost, list, publishingPageModel);
        }

        private void ValidatePublishingPage(object modelHost, List list, PublishingPageDefinition publishingPageModel)
        {
            // TODO
        }

        #endregion
    }
}
