using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class HideContentTypeLinksModelNode : ListItemModelNode
    {

    }

    public static class HideContentTypeLinksDefinitionSyntax
    {
        #region methods

        public static TModelNode AddHideContentTypeLinks<TModelNode>(this TModelNode model, HideContentTypeLinksDefinition definition)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return AddHideContentTypeLinks(model, definition, null);
        }

        public static TModelNode AddHideContentTypeLinks<TModelNode>(this TModelNode model, HideContentTypeLinksDefinition definition,
            Action<HideContentTypeLinksModelNode> action)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
