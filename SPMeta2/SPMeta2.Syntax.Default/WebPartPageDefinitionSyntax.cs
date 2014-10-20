using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class WebPartPageDefinitionSyntax
    {
        #region methods

        public static ModelNode AddWebPartPage(this ModelNode model, WebPartPageDefinition definition)
        {
            return AddWebPartPage(model, definition, null);
        }

        public static ModelNode AddWebPartPage(this ModelNode model, WebPartPageDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region host override

        public static ModelNode AddHostWebPartPage(this ModelNode model, WebPartPageDefinition definition)
        {
            return AddHostWebPartPage(model, definition, null);
        }

        public static ModelNode AddHostWebPartPage(this ModelNode model, WebPartPageDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
