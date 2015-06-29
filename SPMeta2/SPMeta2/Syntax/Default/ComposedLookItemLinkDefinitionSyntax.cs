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

        public static WebModelNode AddComposedLookItemLink(this WebModelNode model, ComposedLookItemLinkDefinition definition)
        {
            return AddComposedLookItemLink(model, definition, null);
        }

        public static WebModelNode AddComposedLookItemLink(this WebModelNode model, ComposedLookItemLinkDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        //public static WebModelNode AddComposedLookItemLinks(this WebModelNode model, IEnumerable<ComposedLookItemLinkDefinition> definitions)
        // {
        //     foreach (var definition in definitions)
        //         model.AddDefinitionNode(definition);

        //     return model;
        // }

        #endregion
    }
}
