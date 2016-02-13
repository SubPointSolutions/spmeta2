using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
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
