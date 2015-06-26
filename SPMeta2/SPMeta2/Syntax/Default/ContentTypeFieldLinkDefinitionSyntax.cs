using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class ContentTypeFieldLinkDefinitionSyntax
    {
        #region methods

        public static ContentTypeModelNode AddContentTypeFieldLink(this ContentTypeModelNode model, ContentTypeFieldLinkDefinition definition)
        {
            return AddContentTypeFieldLink(model, definition, null);
        }

        public static ContentTypeModelNode AddContentTypeFieldLink(this ContentTypeModelNode model, ContentTypeFieldLinkDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion


    }
}
