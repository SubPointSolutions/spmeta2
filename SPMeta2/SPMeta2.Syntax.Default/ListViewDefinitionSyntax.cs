using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class ListViewDefinitionSyntax
    {
        #region methods

        public static ModelNode AddView(this ModelNode model, ListViewDefinition definition)
        {
            return AddView(model, definition, null);
        }

        public static ModelNode AddView(this ModelNode model, ListViewDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
