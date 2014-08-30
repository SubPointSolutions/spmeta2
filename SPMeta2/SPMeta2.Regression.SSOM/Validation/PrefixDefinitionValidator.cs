using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Validation.ServerModelHandlers
{
    public class PrefixDefinitionValidator : PrefixModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var prefixDefinition = model.WithAssertAndCast<PrefixDefinition>("model", value => value.RequireNotNull());

            ValidatePrefix(webAppModelHost.HostWebApplication, prefixDefinition);
   
        }

        private void ValidatePrefix(Microsoft.SharePoint.Administration.SPWebApplication sPWebApplication, PrefixDefinition prefixDefinition)
        {
            
        }
    }
}
