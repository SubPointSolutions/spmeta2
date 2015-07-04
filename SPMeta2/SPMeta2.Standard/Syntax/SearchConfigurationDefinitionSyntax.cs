using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class SearchConfigurationModelNode : TypedModelNode
    {

    }

    public static class SearchConfigurationDefinitionSyntax
    {
        #region publishing page

        public static SiteModelNode AddSearchConfiguration(this SiteModelNode model, SearchConfigurationDefinition definition)
        {
            return AddSearchConfiguration(model, definition, null);
        }

        public static SiteModelNode AddSearchConfiguration(this SiteModelNode model, SearchConfigurationDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
