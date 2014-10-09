using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class HideContentTypeLinksDefinitionSyntax
    {
        #region methods


        public static ModelNode AddHideContentTypeLinks(this ModelNode model, HideContentTypeLinksDefinition definition)
        {
            return AddHideContentTypeLinks(model, definition, null);
        }

        public static ModelNode AddHideContentTypeLinks(this ModelNode model, HideContentTypeLinksDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
