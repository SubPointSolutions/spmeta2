using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static ModelNode AddListItemFieldValue(this ModelNode model, ListItemFieldValueDefinition fieldValueDefinition)
        {
            return AddListItemFieldValue(model, fieldValueDefinition, null);
        }

        public static ModelNode AddListItemFieldValue(this ModelNode model, ListItemFieldValueDefinition fieldValueDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = fieldValueDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }


        #endregion
    }
}
