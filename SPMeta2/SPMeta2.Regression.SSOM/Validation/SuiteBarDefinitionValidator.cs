using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SuiteBarDefinitionValidator : SuiteBarModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<SuiteBarDefinition>("model", value => value.RequireNotNull());

            var spObject = typedModelHost.HostWebApplication;

            var assert = ServiceFactory.AssertService
                               .NewAssert(typedDefinition, spObject)
                               .ShouldNotBeNull(spObject)
                               .ShouldBeEqual(m => m.SuiteBarBrandingElementHtml, o => o.SuiteBarBrandingElementHtml);

        }

        #endregion
    }
}
