using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

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
