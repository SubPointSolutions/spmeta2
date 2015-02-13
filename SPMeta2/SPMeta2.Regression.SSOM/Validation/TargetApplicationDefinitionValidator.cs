using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Administration;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class TargetApplicationDefinitionValidator : TargetApplicationModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var secureStoreModelHost = modelHost.WithAssertAndCast<SecureStoreApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TargetApplicationDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentObject(secureStoreModelHost.HostSecureStore, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                       .ShouldNotBeNull(spObject);
        }
    }

}
