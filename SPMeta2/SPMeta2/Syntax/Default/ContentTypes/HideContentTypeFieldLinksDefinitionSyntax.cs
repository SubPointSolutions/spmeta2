using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class HideContentTypeFieldLinksModelNode : ListItemModelNode
    {

    }

    public static class HideContentTypeFieldLinksDefinitionSyntax
    {
        #region methods

        public static TModelNode AddHideContentTypeFieldLinks<TModelNode>(this TModelNode model, HideContentTypeFieldLinksDefinition definition)
            where TModelNode : ModelNode, IContentTypeModelNode, new()
        {
            return AddHideContentTypeFieldLinks(model, definition, null);
        }

        public static TModelNode AddHideContentTypeFieldLinks<TModelNode>(this TModelNode model, HideContentTypeFieldLinksDefinition definition,
            Action<HideContentTypeFieldLinksModelNode> action)
            where TModelNode : ModelNode, IContentTypeModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
