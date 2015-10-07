using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ResetRoleInheritanceModelNode : TypedModelNode
    {

    }

    public static class ResetRoleInheritanceDefinitionSyntax
    {
        #region methods

        public static TModelNode AddResetRoleInheritance<TModelNode>(this TModelNode model, ResetRoleInheritanceDefinition definition)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            return AddResetRoleInheritance(model, definition, null);
        }

        public static TModelNode AddResetRoleInheritance<TModelNode>(this TModelNode model, ResetRoleInheritanceDefinition definition,
            Action<TModelNode> action)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
