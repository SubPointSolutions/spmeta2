using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.CSOM.DefaultSyntax
{
    [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use ModernSyntax.OnProvisioning/OnProvisioned events.")]
    public static class ModuleFileDefinitionSyntax
    {
        [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use ModernSyntax.OnProvisioning/OnProvisioned events.")]
        public static ModelNode OnCreating(this ModelNode model, Action<ModuleFileDefinition, File> action)
        {
            model.RegisterModelEvent<ModuleFileDefinition, File>(SPMeta2.Common.ModelEventType.OnUpdating, action);

            return model;
        }

        [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use ModernSyntax.OnProvisioning/OnProvisioned events.")]
        public static ModelNode OnCreated(this ModelNode model, Action<ModuleFileDefinition, File> action)
        {
            model.RegisterModelEvent<ModuleFileDefinition, File>(SPMeta2.Common.ModelEventType.OnUpdated, action);

            return model;
        }
    }
}
