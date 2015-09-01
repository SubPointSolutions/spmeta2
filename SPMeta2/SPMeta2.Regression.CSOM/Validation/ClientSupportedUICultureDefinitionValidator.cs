using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSupportedUICultureDefinitionValidator : SupportedUICultureModelHandler
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
