using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;

namespace SPMeta2.Syntax.Default
{
    public static class ComposedLookItemLinkDefinitionSyntax
    {
        #region methods

        public static ModelNode AddComposedLookItemLink(this ModelNode model, ComposedLookItemLinkDefinition definition)
        {
            return AddComposedLookItemLink(model, definition, null);
        }

        public static ModelNode AddComposedLookItemLink(this ModelNode model, ComposedLookItemLinkDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddComposedLookItemLinks(this ModelNode model, IEnumerable<ComposedLookItemLinkDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
