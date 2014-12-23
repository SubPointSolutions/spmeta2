using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WebConfigModificationDefinitionValidator : WebConfigModificationModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WebConfigModificationDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentSPWebConfigModification(webAppModelHost.HostWebApplication, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            assert
                .ShouldBeEqual(m => m.Name, o => o.Name)
                .ShouldBeEqual(m => m.Owner, o => o.Owner)
                .ShouldBeEqual(m => m.Path, o => o.Path)
                .ShouldBeEqual(m => m.Value, o => o.Value)
                .ShouldBeEqual(m => m.Sequence, o => o.Sequence)
                .ShouldBeEqual(m => m.Path, o => o.Path)
                .ShouldBeEqual(m => m.Type, o => o.GetTypeAsString());
        }
    }

    internal static class WebConfigModificationDefinitionHelper
    {
        public static string GetTypeAsString(this SPWebConfigModification config)
        {
            return config.Type.ToString();
        }
    }
}
