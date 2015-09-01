using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SupportedUICultureDefinitionValidator : SupportedUICultureModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<SupportedUICultureDefinition>("model", value => value.RequireNotNull());

            // TODO
            object spObject = null;

            ServiceFactory.AssertService
                       .NewAssert(definition, spObject)
                       .ShouldNotBeNull(spObject);
        }

        #endregion
    }
}
