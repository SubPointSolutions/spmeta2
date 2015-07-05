﻿using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;

namespace SPMeta2.Syntax.Default
{
    public class SecurityRoleModelNode : TypedModelNode
    {

    }

    public static class SecurityRoleDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSecurityRoleLink<TModelNode>(this TModelNode model, SecurityRoleLinkDefinition definition)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            return AddSecurityRoleLink(model, definition, null);
        }

        public static TModelNode AddSecurityRoleLink<TModelNode>(this TModelNode model, SecurityRoleLinkDefinition definition,
            Action<SecurityRoleModelNode> action)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSecurityRoleLinks<TModelNode>(this TModelNode model, IEnumerable<SecurityRoleLinkDefinition> definitions)
           where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static TModelNode AddSecurityRoleLink<TModelNode>(this TModelNode model, string securityRoleName)
               where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            return AddSecurityRoleLink(model, securityRoleName, null);
        }

        public static TModelNode AddSecurityRoleLink<TModelNode>(this TModelNode model, string securityRoleName,
            Action<SecurityRoleModelNode> action)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
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
