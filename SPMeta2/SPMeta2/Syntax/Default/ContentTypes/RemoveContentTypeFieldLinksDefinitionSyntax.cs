using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class RemoveContentTypeFieldLinksDefinitionSyntax
    {
        #region methods


        public static ContentTypeModelNode AddRemoveContentTypeFieldLinks(this ContentTypeModelNode model, RemoveContentTypeFieldLinksDefinition definition)
        {
            return AddRemoveContentTypeFieldLinks(model, definition, null);
        }

        public static ContentTypeModelNode AddRemoveContentTypeFieldLinks(this ContentTypeModelNode model, RemoveContentTypeFieldLinksDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
