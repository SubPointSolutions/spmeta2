using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class UniqueContentTypeOrderDefinitionSyntax
    {
        #region methods


        public static ModelNode AddUniqueContentTypeOrder(this ModelNode model, UniqueContentTypeOrderDefinition definition)
        {
            return AddUniqueContentTypeOrder(model, definition, null);
        }

        public static ModelNode AddUniqueContentTypeOrder(this ModelNode model, UniqueContentTypeOrderDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
