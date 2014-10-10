using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class UniqueContentTypeFieldsOrderDefinitionSyntax
    {
        #region methods


        public static ModelNode AddUniqueContentTypeFieldsOrder(this ModelNode model, UniqueContentTypeFieldsOrderDefinition definition)
        {
            return AddUniqueContentTypeFieldsOrder(model, definition, null);
        }

        public static ModelNode AddUniqueContentTypeFieldsOrder(this ModelNode model, UniqueContentTypeFieldsOrderDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
