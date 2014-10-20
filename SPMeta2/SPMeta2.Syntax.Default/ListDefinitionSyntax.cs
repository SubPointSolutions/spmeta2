using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class ListDefinitionSyntax
    {
        #region methods

        public static ModelNode AddList(this ModelNode model, ListDefinition definition)
        {
            return AddList(model, definition, null);
        }

        public static ModelNode AddList(this ModelNode model, ListDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region host override

        public static ModelNode AddHostList(this ModelNode model, ListDefinition definition)
        {
            return AddHostList(model, definition, null);
        }

        public static ModelNode AddHostList(this ModelNode model, ListDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
