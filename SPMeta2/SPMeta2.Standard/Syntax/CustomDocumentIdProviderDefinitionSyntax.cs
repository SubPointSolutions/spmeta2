using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class CustomDocumentIdProviderModelNode : ListItemModelNode
    {

    }

    public static class CustomDocumentIdProviderDefinitionSyntax
    {
        #region publishing page

        public static SiteModelNode AddCustomDocumentIdProvider(this SiteModelNode model, CustomDocumentIdProviderDefinition definition)
        {
            return AddCustomDocumentIdProvider(model, definition, null);
        }

        public static SiteModelNode AddCustomDocumentIdProvider(this SiteModelNode model, CustomDocumentIdProviderDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
