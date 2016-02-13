using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class BreakRoleInheritanceModelNode : TypedModelNode
    {

    }

    public static class BreakRoleInheritanceDefinitionSyntax
    {
        #region methods

        public static TModelNode AddBreakRoleInheritance<TModelNode>(this TModelNode model, BreakRoleInheritanceDefinition definition)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            return AddBreakRoleInheritance(model, definition, null);
        }

        public static TModelNode AddBreakRoleInheritance<TModelNode>(this TModelNode model, BreakRoleInheritanceDefinition definition,
            Action<TModelNode> action)
            where TModelNode : ModelNode, ISecurableObjectHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        #endregion
    }
}
