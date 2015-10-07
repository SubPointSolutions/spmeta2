using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class SecurityGroupLinkModelNode : TypedModelNode, ISecurityRoleLinkHostModelNode

    {

    }

    public static class SecurityGroupLinkDefinitionSyntax
    {
        #region methods

        #region methods

        public static TModelNode AddSecurityGroupLink<TModelNode>(this TModelNode model, SecurityGroupLinkDefinition definition)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            return AddSecurityGroupLink(model, definition, null);
        }

        public static TModelNode AddSecurityGroupLink<TModelNode>(this TModelNode model, SecurityGroupLinkDefinition definition,
            Action<SecurityGroupLinkModelNode> action)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        public static TModelNode AddSecurityGroupLink<TModelNode>(this TModelNode model, SecurityGroupDefinition definition)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            return AddSecurityGroupLink(model, definition, null);
        }

        public static TModelNode AddSecurityGroupLink<TModelNode>(this TModelNode model, SecurityGroupDefinition definition,
            Action<SecurityGroupLinkModelNode> action)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
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
