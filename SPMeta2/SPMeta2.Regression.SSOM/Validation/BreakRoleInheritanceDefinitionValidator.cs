using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.Regression.SSOM.Extensions;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class BreakRoleInheritanceDefinitionValidator : BreakRoleInheritanceModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var definition = model.WithAssertAndCast<BreakRoleInheritanceDefinition>("model",
                value => value.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(definition, securableObject);

            assert
                .ShouldNotBeNull(securableObject)

                .ShouldBeEqual(m => m.ClearSubscopes, o => o.HasClearSubscopes())
                .ShouldBeEqual(m => m.ForceClearSubscopes, o => o.HasClearSubscopes());
        }
    }
}
