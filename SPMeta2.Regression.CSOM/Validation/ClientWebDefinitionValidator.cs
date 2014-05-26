using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWebDefinitionValidator : WebModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var web = modelHost.WithAssertAndCast<Web>("modelHost", value => value.RequireNotNull());
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());
        }
    }
}
