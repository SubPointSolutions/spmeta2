using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class SecurityRoleLinkDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSecurityRole(this ModelNode model, SecurityRoleDefinition definition)
        {
            return AddSecurityRole(model, definition, null);
        }

        public static ModelNode AddSecurityRole(this ModelNode model, SecurityRoleDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddSecurityRoles(this ModelNode model, IEnumerable<SecurityRoleDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
