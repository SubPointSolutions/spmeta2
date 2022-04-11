using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class UserModelNode : TypedModelNode
    {

    }

    public static class UserDefinitionSyntax
    {
        #region methods

        public static TModelNode AddUser<TModelNode>(this TModelNode model, UserDefinition definition)
            where TModelNode : ModelNode, ISecurityGroupHostModelNode, new()
        {
            return AddUser(model, definition, null);
        }

        public static TModelNode AddUser<TModelNode>(this TModelNode model, UserDefinition definition,
            Action<UserModelNode> action)
            where TModelNode : ModelNode, ISecurityGroupHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddUsers<TModelNode>(this TModelNode model, IEnumerable<UserDefinition> definitions)
           where TModelNode : ModelNode, ISecurityGroupHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
