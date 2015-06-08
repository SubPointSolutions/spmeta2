using System;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ComposedLookItemLinkDefinitionValidator : ComposedLookItemLinkModelHandler
    {
        public override Type TargetType
        {
            get { return typeof(ComposedLookItemLinkDefinition); }
        }

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<ComposedLookItemLinkDefinition>("model", value => value.RequireNotNull());

            var assert = ServiceFactory.AssertService
                                       .NewAssert(typedDefinition, typedModelHost.HostWeb)
                                       .ShouldNotBeNull(typedModelHost.HostWeb);

        }

        #endregion
    }

}
