using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class BreakRoleInheritanceDefinitionValidator : BreakRoleInheritanceModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var definition = model.WithAssertAndCast<BreakRoleInheritanceDefinition>("model", value => value.RequireNotNull());

            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, securableObject)
                                            .ShouldNotBeNull(securableObject)
                                            .ShouldBeEqual(m => m.ClearSubscopes, o => o.HasClearSubscopes())
                //.ShouldBeEqual(m => m.CopyRoleAssignments, o => o.HasCopyRoleAssignments())
                                            .ShouldBeEqual(m => m.ForceClearSubscopes, o => o.HasClearSubscopes());
        }
    }

    internal static class SPSecurableHelper
    {
        public static bool HasClearSubscopes(this SPSecurableObject secObject)
        {
            return secObject.RoleAssignments.Count == 0;
        }

        public static bool HasCopyRoleAssignments(this SPSecurableObject secObject)
        {
            var parent = secObject.FirstUniqueAncestorSecurableObject;
            return secObject.RoleAssignments == parent.RoleAssignments;
        }

    }
}
