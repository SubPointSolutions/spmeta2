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

        public static SiteModelNode AddSecurityRole(this SiteModelNode model, SecurityRoleDefinition definition)
        {
            return AddSecurityRole(model, definition, null);
        }

        public static SiteModelNode AddSecurityRole(this SiteModelNode model, SecurityRoleDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static SiteModelNode AddSecurityRoles(this SiteModelNode model, IEnumerable<SecurityRoleDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
