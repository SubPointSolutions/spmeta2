using SPMeta2.CSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using SPMeta2.Definitions;
using Microsoft.SharePoint.Client;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientBreakRoleInheritanceDefinitionValidator : BreakRoleInheritanceModelHandler
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
        public static bool HasClearSubscopes(this SecurableObject secObject)
        {
            if (!secObject.IsPropertyAvailable("RoleAssignments"))
            {
                secObject.Context.Load(secObject, s => s.RoleAssignments);
                secObject.Context.ExecuteQuery();
            }

            return secObject.RoleAssignments.Count == 0;
        }

        public static bool HasCopyRoleAssignments(this SecurableObject secObject)
        {
            if (!secObject.IsPropertyAvailable("FirstUniqueAncestorSecurableObject"))
            {
                secObject.Context.Load(secObject, s => s.FirstUniqueAncestorSecurableObject);
                secObject.Context.ExecuteQuery();
            }

            var parent = secObject.FirstUniqueAncestorSecurableObject;
            return secObject.RoleAssignments == parent.RoleAssignments;
        }

    }
}
