using Microsoft.SharePoint.Administration;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class PrefixDefinitionValidator : PrefixModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PrefixDefinition>("model", value => value.RequireNotNull());

            var spObject = GetPrefix(webAppModelHost.HostWebApplication, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .ShouldBeEqual(m => m.PrefixType, o => o.GetPrefixTypeString())
                                 .ShouldBeEqual(m => m.Path, o => o.Name);
        }
    }

    internal static class SPPrefixExtensions
    {
        public static string GetPrefixTypeString(this SPPrefix prefix)
        {
            return prefix.PrefixType.ToString();
        }
    }
}
