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
    public static class BreakRoleInheritanceDefinitionSyntax
    {
        #region methods

        public static ModelNode AddBreakRoleInheritance(this ModelNode model, BreakRoleInheritanceDefinition definition)
        {
            return AddBreakRoleInheritance(model, definition, null);
        }

        public static ModelNode AddBreakRoleInheritance(this ModelNode model, BreakRoleInheritanceDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

     
    }
}
