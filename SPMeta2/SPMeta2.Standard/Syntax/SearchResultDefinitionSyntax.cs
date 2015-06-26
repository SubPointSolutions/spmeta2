using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Standard.Syntax
{
    public static class SearchResultDefinitionSyntax
    {
        #region publishing page

        public static SiteModelNode AddSearchResult(this SiteModelNode model, SearchResultDefinition definition)
        {
            return AddSearchResult(model, definition, null);
        }

        public static SiteModelNode AddSearchResult(this SiteModelNode model, SearchResultDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
