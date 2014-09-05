using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWebDefinitionValidator : WebModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var parentWeb = ExtractWeb(modelHost);
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            ValidateWebDefinition(modelHost, parentWeb, webModel);
        }

        private void ValidateWebDefinition(object modelHost, Web web, WebDefinition model)
        {
        }
    }
}
