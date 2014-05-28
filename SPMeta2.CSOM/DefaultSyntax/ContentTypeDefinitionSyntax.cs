using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.CSOM.DefaultSyntax
{
    public static class ContentTypeDefinitionSyntax
    {
        #region methods

        public static ModelNode OnCreating(this ModelNode model, Action<ContentTypeDefinition, ContentType> action)
        {
            model.RegisterModelEvent<ContentTypeDefinition, ContentType>(SPMeta2.Common.ModelEventType.OnUpdating, action);

            return model;
        }

        public static ModelNode OnCreated(this ModelNode model, Action<ContentTypeDefinition, ContentType> action)
        {
            model.RegisterModelEvent<ContentTypeDefinition, ContentType>(SPMeta2.Common.ModelEventType.OnUpdated, action);

            return model;
        }

        #endregion
    }
}
