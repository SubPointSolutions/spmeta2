using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSiteDefinitionValidator : SiteModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SiteDefinition>("model", value => value.RequireNotNull());

            var spObject = siteModelHost.HostSite;

            var assert = ServiceFactory.AssertService
                          .NewAssert(definition, spObject)
                                .ShouldNotBeNull(spObject);

            var skipMessage = "Site definition is not deployed in CSOM.";

            assert
                .SkipProperty(m => m.Name, skipMessage)
                .SkipProperty(m => m.Description, skipMessage)
                .SkipProperty(m => m.SiteTemplate, skipMessage)
                .SkipProperty(m => m.Url, skipMessage)
                .SkipProperty(m => m.PrefixName, skipMessage)

                .SkipProperty(m => m.OwnerLogin, skipMessage)
                .SkipProperty(m => m.OwnerName, skipMessage)
                .SkipProperty(m => m.OwnerEmail, skipMessage)

                .SkipProperty(m => m.SecondaryContactLogin, skipMessage)
                .SkipProperty(m => m.SecondaryContactName, skipMessage)
                .SkipProperty(m => m.SecondaryContactEmail, skipMessage)

                .SkipProperty(m => m.LCID, skipMessage)
                .SkipProperty(m => m.DatabaseName, skipMessage);
        }
    }
}
