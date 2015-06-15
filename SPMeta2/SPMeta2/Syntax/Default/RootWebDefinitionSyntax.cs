using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class RootWebDefinitionSyntax
    {
        #region methods

        public static ModelNode AddRootWeb(this ModelNode model, RootWebDefinition definition)
        {
            return AddRootWeb(model, definition, null);
        }

        public static ModelNode AddRootWeb(this ModelNode model, RootWebDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region host override

        public static ModelNode AddHostRootWeb(this ModelNode model, RootWebDefinition definition)
        {
            return AddHostRootWeb(model, definition, null);
        }

        public static ModelNode AddHostRootWeb(this ModelNode model, RootWebDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
