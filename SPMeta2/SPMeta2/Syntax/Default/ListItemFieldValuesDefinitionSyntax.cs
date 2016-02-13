using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ListItemFieldValuesModelNode : TypedModelNode
    {

    }

    public static class ListItemFieldValuesDefinitionSyntax
    {
        #region methods

        public static TModelNode AddListItemFieldValues<TModelNode>(this TModelNode model, ListItemFieldValuesDefinition definition)
            where TModelNode : ModelNode, IListItemModelNode, new()
        {
            return AddListItemFieldValues(model, definition, null);
        }

        public static TModelNode AddListItemFieldValues<TModelNode>(this TModelNode model, ListItemFieldValuesDefinition definition,
            Action<ListItemFieldValuesModelNode> action)
            where TModelNode : ModelNode, IListItemModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddListItemFieldValues<TModelNode>(this TModelNode model, IEnumerable<ListItemFieldValuesDefinition> definitions)
           where TModelNode : ModelNode, IListItemModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

    }
}
