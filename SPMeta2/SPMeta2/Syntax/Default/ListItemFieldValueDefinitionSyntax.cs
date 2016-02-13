using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ListItemFieldValueModelNode : TypedModelNode
    {

    }

    public static class ListItemFieldValueDefinitionSyntax
    {
        public static TModelNode AddListItemFieldValue<TModelNode>(this TModelNode model,
            Guid fieldId, object fieldValue)
            where TModelNode : ModelNode, IListItemModelNode, new()
        {
            return AddListItemFieldValue(model, fieldId, fieldValue, null);
        }

        public static TModelNode AddListItemFieldValue<TModelNode>(this TModelNode model,
            Guid fieldId, object fieldValue, Action<ListItemFieldValueModelNode> action)
            where TModelNode : ModelNode, IListItemModelNode, new()
        {
            return AddListItemFieldValue(model, new ListItemFieldValueDefinition
            {
                FieldId = fieldId,
                Value = fieldValue
            }, action);
        }

        public static TModelNode AddListItemFieldValue<TModelNode>(this TModelNode model,
            string fieldName, object fieldValue)
            where TModelNode : ModelNode, IListItemModelNode, new()
        {
            return AddListItemFieldValue(model, fieldName, fieldValue, null);
        }

        public static TModelNode AddListItemFieldValue<TModelNode>(this TModelNode model,
            string fieldName, object fieldValue, Action<ListItemFieldValueModelNode> action)
            where TModelNode : ModelNode, IListItemModelNode, new()
        {
            return AddListItemFieldValue(model, new ListItemFieldValueDefinition
            {
                FieldName = fieldName,
                Value = fieldValue
            }, action);
        }

        #region methods

        public static TModelNode AddListItemFieldValue<TModelNode>(this TModelNode model, ListItemFieldValueDefinition definition)
            where TModelNode : ModelNode, IListItemModelNode, new()
        {
            return AddListItemFieldValue(model, definition, null);
        }

        public static TModelNode AddListItemFieldValue<TModelNode>(this TModelNode model, ListItemFieldValueDefinition definition,
            Action<ListItemFieldValueModelNode> action)
            where TModelNode : ModelNode, IListItemModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddListItemFieldValues<TModelNode>(this TModelNode model, IEnumerable<ListItemFieldValueDefinition> definitions)
           where TModelNode : ModelNode, IListItemModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

    }
}
