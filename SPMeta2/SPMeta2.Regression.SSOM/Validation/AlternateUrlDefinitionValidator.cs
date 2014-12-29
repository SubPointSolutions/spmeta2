using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class AlternateUrlDefinitionValidator : AlternateUrlModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<AlternateUrlDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentAlternateUrl(webAppModelHost.HostWebApplication, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                       .ShouldBeEqual(m => m.Url, o => o.IncomingUrl)
                                       .ShouldBeEqual(m => m.UrlZone, o => o.GetUrlZone());
        }
    }

    internal static class SPAlternateUrlExtensions
    {
        public static string GetUrlZone(this SPAlternateUrl alternateUrl)
        {
            return alternateUrl.UrlZone.ToString();
        }
    }
}
