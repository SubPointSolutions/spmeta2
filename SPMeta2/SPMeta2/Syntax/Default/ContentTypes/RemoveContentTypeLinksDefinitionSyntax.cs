using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class RemoveContentTypeLinksModelNode : ListItemModelNode
    {

    }

    public static class RemoveContentTypeLinksDefinitionSyntax
    {
        #region methods

        public static TModelNode AddRemoveContentTypeLinks<TModelNode>(this TModelNode model, RemoveContentTypeLinksDefinition definition)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return AddRemoveContentTypeLinks(model, definition, null);
        }

        public static TModelNode AddRemoveContentTypeLinks<TModelNode>(this TModelNode model, RemoveContentTypeLinksDefinition definition,
            Action<RemoveContentTypeLinksModelNode> action)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
