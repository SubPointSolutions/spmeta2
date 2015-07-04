using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class UniqueContentTypeOrderModelNode : ListItemModelNode
    {

    }

    public static class UniqueContentTypeOrderDefinitionSyntax
    {
        #region methods


        public static ListModelNode AddUniqueContentTypeOrder(this ListModelNode model, UniqueContentTypeOrderDefinition definition)
        {
            return AddUniqueContentTypeOrder(model, definition, null);
        }

        public static ListModelNode AddUniqueContentTypeOrder(this ListModelNode model, UniqueContentTypeOrderDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
