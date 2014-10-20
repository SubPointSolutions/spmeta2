using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class WebDefinitionSyntax
    {
        #region methods

        public static ModelNode AddWeb(this ModelNode model, WebDefinition definition)
        {
            return AddWeb(model, definition, null);
        }

        public static ModelNode AddWeb(this ModelNode model, WebDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region host override

        public static ModelNode AddHostWeb(this ModelNode model, WebDefinition definition)
        {
            return AddHostWeb(model, definition, null);
        }

        public static ModelNode AddHostWeb(this ModelNode model, WebDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
