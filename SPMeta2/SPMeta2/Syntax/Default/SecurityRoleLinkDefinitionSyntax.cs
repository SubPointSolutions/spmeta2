using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class SecurityRoleDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSecurityRoleLink(this  ModelNode model, SecurityRoleDefinition definition)
        {
            return AddSecurityRoleLink(model, definition, null);
        }

        public static ModelNode AddSecurityRoleLink(this ModelNode model, SecurityRoleDefinition definition, Action<ModelNode> action)
        {
            var roleLinkDefinition = new SecurityRoleLinkDefinition
            {
                SecurityRoleName = definition.Name
            };

            return model.AddSecurityRoleLink(roleLinkDefinition, action);
        }

        public static ModelNode AddSecurityRoleLink(this ModelNode model, string securityRoleName)
        {
            return AddSecurityRoleLink(model, securityRoleName, null);
        }

        public static ModelNode AddSecurityRoleLink(this  ModelNode model, string securityRoleName, Action<ModelNode> action)
        {
            var roleLinkDefinition = new SecurityRoleLinkDefinition
            {
                SecurityRoleName = securityRoleName
            };

            return model.AddSecurityRoleLink(roleLinkDefinition, action);
        }

        public static ModelNode AddSecurityRoleLink(this ModelNode model, SecurityRoleLinkDefinition definition)
        {
            return AddSecurityRoleLink(model, definition, null);
        }

        public static ModelNode AddSecurityRoleLink(this  ModelNode model, SecurityRoleLinkDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
