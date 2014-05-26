using System;
using Microsoft.SharePoint.Client.WebParts;
using SPMeta2.Models;
using WebPartDefinition = SPMeta2.Definitions.WebPartDefinition;

namespace SPMeta2.CSOM.DefaultSyntax
{
    public static class WebPartDefinitionSyntax
    {
        #region methods

        #region behavior support

        public static ModelNode OnCreating(this ModelNode model, Action<WebPartDefinition, WebPart> action)
        {
            model.RegisterModelEvent<WebPartDefinition, WebPart>(SPMeta2.Common.ModelEventType.OnUpdating, action);

            return model;
        }

        public static ModelNode OnCreated(this ModelNode model, Action<WebPartDefinition, WebPart> action)
        {
            model.RegisterModelEvent<WebPartDefinition, WebPart>(SPMeta2.Common.ModelEventType.OnUpdated, action);

            return model;
        }

        #endregion

        #endregion
    }
}
