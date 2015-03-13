using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class ListItemFieldValueDefinitionSyntax
    {
        #region methods

        public static ModelNode AddListItemFieldValue(this ModelNode model, Guid fieldId, object fieldValue)
        {
            return AddListItemFieldValue(model, fieldId, fieldValue, null);
        }

        public static ModelNode AddListItemFieldValue(this ModelNode model, Guid fieldId, object fieldValue, Action<ModelNode> action)
        {
            return AddListItemFieldValue(model, new ListItemFieldValueDefinition
            {
                FieldId = fieldId,
                Value = fieldValue
            }, action);
        }

        public static ModelNode AddListItemFieldValue(this ModelNode model, string fieldName, object fieldValue)
        {
            return AddListItemFieldValue(model, fieldName, fieldValue, null);
        }

        public static ModelNode AddListItemFieldValue(this ModelNode model, string fieldName, object fieldValue, Action<ModelNode> action)
        {
            return AddListItemFieldValue(model, new ListItemFieldValueDefinition
            {
                FieldName = fieldName,
                Value = fieldValue
            }, action);
        }

        public static ModelNode AddListItemFieldValue(this ModelNode model, ListItemFieldValueDefinition definition)
        {
            return AddListItemFieldValue(model, definition, null);
        }

        public static ModelNode AddListItemFieldValue(this ModelNode model, ListItemFieldValueDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }



        #endregion

        #region array overload

        public static ModelNode AddListItemFieldValues(this ModelNode model, IEnumerable<ListItemFieldValueDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
      
    }
}
