using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class RemoveContentTypeFieldLinksModelNode : ListItemModelNode
    {

    }

    public static class RemoveContentTypeFieldLinksDefinitionSyntax
    {
        #region methods

        public static TModelNode AddRemoveContentTypeFieldLinks<TModelNode>(this TModelNode model, RemoveContentTypeFieldLinksDefinition definition)
            where TModelNode : ModelNode, IContentTypeModelNode, new()
        {
            return AddRemoveContentTypeFieldLinks(model, definition, null);
        }

        public static TModelNode AddRemoveContentTypeFieldLinks<TModelNode>(this TModelNode model, RemoveContentTypeFieldLinksDefinition definition,
            Action<RemoveContentTypeFieldLinksModelNode> action)
            where TModelNode : ModelNode, IContentTypeModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
