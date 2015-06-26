using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class WebConfigModificationDefinitionSyntax
    {
        #region methods


        public static WebApplicationModelNode AddWebConfigModification(this WebApplicationModelNode model, WebConfigModificationDefinition definition)
        {
            return AddWebConfigModification(model, definition, null);
        }

        public static WebApplicationModelNode AddWebConfigModification(this WebApplicationModelNode model, WebConfigModificationDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
