using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;

namespace SPMeta2.Syntax.Default
{
    public static class ComposedLookItemDefinitionSyntax
    {
        #region methods

        public static ModelNode AddComposedLookItem(this ModelNode model, ComposedLookItemDefinition definition)
        {
            return AddComposedLookItem(model, definition, null);
        }

        public static ModelNode AddComposedLookItem(this ModelNode model, ComposedLookItemDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddComposedLookItems(this ModelNode model, IEnumerable<ComposedLookItemDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
