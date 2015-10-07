using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class UniqueContentTypeFieldsOrderModelNode : ListItemModelNode
    {

    }

    public static class UniqueContentTypeFieldsOrderDefinitionSyntax
    {
        #region methods

        public static TModelNode AddUniqueContentTypeFieldsOrder<TModelNode>(this TModelNode model, UniqueContentTypeFieldsOrderDefinition definition)
            where TModelNode : ModelNode, IContentTypeModelNode, new()
        {
            return AddUniqueContentTypeFieldsOrder(model, definition, null);
        }

        public static TModelNode AddUniqueContentTypeFieldsOrder<TModelNode>(this TModelNode model, UniqueContentTypeFieldsOrderDefinition definition,
            Action<UniqueContentTypeFieldsOrderModelNode> action)
            where TModelNode : ModelNode, IContentTypeModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
