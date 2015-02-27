using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ResetRoleInheritanceDefinitionValidator : ResetRoleInheritanceModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var definition = model.WithAssertAndCast<ResetRoleInheritanceDefinition>("model", value => value.RequireNotNull());

            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, securableObject)
                                            .ShouldNotBeNull(securableObject);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var dstProp = d.GetExpressionValue(m => m.HasUniqueRoleAssignments);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = null,
                    Dst = dstProp,
                    IsValid = d.HasUniqueRoleAssignments == false
                };
            });
        }
    }

}
