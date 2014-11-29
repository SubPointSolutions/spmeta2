using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class FarmDefinitionSyntax
    {
        #region methods

        public static ModelNode AddFarm(this ModelNode model, FarmDefinition definition)
        {
            return AddFarm(model, definition, null);
        }

        public static ModelNode AddFarm(this ModelNode model, FarmDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
