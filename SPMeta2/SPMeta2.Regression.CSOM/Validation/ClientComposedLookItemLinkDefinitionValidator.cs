using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientComposedLookItemLinkDefinitionValidator : ComposedLookItemLinkModelHandler
    {
        public override Type TargetType
        {
            get { return typeof(ComposedLookItemLinkDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<ComposedLookItemLinkDefinition>("model", value => value.RequireNotNull());

            var assert = ServiceFactory.AssertService
                                       .NewAssert(typedDefinition, typedModelHost.HostWeb)
                                       .ShouldNotBeNull(typedModelHost.HostWeb);

        }
    }

    

}
