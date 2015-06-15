using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class SecurityGroupLinkDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSecurityGroupLink(this  ModelNode model, SecurityGroupLinkDefinition definition)
        {
            return AddSecurityGroupLink(model, definition, null);
        }

        public static ModelNode AddSecurityGroupLink(this ModelNode model, SecurityGroupLinkDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddSecurityGroupLink(this  ModelNode model, SecurityGroupDefinition definition)
        {
            return AddSecurityGroupLink(model, definition, null);
        }

        public static ModelNode AddSecurityGroupLink(this  ModelNode model, SecurityGroupDefinition definition, Action<ModelNode> action)
        {
            return AddSecurityGroupLink(model, new SecurityGroupLinkDefinition
            {
                SecurityGroupName = definition.Name,

                IsAssociatedMemberGroup = definition.IsAssociatedMemberGroup,
                IsAssociatedOwnerGroup = definition.IsAssociatedOwnerGroup,
                IsAssociatedVisitorGroup = definition.IsAssociatedVisitorsGroup

            }, action);
        }

        #endregion
    }
}
