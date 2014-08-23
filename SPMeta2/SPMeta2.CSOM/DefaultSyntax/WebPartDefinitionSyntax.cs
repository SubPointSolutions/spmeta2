using System;
using Microsoft.SharePoint.Client.WebParts;
using SPMeta2.Models;
using WebPartDefinition = SPMeta2.Definitions.WebPartDefinition;

namespace SPMeta2.CSOM.DefaultSyntax
{
    [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use ModernSyntax.OnProvisioning/OnProvisioned events.")]
    public static class WebPartDefinitionSyntax
    {
        #region methods

        #region behavior support

        [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use ModernSyntax.OnProvisioning/OnProvisioned events.")]
        public static ModelNode OnCreating(this ModelNode model, Action<WebPartDefinition, WebPart> action)
        {
            model.RegisterModelEvent<WebPartDefinition, WebPart>(SPMeta2.Common.ModelEventType.OnUpdating, action);

            return model;
        }

        [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use ModernSyntax.OnProvisioning/OnProvisioned events.")]
        public static ModelNode OnCreated(this ModelNode model, Action<WebPartDefinition, WebPart> action)
        {
            model.RegisterModelEvent<WebPartDefinition, WebPart>(SPMeta2.Common.ModelEventType.OnUpdated, action);

            return model;
        }

        #endregion

        #endregion
    }
}
