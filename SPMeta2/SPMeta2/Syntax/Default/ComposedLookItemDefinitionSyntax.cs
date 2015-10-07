using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ComposedLookItemModelNode : TypedModelNode
    {

    }

    public static class ComposedLookItemDefinitionSyntax
    {
        #region methods

        public static TModelNode AddComposedLookItem<TModelNode>(this TModelNode model, ComposedLookItemDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddComposedLookItem(model, definition, null);
        }

        public static TModelNode AddComposedLookItem<TModelNode>(this TModelNode model, ComposedLookItemDefinition definition,
            Action<ComposedLookItemModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddComposedLookItems<TModelNode>(this TModelNode model, IEnumerable<ComposedLookItemDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
