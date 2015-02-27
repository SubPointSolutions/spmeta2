using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
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
    public class ClientResetRoleInheritanceDefinitionValidator : ResetRoleInheritanceModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var definition = model.WithAssertAndCast<ResetRoleInheritanceDefinition>("model", value => value.RequireNotNull());

            var context = securableObject.Context;

            if (!securableObject.IsObjectPropertyInstantiated("HasUniqueRoleAssignments"))
            {
                context.Load(securableObject, s => s.HasUniqueRoleAssignments);
                context.ExecuteQueryWithTrace();
            }

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
