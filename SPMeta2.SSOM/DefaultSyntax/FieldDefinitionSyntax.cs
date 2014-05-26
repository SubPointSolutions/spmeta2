using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.SSOM.DefaultSyntax
{
    public static class FieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddField(this ModelNode model, FieldDefinition fieldDefinition, Action<ModelNode> action)
        {
            var defModel = new ModelNode { Value = fieldDefinition };

            model.ChildModels.Add(defModel);

            if (action != null)
                action(defModel);

            return model;
        }

        public static ModelNode OnCreating(this ModelNode model, Action<FieldDefinition, SPField> action)
        {
            model.RegisterModelEvent<FieldDefinition, SPField>(Common.ModelEventType.OnUpdating, action);

            return model;
        }

        public static ModelNode OnCreated(this ModelNode model, Action<FieldDefinition, SPField> action)
        {
            model.RegisterModelEvent<FieldDefinition, SPField>(Common.ModelEventType.OnUpdated, action);

            return model;
        }

        #endregion
    }
}
