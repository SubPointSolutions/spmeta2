using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.CSOM.DefaultSyntax
{
    public static class FieldDefinitionSyntax
    {
        #region methods

        public static ModelNode OnCreating(this ModelNode model, Action<FieldDefinition, Field> action)
        {
            model.RegisterModelEvent<FieldDefinition, Field>(SPMeta2.Common.ModelEventType.OnUpdating, action);

            return model;
        }

        public static ModelNode OnCreated(this ModelNode model, Action<FieldDefinition, Field> action)
        {
            model.RegisterModelEvent<FieldDefinition, Field>(SPMeta2.Common.ModelEventType.OnUpdated, action);

            return model;
        }

        #endregion
    }
}
