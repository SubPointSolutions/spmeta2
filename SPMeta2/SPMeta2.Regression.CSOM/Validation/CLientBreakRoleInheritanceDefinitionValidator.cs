using SPMeta2.CSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientBreakRoleInheritanceDefinitionValidator : BreakRoleInheritanceModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var breakRoleInheritanceModel = model.WithAssertAndCast<BreakRoleInheritanceDefinition>("model", value => value.RequireNotNull());

            ValidateRoleInheritance(modelHost, securableObject, breakRoleInheritanceModel);
        }

        private void ValidateRoleInheritance(object modelHost, Microsoft.SharePoint.Client.SecurableObject securableObject, BreakRoleInheritanceDefinition breakRoleInheritanceModel)
        {
            // TODO
        }
    }
}
