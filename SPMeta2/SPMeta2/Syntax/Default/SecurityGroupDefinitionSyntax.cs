using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class SecurityGroupModelNode : TypedModelNode,
        ISecurityRoleLinkHostModelNode,
        ISecurityGroupHostModelNode
    {

    }

    public static class SecurityGroupDefinitionSyntax
    {

        #region methods

        public static TModelNode AddSecurityGroup<TModelNode>(this TModelNode model, SecurityGroupDefinition definition)
            where TModelNode : ModelNode, ISecurityGroupHostModelNode, new()
        {
            return AddSecurityGroup(model, definition, null);
        }

        public static TModelNode AddSecurityGroup<TModelNode>(this TModelNode model, SecurityGroupDefinition definition,
            Action<SecurityGroupModelNode> action)
            where TModelNode : ModelNode, ISecurityGroupHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSecurityGroups<TModelNode>(this TModelNode model, IEnumerable<SecurityGroupDefinition> definitions)
           where TModelNode : ModelNode, ISecurityGroupHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
