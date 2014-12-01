using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class HideContentTypeFieldLinksDefinitionSyntax
    {
        #region methods

        public static ModelNode AddHideContentTypeFieldLinks(this ModelNode model, HideContentTypeFieldLinksDefinition definition)
        {
            return AddHideContentTypeFieldLinks(model, definition, null);
        }

        public static ModelNode AddHideContentTypeFieldLinks(this ModelNode model, HideContentTypeFieldLinksDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

      
    }
}
