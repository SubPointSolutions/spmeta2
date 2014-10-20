using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class ImageRenditionDefinitionValidator : ImageRenditionModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ImageRenditionDefinition>("model", value => value.RequireNotNull());

            var context = siteModelHost.HostSite.Context;

            var renditions = SiteImageRenditions.GetRenditions(siteModelHost.HostSite.Context);
            context.ExecuteQuery();

            var spObject = GetCurrentImageRendition(renditions, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject);
        }

        #endregion
    }
}
