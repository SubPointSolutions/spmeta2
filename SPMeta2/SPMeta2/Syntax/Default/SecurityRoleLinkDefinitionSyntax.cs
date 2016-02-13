using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class SecurityRoleLinkModelNode : TypedModelNode
    {

    }

    public static class SecurityRoleLinkDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSecurityRoleLink<TModelNode>(this TModelNode model, SecurityRoleLinkDefinition definition)
            where TModelNode : ModelNode, ISecurityRoleLinkHostModelNode, new()
        {
            return AddSecurityRoleLink(model, definition, null);
        }

        public static TModelNode AddSecurityRoleLink<TModelNode>(this TModelNode model, SecurityRoleLinkDefinition definition,
            Action<SecurityRoleLinkModelNode> action)
            where TModelNode : ModelNode, ISecurityRoleLinkHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSecurityRoleLinks<TModelNode>(this TModelNode model, IEnumerable<SecurityRoleLinkDefinition> definitions)
           where TModelNode : ModelNode, ISecurityRoleLinkHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static TModelNode AddSecurityRoleLink<TModelNode>(this TModelNode model, string securityRoleName)
               where TModelNode : ModelNode, ISecurityRoleLinkHostModelNode, new()
        {
            return AddSecurityRoleLink(model, securityRoleName, null);
        }

        public static TModelNode AddSecurityRoleLink<TModelNode>(this TModelNode model, string securityRoleName,
            Action<SecurityRoleLinkModelNode> action)
            where TModelNode : ModelNode, ISecurityRoleLinkHostModelNode, new()
        {
            var roleLinkDefinition = new SecurityRoleLinkDefinition
            {
                SecurityRoleName = securityRoleName
            };

            return model.AddSecurityRoleLink(roleLinkDefinition, action);
        }

        //public static ModelNode AddSecurityRoleLink(this ModelNode model, SecurityRoleLinkDefinition definition)
        //{
        //    return AddSecurityRoleLink(model, definition, null);
        //}

        //public static ModelNode AddSecurityRoleLink(this  ModelNode model, SecurityRoleLinkDefinition definition, Action<ModelNode> action)
        //{
        //    return model.AddDefinitionNode(definition, action);
        //}
    }
}
