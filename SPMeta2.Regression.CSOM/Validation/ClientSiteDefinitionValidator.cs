using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSiteDefinitionValidator : SiteModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var site = modelHost.WithAssertAndCast<Site>("modelHost", value => value.RequireNotNull());
            var siteModel = model.WithAssertAndCast<SiteDefinition>("model", value => value.RequireNotNull());
        }
    }
}
