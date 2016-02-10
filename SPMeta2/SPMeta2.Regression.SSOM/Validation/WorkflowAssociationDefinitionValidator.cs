using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WorkflowAssociationDefinitionValidator : WorkflowAssociationModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var spObject = GetWorfklowAssotiation(modelHost);
            var definition = model.WithAssertAndCast<WorkflowAssociationDefinition>("model",
                value => value.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Name, o => o.Name);

            // TODO

        }
    }
}
