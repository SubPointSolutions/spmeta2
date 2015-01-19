using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class CustomDocumentIdProviderDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddCustomDocumentIdProvider(this ModelNode model, CustomDocumentIdProviderDefinition definition)
        {
            return AddCustomDocumentIdProvider(model, definition, null);
        }

        public static ModelNode AddCustomDocumentIdProvider(this ModelNode model, CustomDocumentIdProviderDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
