using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class UniqueContentTypeOrderModelNode : ListItemModelNode
    {

    }

    public static class UniqueContentTypeOrderDefinitionSyntax
    {
        #region methods

        public static TModelNode AddUniqueContentTypeOrder<TModelNode>(this TModelNode model, UniqueContentTypeOrderDefinition definition)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return AddUniqueContentTypeOrder(model, definition, null);
        }

        public static TModelNode AddUniqueContentTypeOrder<TModelNode>(this TModelNode model, UniqueContentTypeOrderDefinition definition,
            Action<UniqueContentTypeOrderModelNode> action)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
