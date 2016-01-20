using SPMeta2.CSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using SPMeta2.Definitions;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Regression.CSOM.Extensions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientBreakRoleInheritanceDefinitionValidator : BreakRoleInheritanceModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var definition = model.WithAssertAndCast<BreakRoleInheritanceDefinition>("model", value => value.RequireNotNull());

            var assert = ServiceFactory.AssertService.NewAssert(definition, securableObject);

            assert
                .ShouldNotBeNull(securableObject)
                .ShouldBeEqual(m => m.ClearSubscopes, o => o.HasClearSubscopes())
                .ShouldBeEqual(m => m.ForceClearSubscopes, o => o.HasClearSubscopes());
        }
    }
}
