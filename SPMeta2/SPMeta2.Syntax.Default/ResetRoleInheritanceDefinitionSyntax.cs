using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class ResetRoleInheritanceDefinitionSyntax
    {
        #region methods

        public static ModelNode AddResetRoleInheritance(this ModelNode model, ResetRoleInheritanceDefinition definition)
        {
            return AddResetRoleInheritance(model, definition, null);
        }

        public static ModelNode AddResetRoleInheritance(this ModelNode model, ResetRoleInheritanceDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

     
    }
}
