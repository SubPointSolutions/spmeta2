using System;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class ImageRenditionModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ImageRenditionDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var navigationModel = model.WithAssertAndCast<ImageRenditionDefinition>("model", value => value.RequireNotNull());

            DeployImageRenditionSettings(modelHost, siteModelHost, navigationModel);
        }

        private void DeployImageRenditionSettings(object modelHost, SiteModelHost siteModelHost, ImageRenditionDefinition navigationModel)
        {

        }


        #endregion
    }
}
