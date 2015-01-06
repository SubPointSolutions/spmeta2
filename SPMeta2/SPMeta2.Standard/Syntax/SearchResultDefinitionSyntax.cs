using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class SearchResultDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddSearchResult(this ModelNode model, SearchResultDefinition definition)
        {
            return AddSearchResult(model, definition, null);
        }

        public static ModelNode AddSearchResult(this ModelNode model, SearchResultDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
